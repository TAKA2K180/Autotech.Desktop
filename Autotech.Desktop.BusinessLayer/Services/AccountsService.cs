using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Services
{
    public class AccountService
    {
        private readonly string apiUrl = "https://api.autotechph.online/api/v1/Accounts"; // Adjust if needed

        public async Task<List<Accounts>> GetAllAccountsAsync()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Accounts>>();
            }

            throw new Exception("Failed to fetch accounts.");
        }

        public async Task UpdateAccountAsync(Accounts account)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await client.PutAsJsonAsync($"{apiUrl}/{account.Id}", account);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update account: {response.StatusCode} - {error}");
            }
        }

        public async Task<Accounts> GetAccountByIdAsync(Guid id)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await httpClient.GetAsync($"{apiUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Accounts>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to fetch account: {response.StatusCode} - {error}");
        }
    }
}
