using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OperatorClient.Pages;

namespace OperatorClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string fullName)
        {
            InitializeComponent();
            TxtFullName.Text = fullName;
            Navigate("ActiveBatches");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate(((Button)sender).Tag.ToString());
        }

        private void Navigate(string tag)
        {
            switch (tag)
            {
                case "ActiveBatches": MainFrame.Content = new ActiveBatchesPage(); break;
                case "BatchProgram": MainFrame.Content = new BatchProgramPage(0); break;
                case "Extruder": MainFrame.Content = new ExtruderPage(); break;
                case "BatchLog": MainFrame.Content = new BatchLogPage(); break;
                case "ReportProblem": MainFrame.Content = new ReportProblemPage(); break;
            }
        }

        public void NavigateToBatchProgram(int batchId)
        {
            MainFrame.Content = new BatchProgramPage(batchId);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}