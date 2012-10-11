using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKitSharp
{
    public class PKBarcode
    {
        public PKBarcodeFormat Format { get; set; }
        public string Message { get; set; }
        public string MessageEncoding { get; set; }
        public string AltText { get; set; }
    }

    public enum PKBarcodeFormat
    {
        PKBarcodeFormatQR,
        PKBarcodeFormatPDF417,
        PKBarcodeFormatAztec,
        PKBarcodeFormatPassScan
    }
}
