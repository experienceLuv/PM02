using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Pages
{
    public partial class DashboardPage : UserControl
    {
        public DashboardPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadDashboardAsync();
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                var products = await ApiClient.Instance.GetAsync<PaginatedResponse<Product>>("/api/products?pageSize=1");
                TxtActiveProducts.Text = products.TotalCount.ToString();

                var recipes = await ApiClient.Instance.GetAsync<PaginatedResponse<RecipeVersion>>("/api/recipes?pageSize=100");
                TxtActiveRecipes.Text = recipes.Items?.Count(r => r.IsActive).ToString() ?? "0";

                var orders = await ApiClient.Instance.GetAsync<PaginatedResponse<ProductionOrder>>("/api/orders?pageSize=100");
                TxtActiveOrders.Text = orders.Items?.Count(o => o.StatusId == 6).ToString() ?? "0";

                var audits = await ApiClient.Instance.GetAsync<PaginatedResponse<AuditLog>>("/api/audit-logs?pageSize=10");
                LstEvents.ItemsSource = audits.Items?.Select(a => $"[{a.ChangedAt:dd.MM.yyyy HH:mm}] {a.Action}").ToList();
            }
            catch { }
        }
    }
}