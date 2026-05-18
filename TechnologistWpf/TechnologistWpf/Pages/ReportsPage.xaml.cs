using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Services;
using ClosedXML.Excel;

namespace TechnologistWpf.Pages
{
    public partial class ReportsPage : UserControl
    {
        private object _lastReportData;

        public ReportsPage()
        {
            InitializeComponent();
        }

        private async void BtnBatchReport_Click(object sender, RoutedEventArgs e)
        {
            var from = DpFrom.SelectedDate ?? DateTime.MinValue;
            var to = DpTo.SelectedDate ?? DateTime.MaxValue;
            var data = await ApiClient.Instance.GetAsync<List<dynamic>>($"/api/reports/batches?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
            DgReport.ItemsSource = data;
            _lastReportData = data;
        }

        private void BtnExportBatchReport_Click(object sender, RoutedEventArgs e) => ExportToCsv(_lastReportData as List<dynamic>, "batch_report.csv");
        private void BtnExportBatchExcel_Click(object sender, RoutedEventArgs e) => ExportToExcel(_lastReportData as List<dynamic>, "batch_report.xlsx");

        private async void BtnDeviationReport_Click(object sender, RoutedEventArgs e)
        {
            var from = DpFrom.SelectedDate ?? DateTime.MinValue;
            var to = DpTo.SelectedDate ?? DateTime.MaxValue;
            var data = await ApiClient.Instance.GetAsync<List<dynamic>>($"/api/reports/deviations?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}");
            DgReport.ItemsSource = data;
            _lastReportData = data;
        }

        private void BtnExportDeviationReport_Click(object sender, RoutedEventArgs e) => ExportToCsv(_lastReportData as List<dynamic>, "deviation_report.csv");
        private void BtnExportDeviationExcel_Click(object sender, RoutedEventArgs e) => ExportToExcel(_lastReportData as List<dynamic>, "deviation_report.xlsx");

        private async void BtnRecipeUsageReport_Click(object sender, RoutedEventArgs e)
        {
            var data = await ApiClient.Instance.GetAsync<List<dynamic>>("/api/reports/recipe-usage");
            DgReport.ItemsSource = data;
            _lastReportData = data;
        }

        private void BtnExportRecipeUsageReport_Click(object sender, RoutedEventArgs e) => ExportToCsv(_lastReportData as List<dynamic>, "recipe_usage_report.csv");
        private void BtnExportRecipeUsageExcel_Click(object sender, RoutedEventArgs e) => ExportToExcel(_lastReportData as List<dynamic>, "recipe_usage_report.xlsx");

        private async void BtnLabBlockReport_Click(object sender, RoutedEventArgs e)
        {
            var data = await ApiClient.Instance.GetAsync<List<dynamic>>("/api/reports/lab-blocks");
            DgReport.ItemsSource = data;
            _lastReportData = data;
        }

        private void BtnExportLabBlockReport_Click(object sender, RoutedEventArgs e) => ExportToCsv(_lastReportData as List<dynamic>, "lab_block_report.csv");
        private void BtnExportLabBlockExcel_Click(object sender, RoutedEventArgs e) => ExportToExcel(_lastReportData as List<dynamic>, "lab_block_report.xlsx");

        private void ExportToCsv(List<dynamic> data, string fileName)
        {
            if (data == null || data.Count == 0) { MessageBox.Show("Нет данных для экспорта."); return; }
            var first = (object)data[0];
            var props = first.GetType().GetProperties();
            var lines = new List<string> { string.Join(",", props.Select(p => p.Name)) };
            foreach (var item in data)
            {
                var values = props.Select(p => { try { return p.GetValue(item, null)?.ToString() ?? ""; } catch { return ""; } });
                lines.Add(string.Join(",", values));
            }
            System.IO.File.WriteAllLines(fileName, lines, System.Text.Encoding.UTF8);
            System.Diagnostics.Process.Start(fileName);
        }

        private void ExportToExcel(List<dynamic> data, string fileName)
        {
            if (data == null || data.Count == 0) { MessageBox.Show("Нет данных для экспорта."); return; }
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Отчёт");
            var first = (object)data[0];
            var props = first.GetType().GetProperties();
            for (int i = 0; i < props.Length; i++) worksheet.Cell(1, i + 1).Value = props[i].Name;
            for (int row = 0; row < data.Count; row++)
                for (int col = 0; col < props.Length; col++)
                    worksheet.Cell(row + 2, col + 1).Value = props[col].GetValue(data[row])?.ToString() ?? "";
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(fileName);
            System.Diagnostics.Process.Start(fileName);
        }
    }
}