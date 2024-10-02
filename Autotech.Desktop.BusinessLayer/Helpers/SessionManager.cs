using Autotech.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Helpers
{
    public static class SessionManager
    {
        public static string Token { get; private set; }
        public static Agents AgentDetails { get; private set; }

        public static void StoreToken(string token)
        {
            Token = token;
        }

        public static void StoreAgentDetails(Agents agent)
        {
            AgentDetails = agent;
        }

        public static void ClearSession()
        {
            AgentDetails = null;
            Token = null;
        }
    }
}
