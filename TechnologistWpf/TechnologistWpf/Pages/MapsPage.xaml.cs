using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;
using TechnologistWpf.Views;

namespace TechnologistWpf.Pages
{
    public partial class MapsPage : UserControl
    {
        private int _page = 1;
        private const int PageSize = 10;

        public MapsPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var url = $"/api/maps?page={_page}&pageSize={PageSize}";
                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<TechnologicalMap>>(url);
                DgMaps.ItemsSource = response.Items;
                TxtPageInfo.Text = $"Страница {_page} из {(int)Math.Ceiling((double)response.TotalCount / PageSize)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private async void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_page > 1) { _page--; await LoadData(); }
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _page++; await LoadData();
        }

        private async void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                try
                {
                    await ApiClient.Instance.PostAsync($"/api/maps/{id}/approve");
                    MessageBox.Show("Техкарта утверждена");
                    await LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MapEditWindow();
            if (dialog.ShowDialog() == true)
                _ = LoadData();
        }

        private void DgMaps_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DgMaps.SelectedItem is TechnologicalMap map)
            {
                var dialog = new MapEditWindow(map);
                if (dialog.ShowDialog() == true)
                    _ = LoadData();
            }
        }
    }
}