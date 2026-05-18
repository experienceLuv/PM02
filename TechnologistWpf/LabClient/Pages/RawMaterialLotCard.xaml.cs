using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LabClient.Models;
using LabClient.Services;

namespace LabClient.Views
{
    public partial class RawMaterialLotCard : Window
    {
        private readonly int _lotId;
        private RawMaterialLot _lot;

        public RawMaterialLotCard(int lotId)
        {
            InitializeComponent();
            _lotId = lotId;
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                _lot = await ApiClient.Instance.GetAsync<RawMaterialLot>($"/api/raw-material-lots/{_lotId}");
                if (_lot == null)
                {
                    MessageBox.Show("Партия не найдена");
                    Close();
                    return;
                }
                PanelLotInfo.Children.Clear();
                PanelLotInfo.Children.Add(new TextBlock { Text = $"Номер партии: {_lot.LotNumber}" });
                PanelLotInfo.Children.Add(new TextBlock { Text = $"Материал ID: {_lot.RawMaterialId}" });
                PanelLotInfo.Children.Add(new TextBlock { Text = $"Поставщик: {_lot.Supplier}" });
                PanelLotInfo.Children.Add(new TextBlock { Text = $"Дата поступления: {_lot.ReceiptDate:dd.MM.yyyy}" });
                PanelLotInfo.Children.Add(new TextBlock { Text = $"Количество: {_lot.Quantity} {_lot.Unit}" });
                PanelLotInfo.Children.Add(new TextBlock { Text = $"Статус: {_lot.StatusId}" });

                var tests = await ApiClient.Instance.GetAsync<List<LabTest>>($"/api/lab-tests?rawMaterialLotId={_lotId}");
                DgTests.ItemsSource = tests ?? new List<LabTest>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void BtnCreateTest_Click(object sender, RoutedEventArgs e)
        {
            if (_lot == null) return;
            var createWindow = new CreateLabTestWindow(_lot);
            if (createWindow.ShowDialog() == true)
                await LoadData();
        }

        private async void BtnDecision_Click(object sender, RoutedEventArgs e)
        {
            var tests = DgTests.ItemsSource as List<LabTest>;
            if (tests == null || tests.Count == 0 || tests.Any(t => string.IsNullOrEmpty(t.Decision)))
            {
                MessageBox.Show("Не все испытания завершены. Невозможно принять решение.");
                return;
            }
            bool allPassed = tests.All(t => t.Result == "pass");
            string decision = allPassed ? "approved" : "blocked";
            string comment = "";

            if (decision == "blocked")
            {
                var inputDialog = new InputDialog("Введите причину блокировки:");
                if (inputDialog.ShowDialog() == true)
                {
                    comment = inputDialog.Result;
                    if (string.IsNullOrWhiteSpace(comment))
                    {
                        MessageBox.Show("При блокировке обязательно указать причину.");
                        return;
                    }
                }
                else
                {
                    return; // отмена
                }
            }

            try
            {
                int newStatus = allPassed ? 14 : 15;
                await ApiClient.Instance.PutAsync($"/api/raw-material-lots/{_lotId}/status", new { StatusId = newStatus });
                MessageBox.Show("Решение принято.");
                await LoadData();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}