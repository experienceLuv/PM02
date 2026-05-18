using System;
using System.Windows;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Views
{
    public partial class OrderEditWindow : Window
    {
        private readonly ProductionOrder _order;
        public OrderEditWindow(ProductionOrder order = null)
        {
            InitializeComponent();
            _order = order ?? new ProductionOrder();
            if (_order.Id != 0)
            {
                TxtOrderNumber.Text = _order.OrderNumber;
                TxtRecipeId.Text = _order.RecipeId.ToString();
                TxtQuantity.Text = _order.PlannedQuantityKg.ToString();
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _order.OrderNumber = TxtOrderNumber.Text;
            if (!int.TryParse(TxtRecipeId.Text, out int recipeId) || !decimal.TryParse(TxtQuantity.Text, out decimal qty))
            {
                MessageBox.Show("Проверьте введённые данные");
                return;
            }
            _order.RecipeId = recipeId;
            _order.PlannedQuantityKg = qty;
            try
            {
                if (_order.Id == 0)
                    await ApiClient.Instance.PostAsync("/api/orders", _order);
                else
                    await ApiClient.Instance.PutAsync($"/api/orders/{_order.Id}", _order);
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