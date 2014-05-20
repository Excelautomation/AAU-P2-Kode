using System;
using ARK.HelperFunctions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ARK.Model;
using ARK.Model.XML;

namespace Test
{
    [TestClass]
    public class XMLParserTests
    {
        [TestMethod]
        public void GetSunsetFromXml_WithData_ReturnDateTime()
        {
            // Arrange
            DateTime expected = DateTime.Today;

            // Act
            DateTime actual = XmlParser.GetSunsetFromXml();

            // Assert - Tests that today's sunset time was found
            Assert.AreEqual(expected.Date, actual.Date);
        }
    }
}
