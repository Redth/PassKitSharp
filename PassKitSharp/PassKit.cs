using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ionic.Zip;

namespace PassKitSharp
{
    public class PassKit
    {
        public PKImage Background { get; set; }
        public PKImage Logo { get; set; }
        public PKImage Icon { get; set; }

        public PKManifest Manifest { get; set; }

        public Dictionary<string, PKLocalization> Localizations { get; set; }

        public string PassTypeIdentifier { get; set; }
        public int FormatVersion { get; set; }
        public string OrganizationName { get; set; }
        public string SerialNumber { get; set; }
        public string TeamIdentifier { get; set; }
        public string Description { get; set; }
        public PKColor ForegroundColor { get; set; }
        public PKColor BackgroundColor { get; set; }
        public PKColor LabelColor { get; set; }
        public string LogoText { get; set; }
        public DateTime? RelevantDate { get; set; }
        public PKLocationList Locations { get; set; }
        public List<string> AssociatedStoreIdentifiers { get; set; }
        public PKBarcode Barcode { get; set; }

        public PKPassType PassType { get; set; }

        public PKPassFieldSet PrimaryFields { get; set; }
        public PKPassFieldSet SecondaryFields { get; set; }
        public PKPassFieldSet AuxiliaryFields { get; set; }
        public PKPassFieldSet BackFields { get; set; }

        
        public static PassKit Parse(string filename)
        {
            return PKParser.Parse(System.IO.File.OpenRead(filename));
        }

        public static PassKit Parse(System.IO.Stream stream)
        {
            return PKParser.Parse(stream);
        }
    }
}
