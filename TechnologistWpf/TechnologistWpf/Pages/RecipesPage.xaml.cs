using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Pages
{
    public partial class RecipesPage : UserControl
    {
        private int _page = 1;
        private const int PageSize = 10;

        public RecipesPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var url = $"/api/recipes?page={_page}&pageSize={PageSize}";
                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<RecipeVersion>>(url);
                DgRecipes.ItemsSource = response.Items;
                TxtPageInfo.Text = $"Страница {_page} из {(int)Math.Ceiling((double)response.TotalCount / PageSize)}";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void BtnPrev_Click(object sender, RoutedEventArgs e) { if (_page > 1) { _page--; await LoadData(); } }
        private async void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; await LoadData(); }

        private async void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                try
                {
                    await ApiClient.Instance.PostAsync($"/api/recipes/{id}/approve");
                    MessageBox.Show("Рецептура утверждена");
                    await LoadData();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private async void BtnArchive_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is int id)
            {
                try
                {
                    await ApiClient.Instance.PutAsync($"/api/recipes/{id}/archive", new { });
                    MessageBox.Show("Рецептура архивирована");
                    await LoadData();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
    }
}