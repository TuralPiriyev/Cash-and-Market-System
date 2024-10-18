using CassaSystem.Database;
using CassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;


namespace CassaSystem.Views
{
    /// <summary>
    /// Interaction logic for TotalSale.xaml
    /// </summary>
    public partial class TotalSale : Window, INotifyPropertyChanged
    {
        decimal totalSale = 0;
        public ObservableCollection<TotalSaleModel> FilteredSaleModel { get; set; }
        public ObservableCollection<TotalSaleModel> TotalSaleModel { get; set; } = new ObservableCollection<TotalSaleModel>();


        private string connectionString = "Server=WIN-J2HK6JVVRLR;Database=Product;Trusted_Connection=True";

        DatabaseHelper dbHelper = new DatabaseHelper();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }

        public ObservableCollection<TotalSaleModel> SoldProducts { get; set; }
        public ObservableCollection<TotalSaleModel> Products
        {
            get { return SoldProducts; }

            set
            {
                SoldProducts = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public TotalSale()
        {
            InitializeComponent();
            SoldProducts = new ObservableCollection<TotalSaleModel>();
            DataContext = this;

            soldProductList.ItemsSource = SoldProducts;

            LoadSoldProductFromDatabase();
        }

        private void LoadSoldProductFromDatabase()
        {
            List<TotalSaleModel> soldproducts = dbHelper.GetSoldProduct();
            SoldProducts.Clear(); 

            foreach (var e in soldproducts)
            {
                SoldProducts.Add(new TotalSaleModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Barcode = e.Barcode,
                    Price = e.Price,
                    QuantitySold = e.QuantitySold,
                    SaleDate = e.SaleDate,
                });
            }
        }


        private void FilterSales_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;
            string productName = ProductNameTextBox.Text.ToLower();
            string barcode = BarcodeTextBox.Text;

            var filteredSales = SoldProducts.ToList().Where(sale =>
            {
                bool dateFilter = (!startDate.HasValue || sale.SaleDate >= startDate) &&
                                  (!endDate.HasValue || sale.SaleDate <= endDate);
                bool nameFilter = string.IsNullOrEmpty(productName) || sale.Name.ToLower().Contains(productName);
                bool barcodeFilter = string.IsNullOrEmpty(barcode) || sale.Barcode.Contains(barcode);

                return dateFilter && nameFilter && barcodeFilter;
            }).ToList();

            FilteredSaleModel = new ObservableCollection<TotalSaleModel>(filteredSales);

            decimal totalSale = filteredSales.Sum(sale => sale.Price * sale.QuantitySold);
            int totalProductCount = filteredSales.Sum(sale => sale.QuantitySold);

            TotalSaleLabel.Content = $"Total: {totalSale} AZN";
            TotalProductCountLabel.Content = $"Total Products: {totalProductCount}";

            soldProductList.ItemsSource = FilteredSaleModel;
            if (string.IsNullOrWhiteSpace(productName) && string.IsNullOrWhiteSpace(barcode) && !startDate.HasValue && !endDate.HasValue)
            {
                soldProductList.ItemsSource = SoldProducts;
                TotalSaleLabel.Content = $"Total: {SoldProducts.Sum(p => p.Price * p.QuantitySold)} AZN";
                TotalProductCountLabel.Content = $"Total Products: {SoldProducts.Sum(p => p.QuantitySold)}";
                return; 
            }
        }




        private void CalculateTotalSale_Click(object sender, RoutedEventArgs e)
        {
            totalSale = SoldProducts.Sum(p => p.Price * p.QuantitySold); 

            TotalSaleLabel.Content = $"Total: {totalSale} AZN"; 
            
            
             
             
            
        }


        private void ShowTotalProductCount_Click(object sender, RoutedEventArgs e)
        {
            int totalProducts = SoldProducts.Sum(p => p.QuantitySold);

            TotalProductCountLabel.Content = $"Total Products: {totalProducts}";
        }

        private void ShowSoldProducts_Click(object sender, RoutedEventArgs e)
        {
            soldProductList.ItemsSource = SoldProducts;
        }
    }

}
