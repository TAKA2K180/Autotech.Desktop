using Autotech.Desktop.BusinessLayer.DTO;
using Autotech.Desktop.BusinessLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Services
{
    public class AgentsService
    {
        private readonly string uri = "https://localhost:7106/api/v1/Agents";
        public async Task<List<AgentDTO>> GetAllAgentsAsync()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);
            var response = await client.GetAsync($"{uri}/Agents");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AgentDTO>>();
            }

            throw new Exception("Failed to fetch agents");
        }
    }
}
