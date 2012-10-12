using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PassKitSharp
{
    public class PKColor
    {
        public PKColor()
        {
            this.Red = 0;
            this.Green = 0;
            this.Blue = 0;
        }

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public override string ToString()
        {
            return string.Format("rgb({0},{1},{2})", Red, Green, Blue);
        }

        public static PKColor Parse(string rgbString)
        {
            var color = new PKColor();

            rgbString = rgbString.Trim().Replace(" ", "");

            rgbString = Regex.Replace(rgbString, "rgb\\s+?\\(", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            rgbString = rgbString.Replace(")", "");

            var match = Regex.Match(rgbString, "(?<red>[0-9]+),(?<green>[0-9]+),(?<blue>[0-9]+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (match != null)
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                if (match.Groups != null && match.Groups["red"] != null)
                    int.TryParse(match.Groups["red"].Value, out red);

                if (match.Groups != null && match.Groups["green"] != null)
                    int.TryParse(match.Groups["green"].Value, out green);

                if (match.Groups != null && match.Groups["blue"] != null)
                    int.TryParse(match.Groups["blue"].Value, out blue);

                color.Red = red;
                color.Green = green;
                color.Blue = blue;
            }

            return color;
        }
    }
}
