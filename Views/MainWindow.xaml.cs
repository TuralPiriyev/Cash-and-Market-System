using CassaSystem.Database;
using CassaSystem.Models;
using CassaSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CassaSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=WIN-J2HK6JVVRLR;Database=Product;Trusted_Connection=True";

        decimal totalamount = 0;
        private List<Product> scannedProductList = new List<Product>();
        private DatabaseHelper dbHelper = new DatabaseHelper();
        public ObservableCollection<Product> Products { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Products = new ObservableCollection<Product>();
            DataContext = this;
            ProductsList.ItemsSource = Products;
        }

        private void BarcodeTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string barcode = BarcodeTextBox.Text;

            Product product = dbHelper.GetProductByBarcode(barcode);

            if (product != null)
            {
                totalamount += product.Price;

                ResultTextBlock.Text += $" \n Ad: {product.Name}  Qiymet: {product.Price}";

                mebleg.Text = $" Umumi Mebleg: {totalamount}";

                scannedProductList.Add(product);
            }
            else
            {
                ResultTextBlock.Text = "Mehsul Tapilmadi!";
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            totalamount = 0;
            ResultTextBlock.Text = ""; 
            mebleg.Text = $"Umumi Mebleg: {totalamount}";
            scannedProductList.Clear();
        }


        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Product;

            if (product != null)
            {
                Products.Remove(product);
            }
        }

       


        private void Sel_Click(object sender, RoutedEventArgs e)
{
    if (scannedProductList.Count == 0)
    {
        MessageBox.Show("Heç bir məhsul skan edilməyib.");
        return;
    }

    foreach (var product in scannedProductList)
    {
        dbHelper.ReduceStock(product.Id, 1);
        
        dbHelper.IncreaseSale(product.Id, 1);

        var totalSale = new TotalSaleModel
        {
            Name = product.Name,
            Barcode = product.Barcode,
            Price = product.Price,
            QuantitySold = 1 ,
            SaleDate = DateTime.Now,
        };
        
        dbHelper.AddToTotalSale(totalSale); 

        
    }

            ResultOfSale result = new ResultOfSale();
            result.Show();

    totalamount = 0;
    ResultTextBlock.Text = $"Ad:    Qiymet: 00.00";
    mebleg.Text = $"Ümumi Mebleg: {totalamount}";

    scannedProductList.Clear();
}



        private List<Product> GetScannedProducts()
        {
            return scannedProductList;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Market market = new Market();
            market.Show();

            this.Close();
        }
    }

     
}
