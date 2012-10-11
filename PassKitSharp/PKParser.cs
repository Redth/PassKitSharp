using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using Newtonsoft.Json.Linq;

namespace PassKitSharp
{
    public class PKParser
    {
        public static PassKit Parse(Stream pkStream)
        {
            var p = new PassKit();

            using (var zip = ZipFile.Read(pkStream))
            {
                foreach (var e in zip.Entries)
                {
                    if (e.IsDirectory)
                    {
                        continue;
                    }

                    if (e.FileName.Equals("manifest.json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Manifest == null)
                            p.Manifest = new PKManifest();

                        ParseManifest(p, e);
                    }
                    else if (e.FileName.Equals("pass.json", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Pass == null)
                            p.Pass = new PKPass();

                        ParsePassJson(p, e);
                    }
                    else if (e.FileName.Equals("background.png", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Background == null)
                            p.Background = new PKImage();

                        p.Background.Filename = e.FileName;
                        p.Background.Data = ReadStream(e);
                    }
                    else if (e.FileName.Equals("background@2x.png", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Background == null)
                            p.Background = new PKImage();

                        p.Background.HighResFilename = e.FileName;
                        p.Background.HighResData = ReadStream(e);
                    }
                    else if (e.FileName.Equals("logo.png", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Logo == null)
                            p.Logo = new PKImage();

                        p.Logo.Filename = e.FileName;
                        p.Logo.Data = ReadStream(e);
                    }
                    else if (e.FileName.Equals("logo@2x.png", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Logo == null)
                            p.Logo = new PKImage();

                        p.Logo.HighResFilename = e.FileName;
                        p.Logo.HighResData = ReadStream(e);
                    }
                    else if (e.FileName.Equals("icon.png", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Icon == null)
                            p.Icon = new PKImage();

                        p.Icon.Filename = e.FileName;
                        p.Icon.Data = ReadStream(e);
                    }
                    else if (e.FileName.Equals("icon@2x.png", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (p.Icon == null)
                            p.Icon = new PKImage();

                        p.Icon.HighResFilename = e.FileName;
                        p.Icon.HighResData = ReadStream(e);
                    }
                }
            }
                       
            return p;
        }

        static void ParsePassJson(PassKit p, ZipEntry e)
        {
            JObject json = null;

            try
            {
                using (var ms = new MemoryStream())
                {
                    e.Extract(ms);

                    ms.Position = 0;

                    using (var sr = new StreamReader(ms))
                        json = JObject.Parse(sr.ReadToEnd());
                }
            }
            catch { }

            if (p.Pass == null)
                p.Pass = new PKPass();

            if (json["passTypeIdentifier"] == null)
                throw new MissingFieldException("PassKit must include a passTypeIdentifier field!");

            if (json["formatVersion"] == null)
                throw new MissingFieldException("PassKit must include a formatVersion field!");

            if (json["organizationName"] == null)
                throw new MissingFieldException("PassKit must include a organizationName field!");

            if (json["serialNumber"] == null)
                throw new MissingFieldException("PassKit must include a serialNumber field!");

            if (json["teamIdentifier"] == null)
                throw new MissingFieldException("PassKit must include a teamIdentifier field!");
            
            if (json["description"] == null)
                throw new MissingFieldException("PassKit must include a description field!");

            p.Pass.PassTypeIdentifier = json["passTypeIdentifier"].Value<string>();
            p.Pass.FormatVersion = json["formatVersion"].Value<int>();
            p.Pass.OrganizationName = json["organizationName"].Value<string>();
            p.Pass.SerialNumber = json["serialNumber"].Value<string>();
            p.Pass.TeamIdentifier = json["teamIdentifier"].Value<string>();
            p.Pass.Description = json["description"].Value<string>();


            if (json["foregroundColor"] != null)
                p.Pass.ForegroundColor = json["foregroundColor"].Value<string>();

            if (json["backgroundColor"] != null)
                p.Pass.BackgroundColor = json["backgroundColor"].Value<string>();
    
            if (json["labelColor"] != null)
                p.Pass.LabelColor = json["labelColor"].Value<string>();

            if (json["logoText"] != null)
                p.Pass.LogoText = json["logoText"].Value<string>();

            if (json["relevantDate"] != null)
                p.Pass.RelevantDate = json["relevantDate"].Value<DateTime>();

            if (json["associatedStoreIdentifiers"] != null)
            {
                if (p.Pass.AssociatedStoreIdentifiers == null)
                    p.Pass.AssociatedStoreIdentifiers = new List<string>();

                var idarr = (JArray)json["associatedStoreIdentifiers"];
                foreach (var ida in idarr)
                    p.Pass.AssociatedStoreIdentifiers.Add(ida.ToString());
            }

            if (json["locations"] != null && json["locations"] is JArray)
            {
                foreach (JObject loc in (JArray)json["locations"])
                {
                    var pkl = new PKLocation();

                    if (loc["latitude"] == null)
                        throw new MissingFieldException("PassKit Location must include a latitude field!");

                    if (loc["longitude"] == null)
                        throw new MissingFieldException("PassKit Location must include a longitude field!");

                    pkl.Latitude = loc["latitude"].Value<double>();
                    pkl.Longitude = loc["longitude"].Value<double>();

                    if (loc["altitude"] != null)
                        pkl.Altitude = loc["altitude"].Value<double>();

                    if (loc["relevantText"] != null)
                        pkl.RelevantText = loc["relevantText"].Value<string>();

                    if (p.Pass.Locations == null)
                        p.Pass.Locations = new PKLocationList();

                    p.Pass.Locations.Add(pkl);
                }
            }

            if (json["barcode"] != null)
            {
                var bc = json["barcode"] as JObject;

                if (p.Pass.Barcode == null)
                    p.Pass.Barcode = new PKBarcode();

                if (bc["message"] == null)
                    throw new MissingFieldException("PassKit Barcode must include a message field!");
                
                if (bc["format"] == null)
                    throw new MissingFieldException("PassKit Barcode must include a format field!");

                if (bc["messageEncoding"] == null)
                    throw new MissingFieldException("PassKit Barcode must include a messageEncoding field!");

                if (bc["altText"] != null)
                    p.Pass.Barcode.AltText = bc["altText"].ToString();

                p.Pass.Barcode.Message = bc["message"].ToString();
                p.Pass.Barcode.MessageEncoding = bc["messageEncoding"].ToString();
                p.Pass.Barcode.Format = (PKBarcodeFormat)Enum.Parse(typeof(PKBarcodeFormat), bc["format"].ToString());
            }

            if (json["eventTicket"] != null)
                p.Pass.FieldSets = ParsePassSet(json["eventTicket"] as JObject);

            if (json["boardingPass"] != null)
                p.Pass.FieldSets = ParsePassSet(json["boardingPass"] as JObject);

            if (json["coupon"] != null)
                p.Pass.FieldSets = ParsePassSet(json["coupon"] as JObject);

            if (json["generic"] != null)
                p.Pass.FieldSets = ParsePassSet(json["generic"] as JObject);

            if (json["storeCard"] != null)
                p.Pass.FieldSets = ParsePassSet(json["storeCard"] as JObject);
        }

        static void ParseManifest(PassKit p, ZipEntry e)
        {
            JObject json = null;

            try
            {
                using (var ms = new MemoryStream())
                {
                    e.Extract(ms);

                    using (var sr = new StreamReader(ms))
                    {
                        ms.Position = 0;

                        var sj = sr.ReadToEnd();

                        json = JObject.Parse(sj);
                    }
                }
            }
            catch { }

            foreach (var prop in json.Properties())
            {
                var val = prop.Value.ToString();

                if (!p.Manifest.ContainsKey(prop.Name))
                    p.Manifest.Add(prop.Name, val);
                else
                    p.Manifest[prop.Name] = val;
            }
        }

        static PKPassSet ParsePassSet(JObject json)
        {
            var ps = new PKPassSet();

            if (json["primaryFields"] != null && json["primaryFields"] is JArray)
                ParsePassFieldSet(json["primaryFields"] as JArray, ps.PrimaryFields);

            if (json["secondaryFields"] != null && json["secondaryFields"] is JArray)
                ParsePassFieldSet(json["secondaryFields"] as JArray, ps.SecondaryFields);

            if (json["auxiliaryFields"] != null && json["auxiliaryFields"] is JArray)
                ParsePassFieldSet(json["auxiliaryFields"] as JArray, ps.AuxiliaryFields);

            if (json["backFields"] != null && json["backFields"] is JArray)
                ParsePassFieldSet((JArray)json["backFields"], ps.BackFields);

            return ps;
        }

        static void ParsePassFieldSet(JArray fieldItems, PKPassFieldSet set)
        {
            if (set == null)
                set = new PKPassFieldSet();

            foreach (JObject item in fieldItems)
            {
                if (item["value"] == null)
                    throw new MissingFieldException("Field must have a value field!");

                if (item["key"] == null)
                    throw new MissingFieldException("Field must have a key field!");

                PKPassField field = null;
                double tmp = 0;
                DateTime tmpDate = DateTime.UtcNow;

                if (DateTime.TryParse(item["value"].ToString(), out tmpDate) || item["dateStyle"] != null || item["timeStyle"] != null || item["isRelative"] != null)
                {
                    field = new PKPassDateField();

                    if (item["dateStyle"] != null)
                        ((PKPassDateField)field).DateStyle = (PKPassFieldDateStyle)Enum.Parse(typeof(PKPassFieldDateStyle), item["dateStyle"].ToString());

                    if (item["timeStyle"] != null)
                        ((PKPassDateField)field).TimeStyle = (PKPassFieldDateStyle)Enum.Parse(typeof(PKPassFieldDateStyle), item["timeStyle"].ToString());

                    if (item["isRelative"] != null)
                        ((PKPassDateField)field).IsRelative = item["isRelative"].Value<bool>();

                    ((PKPassDateField)field).Value = tmpDate;
                }
                else if (double.TryParse(item["value"].ToString(), out tmp))
                {
                    field = new PKPassNumberField();

                    if (item["numberStyle"] != null)
                        ((PKPassNumberField)field).NumberStyle = (PKPassFieldNumberStyle)Enum.Parse(typeof(PKPassFieldNumberStyle), item["numberStyle"].ToString());

                    ((PKPassNumberField)field).Value = tmp;
                }
                else
                {
                    field = new PKPassStringField();
                    ((PKPassStringField)field).Value = item["value"].ToString();
                }

                if (item["textAlignment"] != null)
                    field.TextAlignment = (PKPassFieldTextAlignment)Enum.Parse(typeof(PKPassFieldTextAlignment), item["textAlignment"].ToString());

                field.Key = item["key"].ToString();

                if (item["label"] != null)
                    field.Label = item["label"].ToString();

                set.Add(field);
            }
        }

        static byte[] ReadStream(ZipEntry e)
        {
            using (var input = new MemoryStream())
            {
                e.Extract(input);

                input.Position = 0;

                byte[] total_stream = new byte[0];

                byte[] stream_array = new byte[0];
                // Setup whatever read size you want (small here for testing)
                byte[] buffer = new byte[32];// * 1024];
                int read = 0;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream_array = new byte[total_stream.Length + read];
                    total_stream.CopyTo(stream_array, 0);
                    Array.Copy(buffer, 0, stream_array, total_stream.Length, read);
                    total_stream = stream_array;
                }

                return total_stream;
            }
        }
       
    }
}
