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
using System.Windows.Threading;

namespace CassaSystem.Views
{
    /// <summary>
    /// Interaction logic for ResultOfSale.xaml
    /// </summary>
    public partial class ResultOfSale : Window
    {
        private DispatcherTimer _timer;
        private int _countdownTime = 1;
        public ResultOfSale()
        {
            InitializeComponent();
            StartTimer();
        }

        private void StartTimer()
        {
            CountdownTextBlock.Text = $"Baglamaga {_countdownTime} saniye qaldi.";
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _countdownTime--;
            CountdownTextBlock.Text = $"Baglanmaga {_countdownTime} saniye qaldi.";

            if(_countdownTime <= 0)
            {
                _timer.Stop();
                this.Close();
            }
           
        }
    }
}
