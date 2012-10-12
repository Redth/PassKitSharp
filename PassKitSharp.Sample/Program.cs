using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PassKitSharp;

namespace PassKitSharp.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var pk =  PassKit.Parse(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "costco.pkpass"));

            Console.WriteLine(pk.Barcode.Message);

            var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testpass.p12"), "password");

            var outFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testoutput.pkpass");

            try { File.Delete(outFile); } catch { }

            pk.Write(outFile, cert);

            Console.WriteLine("OK");
        }
    }
}
