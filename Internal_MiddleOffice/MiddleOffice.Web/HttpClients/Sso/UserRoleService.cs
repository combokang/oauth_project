using MiddleOffice.Entities.Models.Sso;
using MiddleOffice.Entities.ViewModels.Sso;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddleOffice.Web.HttpClients.Sso
{
    public class UserRoleService
    {
        public HttpClient _httpClient { get; }
        private IConfiguration _configuration;
        private readonly string _baseRoute = "/api/user-role";

        public UserRoleService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(_configuration["OAuthSettings:Domain"]);
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserRoleVM>> GetAllAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}?userId={userId}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<UserRoleVM>>(responseStream);
        }

        public async Task AddAsync(string userId, Guid roleId)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  UserId = userId,
                  RoleId = roleId
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task EditAsync(string userId, List<UserRole> entities)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  UserId = userId,
                  UserRoles = entities
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PutAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            using var httpResponse = await _httpClient.DeleteAsync($"{_baseRoute}/{id}");

            httpResponse.EnsureSuccessStatusCode();
        }

    }
}
