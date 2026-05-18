using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;
using TechnologistWpf.Views;

namespace TechnologistWpf.Pages
{
    public partial class ProductsPage : UserControl
    {
        private int _page = 1;
        private const int PageSize = 10;
        private string _search = "";

        public ProductsPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var url = $"/api/products?page={_page}&pageSize={PageSize}";
                if (!string.IsNullOrWhiteSpace(_search))
                    url += $"&search={Uri.EscapeDataString(_search)}";

                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<Product>>(url);
                DgProducts.ItemsSource = response.Items;
                TxtPageInfo.Text = $"Страница {_page} из {(int)Math.Ceiling((double)response.TotalCount / PageSize)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            _search = TxtSearch.Text;
            _page = 1;
            await LoadData();
        }

        private async void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _search = "";
            _page = 1;
            TxtSearch.Text = "";
            await LoadData();
        }

        private async void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (_page > 1) { _page--; await LoadData(); }
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            _page++; await LoadData();
        }

        private async void BtnArchive_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                try
                {
                    // Архивация через PUT /api/products/{id}/archive
                    await ApiClient.Instance.PutAsync($"/api/products/{id}/archive", new { });
                    MessageBox.Show("Продукт архивирован");
                    await LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
        }

        private void DgProducts_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DgProducts.SelectedItem is Product product)
            {
                var dialog = new ProductEditWindow(product);
                if (dialog.ShowDialog() == true)
                    _ = LoadData();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductEditWindow(null);
            if (dialog.ShowDialog() == true)
                _ = LoadData();
        }
    }
}