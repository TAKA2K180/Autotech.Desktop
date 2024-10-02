using Autotech.Desktop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Helpers
{
    public class LoginResponse
    {
        public Agents Agent { get; set; }
        public string Token { get; set; }
    }
}
