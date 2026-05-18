using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Pages
{
    public partial class BatchesPage : UserControl
    {
        private int _page = 1;
        private const int PageSize = 10;

        public BatchesPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var url = $"/api/batches?page={_page}&pageSize={PageSize}";
                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<Batch>>(url);
                DgBatches.ItemsSource = response.Items;
                TxtPageInfo.Text = $"Страница {_page} из {(int)Math.Ceiling((double)response.TotalCount / PageSize)}";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private async void BtnPrev_Click(object sender, RoutedEventArgs e) { if (_page > 1) { _page--; await LoadData(); } }
        private async void BtnNext_Click(object sender, RoutedEventArgs e) { _page++; await LoadData(); }
    }
}