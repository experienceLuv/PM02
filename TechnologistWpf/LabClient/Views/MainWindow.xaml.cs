using LabClient.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LabClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(string fullName)
        {
            InitializeComponent();
            TxtFullName.Text = fullName;
            Navigate("RawMaterialLots");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                try { DragMove(); }
                catch (InvalidOperationException) { /* окно не перемещается, когда не нужно */ }
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag as string;
            Navigate(tag);
        }

        private void Navigate(string tag)
        {
            switch (tag)
            {
                case "RawMaterialLots": MainFrame.Content = new RawMaterialLotsPage(); break;
                case "LabTests": MainFrame.Content = new LabTestsPage(); break;
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }

    }
}