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
            string output = ARK.Model.XML.XMLParser.DlToMemFromFTP("ftp://ws14.surftown.dk", "/upload/monday/AktiveMedlemmer.xml", new System.Net.NetworkCredential("aauarat", "aautest"));
            XMLMembers.dataroot test = ARK.Model.XML.XMLParser.ParseXML<XMLMembers.dataroot>(output);
            Member memTest = new Member(test.activeMembers[0]);
            Console.WriteLine();
        }
    }
}
