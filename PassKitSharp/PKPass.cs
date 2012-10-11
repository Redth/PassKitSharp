using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PassKitSharp
{
    public class PKPass
    {
        public string PassTypeIdentifier { get; set; }
        public int FormatVersion { get; set; }
        public string OrganizationName { get; set; }
        public string SerialNumber { get; set; }
        public string TeamIdentifier { get; set; }
        public string Description { get; set; }
        public string ForegroundColor { get;set;}
        public string BackgroundColor { get;set; }
        public string LabelColor { get;set; }
        public string LogoText { get;set; }
        public DateTime? RelevantDate { get;set; }
        public PKLocationList Locations { get; set; }

        public List<string> AssociatedStoreIdentifiers { get; set; }

        public PKBarcode Barcode { get; set; }


        public PKPassSet Set { get; set; }
       
    }
}
