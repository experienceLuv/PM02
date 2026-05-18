using System;
using System.Windows;
using OperatorClient.Services;

namespace OperatorClient.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await AuthService.LoginAsync(TxtLogin.Text, TxtPassword.Password);
                ApiClient.Instance.SetToken(result.Token);
                Application.Current.Properties["token"] = result.Token;

                var mainWindow = new MainWindow(result.FullName);
                Application.Current.MainWindow = mainWindow;  // 👈 вот эта строка решает проблему
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                LblError.Text = ex.Message;
            }
        }
    }
}