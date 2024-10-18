using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using CassaSystem.Models;

namespace CassaSystem.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {


        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            
            string connectionString = "Server=WIN-J2HK6JVVRLR;Database= Product;Trusted_Connection=True";

            
            string query = "SELECT COUNT(1) FROM [User] WHERE Username=@Username AND Password=@Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);

                    
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                   
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 1)
                    {
                        Info info = new Info();
                        info.Show();

                        this.Close();
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
