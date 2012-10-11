using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKitSharp
{
    public class PKLocalization
    {
        public string Locale { get; set; }
        public PKImage Background { get; set; }
        public PKImage Logo { get; set; }
        public PKImage Icon { get; set; }
        public PKStrings Strings { get; set; }
    }
}
