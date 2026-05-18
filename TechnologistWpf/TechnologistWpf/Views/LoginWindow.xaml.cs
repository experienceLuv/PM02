using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TechnologistWpf.Services;

namespace TechnologistWpf.Views
{
    public partial class LoginWindow : Window
    {
        private string _captchaKey;

        public LoginWindow()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadCaptcha();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private async Task LoadCaptcha()
        {
            try
            {
                var (bytes, key) = await AuthService.GetCaptchaAsync();
                using (var ms = new MemoryStream(bytes))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    ImgCaptcha.Source = image;
                }
                _captchaKey = key;
                LblCaptchaError.Text = "";
            }
            catch (Exception ex)
            {
                LblCaptchaError.Text = "Ошибка капчи: " + ex.Message;
            }
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var login = TxtLogin.Text.Trim();
            var password = TxtPassword.Password;
            var captchaText = TxtCaptcha.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                LblError.Text = "Введите логин и пароль";
                return;
            }

            try
            {
                var result = await AuthService.LoginAsync(login, password, _captchaKey, captchaText);
                ApiClient.Instance.SetToken(result.Token);
                Application.Current.Properties["token"] = result.Token;
                var mainWindow = new MainWindow(result.FullName);
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                LblError.Text = ex.Message;
                await LoadCaptcha();
            }
        }
    }
}