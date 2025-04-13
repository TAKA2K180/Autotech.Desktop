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
        private readonly string apiUrl = "https://localhost:7106/api/v1/Accounts"; // Adjust if needed

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
    }
}
