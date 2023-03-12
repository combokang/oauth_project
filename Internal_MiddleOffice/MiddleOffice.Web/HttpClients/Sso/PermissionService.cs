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
    public class PermissionService
    {
        public HttpClient _httpClient { get; }
        private IConfiguration _configuration;
        private readonly string _baseRoute = "/api/permission";

        public PermissionService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(_configuration["OAuthSettings:Domain"]);
            _httpClient = httpClient;
        }

        public async Task AddAsync(string candidateKey, string name, string description, Guid projectId)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  CandidateKey = candidateKey,
                  Name = name,
                  Description = description,
                  ProjectId = projectId
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task EditAsync(Guid id, string candidateKey, string name, string description)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id,
                  CandidateKey = candidateKey,
                  Name = name,
                  Description = description,
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PutAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<PermissionListVM>> GetPagedAllAsync(Guid projectId, int pageIndex, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/paged?projectId={projectId}&pageIndex={pageIndex}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<PermissionListVM>>(responseStream);
        }

        public async Task<Permission> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/{id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Permission>(responseStream);
        }

        public async Task<IEnumerable<Permission>> GetAllByProjectIdAsync(Guid projectId)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/by-projectId/{projectId}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<Permission>>(responseStream);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var httpResponse = await _httpClient.DeleteAsync($"{_baseRoute}/{id}");

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
