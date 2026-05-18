using System.Windows;
using TechnologistWpf.Services;
using TechnologistWpf.Views;

namespace TechnologistWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new ApiClient("https://localhost:7282"); // проверь порт
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}