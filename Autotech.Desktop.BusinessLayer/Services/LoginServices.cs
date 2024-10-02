using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Core.DTO;
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
    public class LoginServices
    {
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = "https://localhost:7106/api/v1/Auth/Agents/Login";

                    var loginData = new AgentLoginDTO
                    {
                        Username = username,
                        Password = password
                    };

                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(apiUrl, loginData);

                    if (response.IsSuccessStatusCode)
                    {
                        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                        string jwtToken = loginResponse.Token;
                        SessionManager.StoreToken(jwtToken);
                        await FetchAgentDetailsAsync(loginResponse.Agent.Id);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task FetchAgentDetailsAsync(Guid agentId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionManager.Token);
                    var apiUrl = "https://localhost:7106/api/v1/Agents/";
                    HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}{agentId}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the response and store agent details
                        var agentDetails = await response.Content.ReadFromJsonAsync<Agents>();
                        SessionManager.StoreAgentDetails(agentDetails);
                    }
                    else
                    {
                        throw new Exception("Unable to fetch agent details");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
