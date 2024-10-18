using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USB_Barcode_Scanner;

namespace CassaSystem
{
    public class BarcodeScanner
    {
       

        public string Barcode { get; set; }
        public Action<object, BarcodeScannerEventArgs> BarcodeScanned { get;  set; }

        public BarcodeScanner(string barcode)
        {
            Barcode = barcode;
        }

    }
}
