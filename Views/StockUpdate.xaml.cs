using CassaSystem.Models;
using System;
using System.Collections.Generic;
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

namespace CassaSystem.Views
{
    /// <summary>
    /// Interaction logic for StockUpdate.xaml
    /// </summary>
    public partial class StockUpdate : Window
    {
        public event EventHandler<Product> ProductUpdated;
        private Product _product;
        public StockUpdate(Product product)
        {
            InitializeComponent();
            _product = product;

            txtName.Text = product.Name;
            txtBarcode.Text = product.Barcode;
            txtPrice.Text = product.Price.ToString();
            txtStock.Text = product.Stock.ToString();

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
           _product.Name = txtName.Text;
           _product.Barcode = txtBarcode.Text;
           _product.Price = decimal.Parse(txtPrice.Text);
           _product.Stock = int.Parse(txtStock.Text);

            ProductUpdated?.Invoke(this, _product);
            this.Close();

        }
    }
}
