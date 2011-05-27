using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_sections_as_divs : DriverSpecs
    {
        internal override void Specs()
        {
            describe["finding sections by header text"] = () =>
            {
                it["finds by h1 text"] = () =>
                {
                    driver.FindSection("Div Section One h1").Id.should_be("divSectionOne");
                    driver.FindSection("Div Section Two h1").Id.should_be("divSectionTwo");
                };
                it["finds by h2 text"] = () =>
                {
                    driver.FindSection("Div Section One h2").Id.should_be("divSectionOne");
                    driver.FindSection("Div Section Two h2").Id.should_be("divSectionTwo");
                };
                it["finds by h3 text"] = () =>
                {
                    driver.FindSection("Div Section One h3").Id.should_be("divSectionOne");
                    driver.FindSection("Div Section Two h3").Id.should_be("divSectionTwo");
                };
                it["finds by h6 text"] = () =>
                {
                    driver.FindSection("Div Section One h6").Id.should_be("divSectionOne");
                    driver.FindSection("Div Section Two h6").Id.should_be("divSectionTwo");
                };

                it["finds by h2 text within child link"] = () =>
                {
                    driver.FindSection("Div Section One h2 with link").Id.should_be("divSectionOneWithLink");
                    driver.FindSection("Div Section Two h2 with link").Id.should_be("divSectionTwoWithLink");
                };
            };

            it["finds by div by id"] = () =>
            {
                driver.FindSection("divSectionOne").Native.should_be(driver.FindSection("Div Section One h1").Native);
                driver.FindSection("divSectionTwo").Native.should_be(driver.FindSection("Div Section Two h1").Native);
            };
        }
    }
}