using System.Windows;
using LabClient.Services;
using LabClient.Views;

namespace LabClient
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Укажи свой адрес API (тот же, что для технолога)
            new ApiClient("https://localhost:7282");
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}