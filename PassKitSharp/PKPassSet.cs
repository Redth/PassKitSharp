using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKitSharp
{

    public enum PKPassType
    {
        EventTicket,
        BoardingPass,
        Coupon,
        Generic,
        StoreCard
    }

    public class PKPassStringField : PKPassField
    {
        public string Value { get; set; }
    }

    public class PKPassDateField : PKPassField
    {
        public PKPassFieldDateStyle? DateStyle { get; set; }
        public PKPassFieldDateStyle? TimeStyle { get; set; }
        public bool? IsRelative { get; set; }
        public DateTime Value { get;set; }
    }

    public class PKPassNumberField : PKPassField
    {
        public PKPassFieldNumberStyle? NumberStyle { get; set; }
        public double Value { get;set; }
    }

    public abstract class PKPassField
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public PKPassFieldTextAlignment? TextAlignment { get; set; }
     }

    public enum PKPassFieldTextAlignment
    {
        PKTextAlignmentLeft,
        PKTextAlignmentCenter,
        PKTextAlignmentRight,
        PKTextAlignmentNatural
    }

    public enum PKPassFieldNumberStyle
    {
        PKNumberStyleSpellOut
    }

    public enum PKPassFieldDateStyle
    {
        PKDateStyleShort,
        PKDateStyleMedium,
        PKDateStyleLong
    }


    public class PKPassFieldSet : List<PKPassField>
    {
        //public void Add(string key, string label, string value)
        //{
        //    if (!base.Exists(pk => pk.Key.Equals(key)))
        //        base.Add(new PKPassField() { Key = key, Label = label, Value = value });
        //}
    }
}
