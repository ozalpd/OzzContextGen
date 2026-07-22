using OzzContextGen.WPF.Models;
using System.Globalization;
using System.Windows;

namespace OzzContextGen.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var settings = AppSettings.GetAppSettings();

            if (!string.IsNullOrWhiteSpace(settings.UiCulture))
            {
                var culture = new CultureInfo(settings.UiCulture);
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
            }

            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--disable-features=msWebView2EnableDrm");
        }
    }
}
