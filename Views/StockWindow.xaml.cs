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

namespace CassaSystem.Views
{
    /// <summary>
    /// Interaction logic for StockWindow.xaml
    /// </summary>
    public partial class StockWindow : Window, INotifyPropertyChanged
    {
        private string connectionString = "Server=WIN-J2HK6JVVRLR;Database=Product;Trusted_Connection=True";

        DatabaseHelper dbHelper = new DatabaseHelper();


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        

        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private int? selectedStockIndex;
        public int? SelectedStockIndex
        {
            get { return selectedStockIndex; }

            set
            {
                selectedStockIndex = value;
                OnPropertyChanged(nameof(SelectedStockIndex));
            }
        }
        public StockWindow()
        {
            InitializeComponent();
            Products = new ObservableCollection<Product>();
            DataContext = this;

            stockList.ItemsSource = Products;

            LoadProductsFromDatabase();
        }
        private void LoadProductsFromDatabase()
        {
            List<Product> products = dbHelper.GetProduct();
            Products.Clear();

            foreach (var e in products)
            {
                Products.Add(new Product
                {
                    Id = e.Id,
                    Name = e.Name,
                    Barcode = e.Barcode,
                    Price = e.Price,
                    Stock = e.Stock,
                });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            StockSave stockcontrol = new StockSave();
            stockcontrol.ProductAdded += stockControl_StockAdded;
            stockcontrol.Show();
        }

        private void stockControl_StockAdded(object sender, Product newProduct)
        {
            Products.Add(newProduct);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO prd (Name, Barcode, Price, Stock) VALUES (@Name, @Barcode, @Price, @Stock)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", newProduct.Name);
                cmd.Parameters.AddWithValue("@Barcode", newProduct.Barcode);
                cmd.Parameters.AddWithValue("@Price", newProduct.Price);
                cmd.Parameters.AddWithValue("@Stock", newProduct.Stock);
               
              
                cmd.ExecuteNonQuery();
            }

            LoadProductsFromDatabase();

        }

        private void UpdateButton_Click(Object sender, RoutedEventArgs e)
        {
            if(stockList.SelectedItem is Product selectedStock)
            {
                StockUpdate stockUpdate = new StockUpdate(selectedStock);
                stockUpdate.ProductUpdated += stockControl_StockUpdated;
                stockUpdate.Show();
            }

            else
            {
                MessageBox.Show("Zehmet olmasa , yenilemek ucun bir product secin.");
            }
        }

        private void stockControl_StockUpdated(object sender, Product newProduct)
        {
            var product = Products.FirstOrDefault(e => e.Id == newProduct.Id);
            if (product != null)
            {
                product.Name = newProduct.Name;
                product.Barcode = newProduct.Barcode;
                product.Price = newProduct.Price;
                product.Stock = newProduct.Stock;

                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE prd SET Name = @Name, Barcode = @Barcode, Price = @Price, Stock = @Stock WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", newProduct.Id); 
                    cmd.Parameters.AddWithValue("@Name", newProduct.Name);
                    cmd.Parameters.AddWithValue("@Barcode", newProduct.Barcode);
                    cmd.Parameters.AddWithValue("@Price", newProduct.Price);
                    cmd.Parameters.AddWithValue("@Stock", newProduct.Stock);

                    cmd.ExecuteNonQuery();
                }

                LoadProductsFromDatabase(); 
            }
        }

        private void BitmisMehsullar_Click(object sender, RoutedEventArgs e)
        {
            List<Product> outOfStockProducts = dbHelper.GetOutOfStockProducts();

            if (outOfStockProducts.Count > 0)
            {
                stockList.ItemsSource = outOfStockProducts; 
            }
            else
            {
                MessageBox.Show("Bitmiş məhsul yoxdur!");
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            string searchCriteria = ((ComboBoxItem)SearchCriteriaComboBox.SelectedItem).Content.ToString();

            List<Product> filteredProducts = new List<Product>();

            foreach (var product in dbHelper.GetProduct()) 
            {
                switch (searchCriteria)
                {
                    case "Ad":
                        if (product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        {
                            filteredProducts.Add(product);
                        }
                        break;
                    case "Qiymət":
                        if (product.Price.ToString().Contains(searchText))
                        {
                            filteredProducts.Add(product);
                        }
                        break;
                    case "Barkod":
                        if (product.Barcode.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        {
                            filteredProducts.Add(product);
                        }
                        break;
                    case "Məhsul Qalığı":
                        if (product.Stock.ToString().Contains(searchText))
                        {
                            filteredProducts.Add(product);
                        }
                        break;
                }
            }

            stockList.ItemsSource = filteredProducts; 
        }


        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //SettingsWindow settingsWindow = new SettingsWindow();
            //  settingsWindow.Show();

            MessageBox.Show("Bu panel hele ki dizayn olunmayib!");
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (stockList.SelectedItem is Product selectedProduct)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedProduct.Name}?",
                                                          "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM prd WHERE Id = @Id";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@Id", selectedProduct.Id);

                        cmd.ExecuteNonQuery();
                    }

                    Products.Remove(selectedProduct);
                    MessageBox.Show($"{selectedProduct.Name} deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.");
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Info info = new Info();
            info.Show();
            this.Close();
        }
    }
}
