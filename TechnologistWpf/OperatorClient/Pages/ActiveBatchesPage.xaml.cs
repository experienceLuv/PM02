using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OperatorClient.Models;
using OperatorClient.Services;

namespace OperatorClient.Pages
{
    public partial class ActiveBatchesPage : UserControl
    {
        public ActiveBatchesPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadBatches();
        }

        private async Task LoadBatches()
        {
            try
            {
                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<Batch>>("/api/batches?pageSize=100");
                var active = response?.Items?.Where(b => b.StatusId == 6 || b.StatusId == 10).ToList();
                DgBatches.ItemsSource = active ?? new List<Batch>();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки партий");
            }
        }

        private void DgBatches_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DgBatches.SelectedItem is Batch batch)
            {
                var mainWindow = Application.Current.MainWindow as Views.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.NavigateToBatchProgram(batch.Id);
                }
                else
                {
                    MessageBox.Show("Главное окно не найдено");
                }
            }
            else
            {
                MessageBox.Show("Не удалось определить выбранную партию");
            }
        }
    }
}