using System.Windows;
using OperatorClient.Services;
using OperatorClient.Views;

namespace OperatorClient
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // 👇 Укажи порт своего API
            new ApiClient("https://localhost:7282");
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}