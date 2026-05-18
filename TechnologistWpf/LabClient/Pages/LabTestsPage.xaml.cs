using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LabClient.Models;
using LabClient.Services;

namespace LabClient.Pages
{
    public partial class LabTestsPage : UserControl
    {
        public LabTestsPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var tests = await ApiClient.Instance.GetAsync<List<LabTest>>("/api/lab-tests");
                DgTests.ItemsSource = tests ?? new List<LabTest>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}