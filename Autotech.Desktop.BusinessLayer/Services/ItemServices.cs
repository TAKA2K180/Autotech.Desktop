using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Autotech.Desktop.BusinessLayer.Helpers.PagingHelper;

namespace Autotech.Desktop.BusinessLayer.Services
{
    public class ItemServices
    {
        private static string apiUrl => $"{ApiConfig.BaseUrl}/Items";

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
                LogHelper.Log("Error: ", ex);
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

            var url = $"{apiUrl}/desktop/paginated?pageNumber={pageNumber}&pageSize={pageSize}";
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
                
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };
                
                var json = JsonSerializer.Serialize(item, options);
                LogHelper.Log($"Updating item with ID: {item.Id}\nPayload: {json}");

                var response = await httpClient.PutAsJsonAsync($"{apiUrl}/{item.Id}", item);

                if (response.IsSuccessStatusCode)
                {
                    LogHelper.Log($"Item {item.Id} updated successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    LogHelper.Log($"Update failed. Status: {response.StatusCode}, Error: {errorContent}");
                    return false;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Log("Error updating item: ", ex);
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
