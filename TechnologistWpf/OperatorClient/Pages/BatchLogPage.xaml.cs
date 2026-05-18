using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OperatorClient.Models;
using OperatorClient.Services;

namespace OperatorClient.Pages
{
    public partial class BatchLogPage : UserControl
    {
        public BatchLogPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadLog();
        }

        private async Task LoadLog()
        {
            try
            {
                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<AuditLog>>("/api/audit-logs?pageSize=50");
                DgAudit.ItemsSource = response?.Items ?? new List<AuditLog>();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}