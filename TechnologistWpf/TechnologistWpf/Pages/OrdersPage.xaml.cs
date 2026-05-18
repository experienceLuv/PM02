using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechnologistWpf.Models;
using TechnologistWpf.Services;

namespace TechnologistWpf.Pages
{
    public partial class OrdersPage : UserControl
    {
        public OrdersPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await LoadData();
        }

        private Task LoadData()
        {
            var orders = new List<ProductionOrder>
    {
        new ProductionOrder { Id = 1, OrderNumber = "PO-2401", RecipeId = 1, PlannedQuantityKg = 1000, StatusId = 7, PlannedStartDate = new DateTime(2025,3,1) },
        new ProductionOrder { Id = 2, OrderNumber = "PO-2402", RecipeId = 2, PlannedQuantityKg = 500, StatusId = 6, PlannedStartDate = new DateTime(2025,3,3) },
        new ProductionOrder { Id = 3, OrderNumber = "PO-2403", RecipeId = 4, PlannedQuantityKg = 2000, StatusId = 5, PlannedStartDate = new DateTime(2025,3,10) },
        new ProductionOrder { Id = 4, OrderNumber = "PO-2404", RecipeId = 1, PlannedQuantityKg = 800, StatusId = 6, PlannedStartDate = new DateTime(2025,3,4) },
        new ProductionOrder { Id = 5, OrderNumber = "PO-2405", RecipeId = 5, PlannedQuantityKg = 300, StatusId = 7, PlannedStartDate = new DateTime(2025,3,1) }
    };
            DgOrders.ItemsSource = orders;
            return Task.CompletedTask;
        }
    }
}