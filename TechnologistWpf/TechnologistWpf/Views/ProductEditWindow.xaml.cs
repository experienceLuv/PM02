using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Views
{
    public partial class ProductEditWindow : Window
    {
        private readonly Product _product;

        public ProductEditWindow(Product product)
        {
            InitializeComponent();
            _product = product ?? new Product();
            LoadProduct();
            LoadRelatedData();
        }

        private void LoadProduct()
        {
            TxtCode.Text = _product.Code;
            TxtName.Text = _product.Name;
            TxtType.Text = _product.Type;
            TxtForm.Text = _product.Form;
        }

        private async Task LoadRelatedData()
        {
            try
            {
                // Загружаем рецепты этого продукта
                var recipesResponse = await ApiClient.Instance.GetAsync<PaginatedResponse<RecipeVersion>>($"/api/recipes?productId={_product.Id}&pageSize=50");
                DgRecipes.ItemsSource = recipesResponse.Items;

                // Загружаем техкарты этого продукта
                var mapsResponse = await ApiClient.Instance.GetAsync<PaginatedResponse<TechnologicalMap>>($"/api/maps?productId={_product.Id}&pageSize=50");
                DgMaps.ItemsSource = mapsResponse.Items;
            }
            catch { /* если API не поддерживает фильтр, просто оставляем пустыми */ }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _product.Code = TxtCode.Text;
            _product.Name = TxtName.Text;
            _product.Type = TxtType.Text;
            _product.Form = TxtForm.Text;

            try
            {
                if (_product.Id == 0)
                {
                    var newProduct = await ApiClient.Instance.PostAsync<Product>("/api/products", _product);
                    _product.Id = newProduct.Id;
                }
                else
                {
                    await ApiClient.Instance.PutAsync($"/api/products/{_product.Id}", _product);
                }
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