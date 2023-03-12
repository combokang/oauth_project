using MiddleOffice.Entities.Models.Sso;
using MiddleOffice.Entities.ViewModels.Sso;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddleOffice.Web.HttpClients.Sso
{
    public class ProjectService
    {
        public HttpClient _httpClient { get; }
        private IConfiguration _configuration;
        private readonly string _baseRoute = "/api/project";

        public ProjectService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(_configuration["OAuthSettings:Domain"]);
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProjectListVM>> GetPagedAllAsync(string name, int pageIndex, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/paged?name={name}&pageIndex={pageIndex}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<ProjectListVM>>(responseStream);
        }
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<Project>>(responseStream);
        }
        public async Task<IEnumerable<ProjectInfoListVM>> GetInfoAllAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/getinfoall");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<ProjectInfoListVM>>(responseStream);
        }


        public async Task AddAsync(string name, string domain, string callbackUrls)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Name = name,
                  Domain = domain,
                  CallbackUrls = callbackUrls
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task<Project> GetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/{id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Project>(responseStream);
        }

        public async Task EditAsync(Guid id, string name, string domain, string callbackUrls)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id,
                  Name = name,
                  Domain = domain,
                  CallbackUrls = callbackUrls
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PutAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task DownloadReportAsync(string name, Stream outStream)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Name = name
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}/export", data);

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            responseStream.Seek(0, SeekOrigin.Begin);
            responseStream.CopyTo(outStream);
        }
        public async Task DeleteAsync(Guid id)
        {
            using var httpResponse = await _httpClient.DeleteAsync($"{_baseRoute}/{id}");

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
