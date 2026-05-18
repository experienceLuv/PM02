using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Pages
{
    public partial class ExtruderPage : UserControl
    {
        public ExtruderPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadBatches();
            SldTemp1.ValueChanged += (s, e) => TxtTemp1.Text = ((int)SldTemp1.Value).ToString();
            SldTemp2.ValueChanged += (s, e) => TxtTemp2.Text = ((int)SldTemp2.Value).ToString();
            SldTemp3.ValueChanged += (s, e) => TxtTemp3.Text = ((int)SldTemp3.Value).ToString();
            SldPressure.ValueChanged += (s, e) => TxtPressure.Text = SldPressure.Value.ToString("F1");
            SldSpeed.ValueChanged += (s, e) => TxtSpeed.Text = ((int)SldSpeed.Value).ToString();
        }

        private async Task LoadBatches()
        {
            try
            {
                var response = await ApiClient.Instance.GetAsync<PaginatedResponse<Batch>>("/api/batches?pageSize=100");
                var activeBatches = response.Items.Where(b => b.StatusId == 6 || b.StatusId == 10).ToList();
                CmbBatch.ItemsSource = activeBatches;
                CmbBatch.DisplayMemberPath = "BatchNumber";
                CmbBatch.SelectedValuePath = "Id";
                if (activeBatches.Any())
                    CmbBatch.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void CmbBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Здесь можно загрузить сохранённые параметры для выбранной партии (пока оставим значения по умолчанию)
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbBatch.SelectedValue == null)
            {
                MessageBox.Show("Выберите партию");
                return;
            }
            try
            {
                // Имитация сохранения — можно отправить на API или сохранить локально
                await Task.Delay(300); // имитация задержки
                LblMessage.Text = "Программа сохранена для партии " + CmbBatch.Text;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}