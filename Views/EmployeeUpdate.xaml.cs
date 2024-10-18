using CassaSystem.Models;
using System;
using System.Windows;

namespace CassaSystem.Views
{
    public partial class EmployeeUpdate : Window
    {
        public event EventHandler<EmployeeModel> EmployeeUpdated;

        private EmployeeModel _employee;

        public EmployeeUpdate(EmployeeModel employee)
        {
            InitializeComponent();
            _employee = employee;

            txtName.Text = employee.Name;
            txtSurname.Text = employee.Surname;
            txtPosition.Text = employee.Position;
            txtEmail.Text = employee.Email;
            txtPhone.Text = employee.Phone;
            txtAddress.Text = employee.Address;
            txtSalary.Text = employee.Salary.ToString();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _employee.Name = txtName.Text;
            _employee.Surname = txtSurname.Text;
            _employee.Position = txtPosition.Text;
            _employee.Email = txtEmail.Text;
            _employee.Phone = txtPhone.Text;
            _employee.Address = txtAddress.Text;
            _employee.Salary = decimal.Parse(txtSalary.Text);

            EmployeeUpdated?.Invoke(this, _employee);
            this.Close();
        }
    }
}
