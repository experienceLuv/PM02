using System;
using System.Windows;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Views
{
    public partial class RecipeEditWindow : Window
    {
        private readonly RecipeVersion _recipe;
        public RecipeEditWindow(RecipeVersion recipe = null)
        {
            InitializeComponent();
            _recipe = recipe ?? new RecipeVersion();
            if (_recipe.Id != 0)
            {
                TxtProductId.Text = _recipe.ProductId.ToString();
                TxtVersion.Text = _recipe.Version.ToString();
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtProductId.Text, out int productId) || !int.TryParse(TxtVersion.Text, out int version))
            {
                MessageBox.Show("Введите корректные числа");
                return;
            }
            _recipe.ProductId = productId;
            _recipe.Version = version;
            try
            {
                if (_recipe.Id == 0)
                {
                    // создание новой рецептуры: нужен POST с компонентами, пока отправляем без компонентов
                    await ApiClient.Instance.PostAsync<RecipeVersion>("/api/recipes", _recipe);
                }
                else
                {
                    await ApiClient.Instance.PutAsync($"/api/recipes/{_recipe.Id}", _recipe);
                }
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}