using MiddleOffice.Entities.Models.Sso;
using MiddleOffice.Entities.ViewModels.Sso;
using MiddleOffice.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddleOffice.Web.HttpClients.Sso
{
    public class UserService
    {
        public HttpClient _httpClient { get; }
        private IConfiguration _configuration;
        private readonly string _baseRoute = "/api/user";

        public UserService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            httpClient.BaseAddress = new Uri(_configuration["OAuthSettings:Domain"]);
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserListVM>> GetPagedAllAsync(string id, int pageIndex, int pageSize)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/paged?id={id}&pageIndex={pageIndex}&pageSize={pageSize}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<UserListVM>>(responseStream);
        }

        public async Task AddAsync(string id, string password, string name, string email)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id,
                  Password = password,
                  Name = name,
                  Email = email
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task<User> GetAsync(string id)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/{id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<User>(responseStream);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var response = await _httpClient.GetAsync($"{_baseRoute}/exists?id={id}");

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<bool>(responseStream);
        }

        public async Task EditAsync(string id, string password, string name, string email)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id,
                  Password = password,
                  Name = name,
                  Email = email
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PutAsync($"{_baseRoute}", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            using var httpResponse = await _httpClient.DeleteAsync($"{_baseRoute}/{id}");

            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task UnlockAsync(string id)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}/unlock", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task DownloadReportAsync(string id, Stream outStream)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  Id = id
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}/export", data);

            response.EnsureSuccessStatusCode();

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            responseStream.Seek(0, SeekOrigin.Begin);
            responseStream.CopyTo(outStream);
        }

        public async Task<HttpStatusCode> CheckPermissionAsync(string permissionKey)
        {
            var data = new StringContent(
              JsonSerializer.Serialize(new
              {
                  ClientId = _configuration["OAuthSettings:ClientId"],
                  PermissionKey= permissionKey
              }),
              Encoding.UTF8,
              "application/json");

            var response = await _httpClient.PostAsync($"{_baseRoute}/permission", data);

            return response.StatusCode;
        }
    }
}
