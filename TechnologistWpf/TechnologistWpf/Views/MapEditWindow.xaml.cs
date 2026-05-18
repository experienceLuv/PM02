using System;
using System.Windows;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Views
{
    public partial class MapEditWindow : Window
    {
        private readonly TechnologicalMap _map;
        public MapEditWindow(TechnologicalMap map = null)
        {
            InitializeComponent();
            _map = map ?? new TechnologicalMap();
            if (_map.Id != 0)
            {
                TxtName.Text = _map.Name;
                TxtProductId.Text = _map.ProductId.ToString();
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _map.Name = TxtName.Text;
            if (!int.TryParse(TxtProductId.Text, out int productId))
            {
                MessageBox.Show("Введите корректный ID продукта");
                return;
            }
            _map.ProductId = productId;
            try
            {
                if (_map.Id == 0)
                    await ApiClient.Instance.PostAsync("/api/maps", _map);
                else
                    await ApiClient.Instance.PutAsync($"/api/maps/{_map.Id}", _map);
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