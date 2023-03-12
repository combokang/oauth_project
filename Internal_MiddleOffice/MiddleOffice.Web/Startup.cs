using MiddleOffice.Web.Handlers;
using MiddleOffice.Web.HttpClients.Sso;
using MiddleOffice.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiddleOffice.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpContextAccessor();

            services.AddAuthentication(config =>
            {

                config.DefaultAuthenticateScheme = "MiddleOffice";
                config.DefaultSignInScheme = "MiddleOffice";
                config.DefaultChallengeScheme = "Bearer";
            })
              .AddCookie("MiddleOffice")
              .AddOAuth("Bearer", config =>
              {
                  config.ClientId = Configuration["OAuthSettings:ClientId"];
                  config.ClientSecret = Configuration["OAuthSettings:ClientSecret"];
                  config.CallbackPath = "/oauth/callback";
                  config.AuthorizationEndpoint = Configuration["OAuthSettings:AuthorizationEndpoint"];
                  config.TokenEndpoint = Configuration["OAuthSettings:TokenEndpoint"];
                  config.SaveTokens = true;

                  config.Events = new OAuthEvents()
                  {
                      OnCreatingTicket = context =>
                      {
                          var accessToken = context.AccessToken;
                          var base64payload = accessToken.Split('.')[1];

                          switch (base64payload.Length % 4)
                          {
                              case 2: base64payload += "=="; break;
                              case 3: base64payload += "="; break;
                          }

                          var bytes = Convert.FromBase64String(base64payload);
                          var jsonPayload = Encoding.UTF8.GetString(bytes);
                          var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonPayload);

                          context.Identity.AddClaims(claims.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

                          return Task.CompletedTask;
                      }
                  };
              });

            services.AddTransient<TokenHandler>();
            services.AddHttpClient<ProjectService>().AddHttpMessageHandler<TokenHandler>();
            services.AddHttpClient<RoleService>().AddHttpMessageHandler<TokenHandler>();
            services.AddHttpClient<PermissionService>().AddHttpMessageHandler<TokenHandler>();
            services.AddHttpClient<UserService>().AddHttpMessageHandler<TokenHandler>();
            services.AddHttpClient<RolePermissionService>().AddHttpMessageHandler<TokenHandler>();
            services.AddHttpClient<UserRoleService>().AddHttpMessageHandler<TokenHandler>();

            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddSession();
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Staging"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions()
                {
                    ExceptionHandler = async context =>
                    {
                        var isApi = Regex.IsMatch(context.Request.Path.Value, "^/api/", RegexOptions.IgnoreCase);
                        var isAjax = !string.IsNullOrEmpty(context.Request.Headers["x-requested-with"]) && context.Request.Headers["x-requested-with"][0].ToLower() == "xmlhttprequest";

                        if (isApi || isAjax)
                        {
                            context.Response.ContentType = "application/json";
                            var json = @"{ ""Message"": ""Internal Server Error"" }";
                            await context.Response.WriteAsync(json);
                            return;
                        }

                        context.Response.Redirect("/Error/InternalServerErrorPage");
                    }
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages(context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == (int)HttpStatusCode.NotFound) { response.Redirect("/Error/NotFoundPage"); }

                return Task.CompletedTask;
            });



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
