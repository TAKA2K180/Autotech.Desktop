using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Helpers
{
    public class LogHelper
    {
        private static readonly object _lock = new object();
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "errors.log");

        static LogHelper()
        {
            string logDir = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
        }

        public static void Log(string message, Exception ex = null)
        {
            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("==================================");
                    writer.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine($"Message: {message}");
                    if (ex != null)
                    {
                        writer.WriteLine($"Exception: {ex.Message}");
                        writer.WriteLine($"StackTrace: {ex.StackTrace}");
                    }
                    writer.WriteLine("==================================");
                    writer.WriteLine();
                }
            }
        }
    }
}
