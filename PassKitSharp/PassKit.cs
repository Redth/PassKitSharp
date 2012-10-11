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

        public PKPass Pass { get; set; }

        public Dictionary<string, PKLocalization> Localizations { get; set; }


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
