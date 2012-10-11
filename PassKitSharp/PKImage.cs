using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKitSharp
{
    public class PKImage
    {
        public string Filename { get; set; }
        public string HighResFilename { get; set; }

        public byte[] Data { get; set; }
        public byte[] HighResData { get; set; }
    }
}
