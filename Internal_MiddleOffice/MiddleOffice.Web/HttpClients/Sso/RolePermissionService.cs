using MiddleOffice.Entities.ViewModels.Sso;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddleOffice.Web.HttpClients.Sso
{
    public class RolePermissionService
    {
        public HttpClient _httpClient { get; }
        private IConfiguration _configuration;
        private readonly string _baseRoute = "/api/role-permission";

        public RolePermissionService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(_configuration["OAuthSettings:Domain"]);
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RolePermissionVM>> GetAllByRoleIdAsync(Guid roleId)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/by-roleId/{roleId}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<RolePermissionVM>>(responseStream);
        }
    }
}
