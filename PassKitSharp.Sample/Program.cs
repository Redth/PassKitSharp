using System;
using System.Collections.Generic;
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
            var pk =  PassKit.Parse(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.pkpass"));

            Console.WriteLine(pk.Pass.Barcode.Message);
        }
    }
}
