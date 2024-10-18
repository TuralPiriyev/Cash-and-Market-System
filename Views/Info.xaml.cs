using CassaSystem.Models;
using CassaSystem.Views;
using System.Windows;

namespace CassaSystem
{
    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        public Info()
        {
            InitializeComponent();
        }
        private void UmumiSatis_Click(object sender, RoutedEventArgs e)
        {
            Views.TotalSale totalSale = new Views.TotalSale();
            totalSale.Show();
        }
        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            StockWindow stockWindow = new StockWindow();
            stockWindow.Show();
        }
        private void GunlukSatis_Click(object sender, RoutedEventArgs e)
        {
            DailySale dailySale = new DailySale();
            dailySale.Show();
        }

        private void Isciler_Click(object sender, RoutedEventArgs e)
        {
            Views.Employee employee = new Views.Employee();
            employee.Show();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Market market = new Market();
            market.Show();

            this.Close();
        }
    }
}
