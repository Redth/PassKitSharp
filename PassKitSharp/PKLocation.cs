using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKitSharp
{
    public class PKLocationList : List<PKLocation>
    {
    }

    public class PKLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public string RelevantText { get; set; }
    }
}
