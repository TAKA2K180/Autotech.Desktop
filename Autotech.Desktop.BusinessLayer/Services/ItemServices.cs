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
    }
}
