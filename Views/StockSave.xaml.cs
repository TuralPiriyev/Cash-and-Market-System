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
    /// Interaction logic for StockSave.xaml
    /// </summary>
    public partial class StockSave : Window
    {
        public event EventHandler<Product> ProductAdded;
        public StockSave()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product
            {
                Name = txtName.Text,
                Barcode = txtBarcode.Text,
                Price = decimal.Parse(txtPrice.Text),
                Stock = int.Parse(txtStock.Text)
            };

            ProductAdded?.Invoke(this, newProduct);
            this.Close();
        }
    }
}
