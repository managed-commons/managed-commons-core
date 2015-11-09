using System;
using System.Linq;
using NUnit.Framework;

namespace Commons.Tests
{
    [TestFixture]
    public class LicenseAttributeTests
    {
        [Test]
        public void LicenseAttribute_New_MultipleParameters()
        {
            var attr = new LicenseAttribute("name", "http://nowhere", "details", true);
            Assert.AreEqual(LicenseType.Proprietary, attr.LicenseType);
            Assert.AreEqual(true, attr.IsProprietary);
            Assert.AreEqual("name", attr.Name);
            var text = attr.ToString();
            Assert.That(text, Is.StringContaining("name"));
            Assert.That(text, Is.StringContaining("details"));
            Assert.That(text, Is.StringContaining("nowhere"));
        }

        [Test]
        public void LicenseAttribute_New_SingleParameter()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LicenseAttribute(LicenseType.Other));
            var attr = new LicenseAttribute(LicenseType.MIT);
            Assert.AreEqual(LicenseType.MIT, attr.LicenseType);
            Assert.AreEqual(false, attr.IsProprietary);
            Assert.AreEqual("MIT License", attr.Name);
        }

        [Test]
        public void LicenseAttribute_ToString()
        {
            var attr = new LicenseAttribute(LicenseType.MIT);
            Assert.AreEqual("MIT License - See https://opensource.org/licenses/MIT", attr.ToString());
        }
    }
}
