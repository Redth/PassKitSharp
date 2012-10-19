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
        public PKImage Strip { get; set; }
        public PKImage Thumbnail { get; set; }
        public PKImage Footer { get; set; }

        public PKManifest Manifest { get; set; }

        public Dictionary<string, PKLocalization> Localizations { get; set; }

        public string PassTypeIdentifier { get; set; }
        public int FormatVersion { get; set; }
        public string OrganizationName { get; set; }
        public string SerialNumber { get; set; }
        public string TeamIdentifier { get; set; }
        public string Description { get; set; }
        public string WebServiceURL { get; set; }
        public string AuthenticationToken { get; set; }
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

        public bool? SuppressStripShine { get; set; }

        
        public static PassKit Parse(string filename, bool loadHighResImages = true)
        {
            return PKParser.Parse(System.IO.File.OpenRead(filename), loadHighResImages);
        }

        public static PassKit Parse(System.IO.Stream stream, bool loadHighResImages = true)
        {
            return PKParser.Parse(stream, loadHighResImages);
        }

        public void Write(string passKitFilename, string certificateFilename)
        {
            PKWriter.Write(this, passKitFilename, certificateFilename);
        }

        public void Write(string passKitFilename, byte[] certificateData)
        {
            PKWriter.Write(this, passKitFilename, certificateData);
        }

        public void Write(string passKitFilename, System.Security.Cryptography.X509Certificates.X509Certificate2 certificate)
        {
            PKWriter.Write(this, passKitFilename, certificate);
        }
    }
}
