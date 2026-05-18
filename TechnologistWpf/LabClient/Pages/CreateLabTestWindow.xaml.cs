using LabClient.Models;
using LabClient.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LabClient.Views
{
    public partial class CreateLabTestWindow : Window
    {
        private readonly RawMaterialLot _lot;

        public CreateLabTestWindow(RawMaterialLot lot)
        {
            InitializeComponent();
            _lot = lot;
            TxtLotInfo.Text = $"Партия №{lot.LotNumber}, материал {lot.RawMaterialId}";
        }

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var test = new LabTest
            {
                RawMaterialLotId = _lot.Id,
                SampleType = ((ComboBoxItem)CmbType.SelectedItem).Content.ToString(),
                ParameterName = TxtParamName.Text,
                StandardValue = TxtStandard.Text,
                Unit = "",
                Result = "",
                Decision = null,
                AnalystComment = TxtComment.Text
            };
            try
            {
                await ApiClient.Instance.PostAsync("/api/lab-tests", test);
                DialogResult = true;
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}