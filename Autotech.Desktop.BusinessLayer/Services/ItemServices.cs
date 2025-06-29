using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Autotech.Desktop.BusinessLayer.Helpers.PagingHelper;

namespace Autotech.Desktop.BusinessLayer.Services
{
    public class ItemServices
    {
        private readonly string apiUrl = "https://localhost:7106/api/v1/Items";

        public async Task<List<Items>> GetAllItemsAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Add JWT token to the request headers for authentication if required
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize and return the list of items
                        return await response.Content.ReadFromJsonAsync<List<Items>>();
                    }
                    else
                    {
                        throw new Exception("Unable to fetch items from the server.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors, for example, logging the exception
                throw;
            }
        }
        public async Task<Items> GetItemByIdAsync(Guid id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);

                    HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<Items>();
                    }

                    throw new Exception("Item not found.");
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Items>> GetPaginatedItemsAsync(int pageNumber, int pageSize)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var url = $"https://localhost:7106/api/v1/Items/paginated?pageNumber={pageNumber}&pageSize={pageSize}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginatedResponse<Items>>();
                return result?.Items ?? new List<Items>();
            }

            throw new Exception("Failed to retrieve paginated items.");
        }

        public async Task<bool> UpdateItemAsync(Items item)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);
                var json = JsonSerializer.Serialize(item, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var response = await httpClient.PutAsJsonAsync($"{apiUrl}/{item.Id}", item);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateBulkItemsAsync(List<ItemRequestDto> items)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);

                var response = await client.PostAsJsonAsync(apiUrl, items);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }
    }

}
