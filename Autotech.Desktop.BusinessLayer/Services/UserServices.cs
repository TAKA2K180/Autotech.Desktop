using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Services
{
    public class UserServices
    {
        private readonly string apiUrl = "https://api.autotechph.online/api/v1/User";

        public async Task<Agents> GetCurrentUserAsync()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync($"{apiUrl}/me");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Agents>();
            }

            throw new Exception("Unable to retrieve user information.");
        }

        public async Task<List<Agents>> GetAllUsersAsync()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Agents>>();
            }

            throw new Exception("Failed to retrieve users.");
        }

        public async Task<bool> CreateUserAsync(Agents agents)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var content = JsonContent.Create(agents);
            var response = await httpClient.PostAsync(apiUrl, content);

            return response.IsSuccessStatusCode;
        }
    }
}
