using Internal.SSO.IServices;
using Internal.SSO.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Internal.SSO.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProjectCodeService, ProjectCodeService>();
            services.AddTransient<IRolePermissionService, RolePermissionService>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IUserAccessLogService, UserAccessLogService>();

            services
                .AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", config =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(Configuration["Constants:Secret"]);
                    var key = new SymmetricSecurityKey(secretBytes);

                    config.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.ContainsKey("access_token")) //¤ä´©±qquery string 
                            {
                                context.Token = context.Request.Query["access_token"];
                            }

                            return Task.CompletedTask;
                        }
                    };

                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = Configuration["Constants:Issuer"],
                        ValidAudience = Configuration["Constants:Audience"],
                        IssuerSigningKey = key,
                    };
                });

            services.AddControllersWithViews(opt =>
            {
                // remove formatter that turns nulls into 204 - No Content responses
                opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs);
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Staging") || env.IsEnvironment("Uat"))
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
