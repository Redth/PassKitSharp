using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKitSharp
{
    public class PKBoardingPassSet : PKPassSet
    {

    }

    public class PKCouponPassSet : PKPassSet
    {

    }

    public class PKEventTicketPassSet : PKPassSet
    {

    }

    public class PKGenericPassSet : PKPassSet
    {
    }

    public class PKStoreCardPassSet : PKPassSet
    {

    }

    public class PKPassSet
    {
        public PKPassSet()
        {
            this.PrimaryFields = new PKPassFieldSet();
            this.SecondaryFields = new PKPassFieldSet();
            this.AuxiliaryFields = new PKPassFieldSet();
            this.BackFields = new PKPassFieldSet();
        }

        public PKPassFieldSet PrimaryFields { get; set; }
        public PKPassFieldSet SecondaryFields { get; set; }
        public PKPassFieldSet AuxiliaryFields { get; set; }
        public PKPassFieldSet BackFields { get; set; }
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
        PKTextAlignmentRight
    }

    public enum PKPassFieldNumberStyle
    {
        PKNumberStyleSpellOut
    }

    public enum PKPassFieldDateStyle
    {
        PKDateStyleMedium
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
