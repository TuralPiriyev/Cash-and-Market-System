using CassaSystem.Models;
using CassaSystem.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CassaSystem.Database
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=WIN-J2HK6JVVRLR;Database= Product;Trusted_Connection=True";

        // SqlProductRepository/////////////////////////////////////////
        public Product GetProductByBarcode(string barcode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM prd WHERE Barcode = @Barcode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Barcode", barcode);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Product
                    {
                        // Burda Mappers-dan da istifade ede bilerik
                        Id = (int)reader["Id"],
                        Barcode = (string)reader["Barcode"],
                        Name = (string)reader["Name"],
                        Price = (decimal)reader["Price"]
                    };
                }
            }
            return null;
        }

        public void Add(Product product)
        {
            using SqlConnection connection = new SqlConnection (connectionString);
            connection.Open();

            string query = "Insert into prd values (@Barcode, @Name, @Price, @Stock)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Stock", product.Stock);

            cmd.ExecuteNonQuery();
        }
        public void UpdateProduct(Product product)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "Insert into prd values (@Barcode, @Name, @Price, @Stock)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Barcode", product.Barcode);
           
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Stock", product.Stock);

            cmd.ExecuteNonQuery();
        }
        public List<Product> GetProduct()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            const string query = "SELECT * FROM prd";
            SqlCommand cmd = new SqlCommand(query, connection);

            SqlDataReader reader = cmd.ExecuteReader();
            List<Product> result = new List<Product>();


            while (reader.Read())
            {
                result.Add(new Product
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Barcode = (string)reader["Barcode"],
                    Price = (decimal)reader["Price"],
                    Stock = (int)reader["Stock"]
                });
            }

            return result;
        }
        public List<TotalSaleModel> GetSoldProduct()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            const string query = "SELECT * FROM TotalSale where QuantitySold>=1";
            SqlCommand cmd = new SqlCommand(query, connection);

            SqlDataReader reader = cmd.ExecuteReader();
            List<TotalSaleModel> result = new List<TotalSaleModel>();


            while (reader.Read())
            {
                result.Add(new TotalSaleModel
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Barcode = (string)reader["Barcode"],
                    Price = (decimal)reader["Price"],
                    QuantitySold = (int)reader["QuantitySold"],
                    SaleDate = (DateTime)reader["SaleDate"]

                });
            }

            return result;
        }

      public void AddToTotalSale(TotalSaleModel totalSaleModel)
      {
          using SqlConnection connection = new SqlConnection(connectionString);
          connection.Open();

          string query = "Insert into TotalSale (Barcode, Name, Price, QuantitySold, SaleDate) values (@Barcode, @Name, @Price, @QuantitySold, @SaleDate)";
          SqlCommand cmd = new SqlCommand(query, connection);
          cmd.Parameters.AddWithValue("@Barcode", totalSaleModel.Barcode);
          cmd.Parameters.AddWithValue("@Name", totalSaleModel.Name);
          cmd.Parameters.AddWithValue("@Price", totalSaleModel.Price);
          cmd.Parameters.AddWithValue("@QuantitySold", totalSaleModel.QuantitySold);
          cmd.Parameters.AddWithValue("@SaleDate", totalSaleModel.SaleDate);

          cmd.ExecuteNonQuery();
      }

        public void DeleteProduct(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            const string query = "delete from prd where id = @id ";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }
        public void ReduceStock(int productId, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE prd SET Stock = Stock - @Quantity WHERE Id = @Id AND Stock > 0";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    MessageBox.Show("Stokda kifayət qədər məhsul yoxdur və ya məhsul mövcud deyil.");
                }
            }
        }
        public void IncreaseSale(int productId, int quantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Update TotalSale set QuantitySold = QuantitySold + @Quantity where id = @Id"; // @Quantity istifadə edin
                SqlCommand cmd = new SqlCommand(query, connection);

                // Parametrləri əlavə edin
                cmd.Parameters.AddWithValue("@Id", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity); // Burada @Quantity olmalıdır

                int rowsAffected = cmd.ExecuteNonQuery();
              
            }
        }





        public List<Product> GetOutOfStockProducts()
        {
            List<Product> outOfStockProducts = new List<Product>();
            string query = "SELECT * FROM prd WHERE Stock <= 0"; // SQL sorğusu

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Barcode = (string)reader["Barcode"],
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"]
                    };
                    outOfStockProducts.Add(product);
                }
            }

            return outOfStockProducts;
        }





        //SqlEmployeRepository////////////////////////////////
        public void AddEmployee(EmployeeModel employee)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "Insert into Employee values(@Name, @Surname, @Email, @Phone, @Address, @Salary, @Position)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Surname", employee.Surname);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Phone", employee.Phone);
            cmd.Parameters.AddWithValue("@Address", employee.Address);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@Position", employee.Position);

            cmd.ExecuteNonQuery ();

        }
        public void UpdateEmployee(EmployeeModel employee)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "Update  Employee set Name=@Name, Surname =@Surname, Email =@Email, Phone =@Phone, Address =@Address, Salary =@Salary, Position = @Position)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Surname", employee.Surname);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Phone", employee.Phone);
            cmd.Parameters.AddWithValue("@Address", employee.Address);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@Position", employee.Position);

            cmd.ExecuteNonQuery();

        }
        public void DeleteEmployee(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            const string query = "delete from Employee where id = @id ";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }

        public List<EmployeeModel> Get()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            const string query = "SELECT * FROM Employee";
            SqlCommand cmd = new SqlCommand(query, connection);

            SqlDataReader reader = cmd.ExecuteReader();
            List<EmployeeModel> result = new List<EmployeeModel>();

            
            while (reader.Read())
            {
                result.Add(new EmployeeModel
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Surname = (string)reader["Surname"],
                    Position = (string)reader["Position"],
                    Email = (string)reader["Email"],
                    Phone = (string)reader["Phone"],
                    Address = (string)reader["Address"],
                    Salary = (decimal)reader["Salary"]
                });
            }

            return result; 
        }

       
    }
}
