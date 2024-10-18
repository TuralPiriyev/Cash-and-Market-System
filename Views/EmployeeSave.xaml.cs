using CassaSystem.Models;
using System;
using System.Windows;

namespace CassaSystem.Views
{
    public partial class EmployeeSave : Window
    {
        public event EventHandler<EmployeeModel> EmployeeAdded;

        public EmployeeSave()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // İstifadəçi girişindən yeni EmployeeModel nümunəsi yaradın
            EmployeeModel newEmployee = new EmployeeModel
            {
                Name = txtName.Text,
                Surname = txtSurname.Text,
                Position = txtPosition.Text,
                Email = txtEmail.Text,
                Phone = txtPhone.Text,
                Address = txtAddress.Text,
                Salary = decimal.Parse(txtSalary.Text)
            };

            // EmployeeAdded hadisəsini tetikleyin
            EmployeeAdded?.Invoke(this, newEmployee);
            this.Close();
        }
    }
}
