using CassaSystem.Database;
using CassaSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CassaSystem.Views
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Window, INotifyPropertyChanged
    {
        private string connectionString = "Server=WIN-J2HK6JVVRLR;Database=Product;Trusted_Connection=True";

        DatabaseHelper dbHelper = new DatabaseHelper();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<EmployeeModel> _employees;
        public ObservableCollection<EmployeeModel> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees)); // Notify change
            }
        }

        private int? _selectedEmployeeIndex;
        public int? SelectedEmployeeIndex
        {
            get { return _selectedEmployeeIndex; }
            set
            {
                _selectedEmployeeIndex = value;
                OnPropertyChanged(nameof(SelectedEmployeeIndex));
            }
        }

        public Employee()
        {
            InitializeComponent();
            Employees = new ObservableCollection<EmployeeModel>();
            DataContext = this;
            employeeList.ItemsSource = Employees;

            LoadEmployeesFromDatabase();
        }

        private void LoadEmployeesFromDatabase()
        {
            List<EmployeeModel> employees = dbHelper.Get();
            Employees.Clear(); // Clear previous employees

            foreach (var e in employees)
            {
                Employees.Add(new EmployeeModel
                {
                    Id = e.Id, // Add Id value
                    Name = e.Name,
                    Surname = e.Surname,
                    Position = e.Position,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Salary = e.Salary,
                });
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeSave employeeControl = new EmployeeSave();
            employeeControl.EmployeeAdded += employeeControl_EmployeeAdded;
            employeeControl.Show();
        }

        private void employeeControl_EmployeeAdded(object sender, EmployeeModel newEmployee)
        {
            Employees.Add(newEmployee);

            // Add to database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Employee (Name, Surname, Position, Email, Phone, Address, Salary) VALUES (@Name, @Surname, @Position, @Email, @Phone, @Address, @Salary)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", newEmployee.Name);
                cmd.Parameters.AddWithValue("@Surname", newEmployee.Surname);
                cmd.Parameters.AddWithValue("@Position", newEmployee.Position);
                cmd.Parameters.AddWithValue("@Email", newEmployee.Email);
                cmd.Parameters.AddWithValue("@Phone", newEmployee.Phone);
                cmd.Parameters.AddWithValue("@Address", newEmployee.Address);
                cmd.Parameters.AddWithValue("@Salary", newEmployee.Salary);
                cmd.ExecuteNonQuery();
            }

            LoadEmployeesFromDatabase(); 
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeList.SelectedItem is EmployeeModel selectedEmployee)
            {
                EmployeeUpdate employeeUpdate = new EmployeeUpdate(selectedEmployee);
                employeeUpdate.EmployeeUpdated += employeeControl_EmployeeUpdated; 
                employeeUpdate.Show();
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa, yeniləmək üçün bir işçi seçin.");
            }
        }


        private void employeeControl_EmployeeUpdated(object sender, EmployeeModel updatedEmployee)
        {
            // Find and update the employee in the collection
            var employee = Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);
            if (employee != null)
            {
                employee.Name = updatedEmployee.Name;
                employee.Surname = updatedEmployee.Surname;
                employee.Position = updatedEmployee.Position;
                employee.Email = updatedEmployee.Email;
                employee.Phone = updatedEmployee.Phone;
                employee.Address = updatedEmployee.Address;
                employee.Salary = updatedEmployee.Salary;

                // Update in the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Employee SET Name = @Name, Surname = @Surname, Position = @Position, Email = @Email, Phone = @Phone, Address = @Address, Salary = @Salary WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Id", employee.Id);
                    cmd.Parameters.AddWithValue("@Name", updatedEmployee.Name);
                    cmd.Parameters.AddWithValue("@Surname", updatedEmployee.Surname);
                    cmd.Parameters.AddWithValue("@Position", updatedEmployee.Position);
                    cmd.Parameters.AddWithValue("@Email", updatedEmployee.Email);
                    cmd.Parameters.AddWithValue("@Phone", updatedEmployee.Phone);
                    cmd.Parameters.AddWithValue("@Address", updatedEmployee.Address);
                    cmd.Parameters.AddWithValue("@Salary", updatedEmployee.Salary);
                    cmd.ExecuteNonQuery();
                }

                LoadEmployeesFromDatabase(); // Reload
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (employeeList.SelectedItem is EmployeeModel selectedEmployee)
            {
                dbHelper.DeleteEmployee(selectedEmployee.Id);
                MessageBox.Show($"{selectedEmployee.Name} employeer deleted successfully!");

                // Reload the data
                LoadEmployeesFromDatabase();
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.");
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
