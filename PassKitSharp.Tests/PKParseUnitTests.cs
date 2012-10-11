using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PassKitSharp.Tests
{
    [TestClass]
    public class PKParseUnitTests
    {
        [TestMethod]
        public void ParseSamplePasses()
        {

            var passes = System.IO.Directory.EnumerateFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SamplePasses"), "*.pkpass");

            foreach (var file in passes)
            {
                PassKit.Parse(file); 
            }

        }
    }
}
