using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TechnologistWpf.Pages;

namespace TechnologistWpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string fullName)
        {
            InitializeComponent();
            TxtFullName.Text = fullName;
            Navigate("Dashboard");
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
                case "Dashboard": MainFrame.Content = new DashboardPage(); break;
                case "Products": MainFrame.Content = new ProductsPage(); break;
                case "Recipes": MainFrame.Content = new RecipesPage(); break;
                case "Maps": MainFrame.Content = new MapsPage(); break;
                case "Orders": MainFrame.Content = new OrdersPage(); break;
                case "Batches": MainFrame.Content = new BatchesPage(); break;
                case "Extruder": MainFrame.Content = new ExtruderPage(); break;
                case "Audit": MainFrame.Content = new AuditPage(); break;
                case "Reports": MainFrame.Content = new ReportsPage(); break;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}