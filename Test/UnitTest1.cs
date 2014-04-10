using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARK.Model;
using ARK.Model.XML;

namespace Test
{
    [TestClass]
    public class XMLParserTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string output = ARK.Model.XML.XMLParser.DlToMemFromFTP("ftp://ws14.surftown.dk", "/upload/monday/BådeSpecifik.xml", new System.Net.NetworkCredential("aauarat", "aautest"));
            XMLBoats.dataroot test = ARK.Model.XML.XMLParser.ParseXML<XMLBoats.dataroot>(output);
            Console.WriteLine();
        }
    }
}
