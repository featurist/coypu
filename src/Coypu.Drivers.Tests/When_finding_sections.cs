using Coypu.Drivers.Watin;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver))]
    internal class When_finding_sections : DriverSpecs
    {
        internal override void Specs()
        {
            describe["finding sections by header text"] = () =>
            {
                it["finds by h1 text"] = () =>
                {
                    driver.FindSection("Section One h1").Id.should_be("sectionOne");
                    driver.FindSection("Section Two h1").Id.should_be("sectionTwo");
                };
                it["finds by h2 text"] = () =>
                {
                    driver.FindSection("Section One h2").Id.should_be("sectionOne");
                    driver.FindSection("Section Two h2").Id.should_be("sectionTwo");
                };
                it["finds by h3 text"] = () =>
                {
                    driver.FindSection("Section One h3").Id.should_be("sectionOne");
                    driver.FindSection("Section Two h3").Id.should_be("sectionTwo");
                };
                it["finds by h6 text"] = () =>
                {
                    driver.FindSection("Section One h6").Id.should_be("sectionOne");
                    driver.FindSection("Section Two h6").Id.should_be("sectionTwo");
                };
                it["finds by section by id"] = () =>
                {
                    driver.FindSection("sectionOne").Native.should_be(driver.FindSection("Section One h1").Native);
                    driver.FindSection("sectionTwo").Native.should_be(driver.FindSection("Section Two h1").Native);
                };

                it["finds section by id"] = () =>
                {
                    driver.FindSection("sectionOne").Native.should_be(driver.FindSection("Section One h1").Native);
                    driver.FindSection("sectionTwo").Native.should_be(driver.FindSection("Section Two h1").Native);
                };

               
            };
            
            it["only finds div and section"] = () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindSection("scope1TextInputFieldId"));
                Assert.Throws<MissingHtmlException>(() => driver.FindSection("fieldsetScope2"));
            };
        }
    }

}