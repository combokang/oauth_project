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
    public class RoleService
    {
        public HttpClient _httpClient { get; }
        private IConfiguration _configuration;
        private readonly string _baseRoute = "/api/role";

        public RoleService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(_configuration["OAuthSettings:Domain"]);
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RoleListVM>> GetPagedAllAsync(Guid projectId, int pageIndex, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/paged?projectId={projectId}&pageIndex={pageIndex}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<RoleListVM>>(responseStream);
        }

        public async Task AddAsync(string name, string description, Guid projectId, List<Guid> permissionIds)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Name = name,
                  Description = description,
                  ProjectId = projectId,
                  PermissionIds = permissionIds
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task<Role> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/{id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Role>(responseStream);
        }

        public async Task EditAsync(Guid id, string name, string description, List<Guid> permissionIds)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id,
                  Name = name,
                  Description = description,
                  PermissionIds = permissionIds
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PutAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            using var httpResponse = await _httpClient.DeleteAsync($"{_baseRoute}/{id}");

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
