using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ionic.Zip;

namespace PassKitSharp
{
    public class PKWriter
    {
        public static void Write(PassKit passKit, string file, string certificateFilename)
        {
            Write(passKit, file, new X509Certificate2(certificateFilename));
        }

        public static void Write(PassKit passKit, string file, byte[] certificateData)
        {
            Write(passKit, file, new X509Certificate2(certificateData));
        }

        public static void Write(PassKit passKit, string file, X509Certificate2 certificate)
        {
            
            var manifestHashes = new Dictionary<string, string>();

            using (var zipFile = new ZipFile(file))
            {
                WritePass(passKit, zipFile, manifestHashes);

                if (passKit.Background != null)
                {
                    if (passKit.Background.Data != null && passKit.Background.Data.Length > 0)
                    {
                        zipFile.AddEntry("background.png", passKit.Background.Data);
                        manifestHashes.Add("background.png", CalculateSHA1(passKit.Background.Data));
                    }
                    if (passKit.Background.HighResData != null && passKit.Background.HighResData.Length > 0)
                    {
                        zipFile.AddEntry("background@2x.png", passKit.Background.HighResData);
                        manifestHashes.Add("background@2x.png", CalculateSHA1(passKit.Background.HighResData));
                    }
                }

                if (passKit.Logo != null)
                {
                    if (passKit.Logo.Data != null && passKit.Logo.Data.Length > 0)
                    {
                        zipFile.AddEntry("logo.png", passKit.Logo.Data);
                        manifestHashes.Add("logo.png", CalculateSHA1(passKit.Logo.Data));
                    }
                    if (passKit.Logo.HighResData != null && passKit.Logo.HighResData.Length > 0)
                    {
                        zipFile.AddEntry("logo@2x.png", passKit.Logo.HighResData);
                        manifestHashes.Add("logo@2x.png", CalculateSHA1(passKit.Logo.HighResData));
                    }
                }

                if (passKit.Icon != null)
                {
                    if (passKit.Icon.Data != null && passKit.Icon.Data.Length > 0)
                    {
                        zipFile.AddEntry("icon.png", passKit.Icon.Data);
                        manifestHashes.Add("icon.png", CalculateSHA1(passKit.Icon.Data));
                    }
                    if (passKit.Icon.HighResData != null && passKit.Icon.HighResData.Length > 0)
                    {
                        zipFile.AddEntry("icon@2x.png", passKit.Icon.HighResData);
                        manifestHashes.Add("icon@2x.png", CalculateSHA1(passKit.Icon.HighResData));
                    }
                }

                WriteManifest(passKit, zipFile, manifestHashes, certificate);

                zipFile.Save();
            }
        }

        static void WriteManifest(PassKit pk, ZipFile zipFile, Dictionary<string, string> manifestHashes, X509Certificate2 certificate)
        {
            var json = new JObject();

            foreach (var key in manifestHashes.Keys)
            {
                json[key] = manifestHashes[key];
            }

            var data = Encoding.UTF8.GetBytes(json.ToString());


            zipFile.AddEntry("manifest.json", data);

            SignManifest(zipFile, data, certificate);
        }

        

        static void WritePass(PassKit pk, ZipFile zipFile, Dictionary<string, string> manifestHashes)
        {
            var json = new JObject();

            json["passTypeIdentifier"] = pk.PassTypeIdentifier;
            json["formatVersion"] = pk.FormatVersion;
            json["organizationName"] = pk.OrganizationName;
            json["serialNumber"] = pk.SerialNumber;
            json["teamIdentifier"] = pk.TeamIdentifier;
            json["description"] = pk.Description;

            if (pk.ForegroundColor != null)
                json["foregroundColor"] = pk.ForegroundColor.ToString();

            if (pk.BackgroundColor != null)
                json["backgroundColor"] = pk.BackgroundColor.ToString();

            if (pk.LabelColor != null)
                json["labelColor"] = pk.LabelColor.ToString();

            if (!string.IsNullOrEmpty(pk.LogoText))
                json["logoText"] = pk.LogoText;

            if (pk.RelevantDate.HasValue)
                json["relevantDate"] = pk.RelevantDate.Value.ToString();

            if (pk.AssociatedStoreIdentifiers != null && pk.AssociatedStoreIdentifiers.Count > 0)
                json["associatedStoreIdentifiers"] = new JArray(pk.AssociatedStoreIdentifiers.ToArray());

            if (pk.Locations != null && pk.Locations.Count > 0)
            {
                var jsonLocations = new JArray();

                foreach (var l in pk.Locations)
                {
                    var jLoc = new JObject();

                    jLoc["latitude"] = l.Latitude;
                    jLoc["longitude"] = l.Longitude;

                    if (l.Altitude.HasValue)
                        jLoc["altitude"] = l.Altitude.Value;

                    if (!string.IsNullOrEmpty(l.RelevantText))
                        jLoc["relevantText"] = l.RelevantText;

                    jsonLocations.Add(jLoc);
                }

                json["locations"] = jsonLocations;
            }

            if (pk.Barcode != null)
            {
                var jsonBc = new JObject();

                jsonBc["message"] = pk.Barcode.Message;
                jsonBc["format"] = pk.Barcode.Format.ToString();
                jsonBc["messageEncoding"] = pk.Barcode.MessageEncoding;

                if (!string.IsNullOrEmpty(pk.Barcode.AltText))
                    jsonBc["altText"] = pk.Barcode.AltText;

                json["barcode"] = jsonBc;
            }

            var jSet = new JObject();

            if (pk.PrimaryFields != null && pk.PrimaryFields.Count > 0)
                jSet["primaryFields"] = WritePassSet(pk.PrimaryFields);

            if (pk.SecondaryFields != null && pk.SecondaryFields.Count > 0)
                jSet["secondaryFields"] = WritePassSet(pk.SecondaryFields);

            if (pk.AuxiliaryFields != null && pk.AuxiliaryFields.Count > 0)
                jSet["auxiliaryFields"] = WritePassSet(pk.AuxiliaryFields);

            if (pk.BackFields != null && pk.BackFields.Count > 0)
                jSet["backFields"] = WritePassSet(pk.BackFields);

            var setName = "eventTicket";
            switch (pk.PassType)
            {
                case PKPassType.BoardingPass:
                    setName = "boardingPass";
                    break;
                case PKPassType.Coupon:
                    setName = "coupon";
                    break;
                case PKPassType.EventTicket:
                    setName = "eventTicket";
                    break;
                case PKPassType.Generic:
                    setName = "generic";
                    break;
                case PKPassType.StoreCard:
                    setName = "storeCard";
                    break;
            }

            json[setName] = jSet;

            var data = System.Text.Encoding.UTF8.GetBytes(json.ToString());

            manifestHashes.Add("pass.json", CalculateSHA1(data, Encoding.UTF8));

            zipFile.AddEntry("pass.json", data);
        }

        static JArray WritePassSet(PKPassFieldSet pfs)
        {
            var json = new JArray();

            foreach (var f in pfs)
            {
                var j = new JObject();

                j["key"] = f.Key;
                j["label"] = f.Label;

                if (f is PKPassDateField)
                {
                    var fd = f as PKPassDateField;
                    j["value"] = fd.Value.ToString();

                    if (fd.DateStyle.HasValue)
                        j["dateStyle"] = fd.DateStyle.Value.ToString();
                    if (fd.TimeStyle.HasValue)
                        j["timeStyle"] = fd.TimeStyle.Value.ToString();
                    if (fd.IsRelative.HasValue)
                        j["isRelative"] = fd.IsRelative.Value;
                }
                else if (f is PKPassNumberField)
                {
                    var fd = f as PKPassNumberField;
                    j["value"] = fd.Value;

                    if (fd.NumberStyle.HasValue)
                        j["numberStyle"] = fd.NumberStyle.Value.ToString();
                }
                else if (f is PKPassStringField)
                {
                    var fd = f as PKPassStringField;
                    j["value"] = fd.Value;
                }

                if (f.TextAlignment.HasValue)
                    j["textAlignment"] = f.TextAlignment.ToString();

                json.Add(j);
            }

            return json;
        }

        static void SignManifest(ZipFile zipFile, byte[] manifestFileData, X509Certificate2 certificate)
        {
            var signer = new CmsSigner(SubjectIdentifierType.SubjectKeyIdentifier, certificate);
            signer.IncludeOption = X509IncludeOption.ExcludeRoot;

            ContentInfo cont = new ContentInfo(manifestFileData);

            var cms = new SignedCms(cont, true);
            cms.ComputeSignature(signer); // System.Security.Cryptography.CryptographicException
            
            var myCmsMessage = cms.Encode();

            zipFile.AddEntry("signature", myCmsMessage);
        }

        static string CalculateSHA1(byte[] buffer)
        {
            return CalculateSHA1(buffer, Encoding.UTF8);
        }

        static string CalculateSHA1(byte[] buffer, Encoding enc)
        {
            var cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "").ToLower();
        }
    }
}
