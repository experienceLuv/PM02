using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OperatorClient.Models;
using OperatorClient.Services;

namespace OperatorClient.Pages
{
    public partial class BatchProgramPage : UserControl
    {
        private int _batchId;
        private List<BatchStepExecution> _steps;
        private BatchStepExecution _selectedStep;

        // Конструктор без параметров (требуется WPF)
        public BatchProgramPage() : this(0) { }

        public BatchProgramPage(int batchId)
        {
            InitializeComponent();
            _batchId = batchId;
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            // Информация о партии
            if (_batchId > 0)
            {
                try
                {
                    var response = await ApiClient.Instance.GetAsync<PaginatedResponse<Batch>>("/api/batches");
                    var batch = response?.Items?.FirstOrDefault(b => b.Id == _batchId);
                    if (batch != null)
                    {
                        TxtBatchInfo.Text = $"Партия: {batch.BatchNumber}";
                        TxtBatchStatus.Text = $"Статус: {batch.StatusId}";
                    }
                }
                catch { }
            }

            // Шаги – всегда показываем статические, если API не вернул реальные
            _steps = await LoadStepsAsync();
            if (_steps == null || _steps.Count == 0)
                _steps = GetStaticSteps();

            LstSteps.ItemsSource = _steps;
            if (_steps.Count > 0)
            {
                LstSteps.SelectedIndex = 0;
                // Принудительно вызываем событие выбора, чтобы отобразились параметры
                LstSteps_SelectionChanged(null, null);
            }
        }

        private async Task<List<BatchStepExecution>> LoadStepsAsync()
        {
            try
            {
                var steps = await ApiClient.Instance.GetAsync<List<BatchStepExecution>>($"/api/batches/{_batchId}/steps");
                if (steps != null && steps.Count > 0)
                    return steps;
            }
            catch { }
            return null;
        }

        private List<BatchStepExecution> GetStaticSteps()
        {
            return new List<BatchStepExecution>
            {
                new BatchStepExecution { Id = 1, BatchId = _batchId, StepOrder = 1, StepName = "Смешивание", PlannedTempC = 45 },
                new BatchStepExecution { Id = 2, BatchId = _batchId, StepOrder = 2, StepName = "Выдержка", PlannedTempC = 60 },
                new BatchStepExecution { Id = 3, BatchId = _batchId, StepOrder = 3, StepName = "Экструзия", PlannedTempC = 80 }
            };
        }

        private void LstSteps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstSteps.SelectedItem is BatchStepExecution step)
            {
                _selectedStep = step;
                TxtStepName.Text = $"Шаг {step.StepOrder}: {step.StepName}";
                TxtPlannedTemp.Text = step.PlannedTempC?.ToString() ?? "–";
                TxtPlannedDuration.Text = step.PlannedDurationMin?.ToString() ?? "–";
                TxtPlannedPressure.Text = step.PlannedPressureBar?.ToString() ?? "–";
                TxtActualTemp.Text = step.ActualTempC?.ToString() ?? "";
                TxtActualDuration.Text = step.ActualDurationMin?.ToString() ?? "";
                TxtActualPressure.Text = step.ActualPressureBar?.ToString() ?? "";
                LblMessage.Text = "";
            }
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStep == null) return;
            try
            {
                await ApiClient.Instance.PostAsync($"/api/batches/{_batchId}/steps/{_selectedStep.Id}/start");
                LblMessage.Text = "Шаг начат";
            }
            catch (Exception ex) { LblMessage.Text = ex.Message; }
        }

        private async void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStep == null) return;
            if (!decimal.TryParse(TxtActualTemp.Text, out decimal temp) ||
                !int.TryParse(TxtActualDuration.Text, out int duration) ||
                !decimal.TryParse(TxtActualPressure.Text, out decimal pressure))
            {
                LblMessage.Text = "Заполните все фактические значения корректно";
                return;
            }

            try
            {
                await ApiClient.Instance.PutAsync($"/api/batches/{_batchId}/steps/{_selectedStep.Id}/complete",
                    new CompleteStepDto
                    {
                        ActualTemp = temp,
                        ActualDuration = duration,
                        ActualPressure = pressure,
                        Comment = "Выполнено оператором"
                    });
                LblMessage.Text = "Шаг завершён";
                await LoadData();
            }
            catch (Exception ex) { LblMessage.Text = ex.Message; }
        }
    }
}