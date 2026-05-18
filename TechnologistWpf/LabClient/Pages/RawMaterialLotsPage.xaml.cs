using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LabClient.Models;
using LabClient.Services;

namespace LabClient.Pages
{
    public partial class RawMaterialLotsPage : UserControl
    {
        private List<RawMaterialLot> _allLots = new List<RawMaterialLot>();
        private string _search = "";

        public RawMaterialLotsPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var lots = await ApiClient.Instance.GetAsync<List<RawMaterialLot>>("/api/raw-material-lots");
                _allLots = lots ?? new List<RawMaterialLot>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void ApplyFilter()
        {
            var filtered = string.IsNullOrWhiteSpace(_search)
                ? _allLots
                : _allLots.Where(l => l.LotNumber.Contains(_search) || (l.Supplier?.Contains(_search) ?? false)).ToList();
            DgLots.ItemsSource = filtered;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            _search = TxtSearch.Text;
            ApplyFilter();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            _search = "";
            TxtSearch.Text = "";
            ApplyFilter();
        }

        private void DgLots_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DgLots.SelectedItem is RawMaterialLot lot)
            {
                var card = new Views.RawMaterialLotCard(lot.Id);
                card.ShowDialog();
                _ = LoadData();
            }
        }
    }
}