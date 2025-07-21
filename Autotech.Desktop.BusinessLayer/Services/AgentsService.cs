using Autotech.Desktop.BusinessLayer.DTO;
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
    public class AgentsService
    {
        private readonly string uri = "https://api.autotechph.online/api/v1/Agents";
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

        public async Task UpdateAgentAsync(Agents agent)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await client.PutAsJsonAsync($"https://api.autotechph.online/api/v1/Agents/{agent.Id}", agent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update agent: {response.StatusCode} - {error}");
            }
        }

        public async Task AddAgentAsync(Agents agent)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);

            var response = await client.PostAsJsonAsync("https://api.autotechph.online/api/v1/Agents", agent);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to add agent: {error}");
            }
        }
    }
}
