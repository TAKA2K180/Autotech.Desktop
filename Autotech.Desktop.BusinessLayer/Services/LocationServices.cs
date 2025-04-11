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
    public class LocationServices
    {
        private readonly string apiUrl = "https://localhost:7106/api/v1/Locations";

        public async Task<List<Locations>> GetAllLocationsAsync()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", SessionManager.Token);

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<List<Locations>>();
                    }

                    throw new Exception("Failed to fetch locations.");
                }
            }
            catch (Exception ex)
            {
                // Optional: Log exception
                throw new ApplicationException("An error occurred while fetching locations.", ex);
            }
        }
    }
}
