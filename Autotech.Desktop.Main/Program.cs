using Autotech.Desktop.BusinessLayer.Helpers;
using Autotech.Desktop.Main.View;

namespace Autotech.Desktop.Main
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new SplashForm());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogHelper.Log("UI thread unhandled exception", e.Exception);
            MessageBox.Show("An unexpected error occurred. Details have been logged.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Log("Non-UI thread unhandled exception", e.ExceptionObject as Exception);
        }
    }
}