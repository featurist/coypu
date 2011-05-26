using System;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_finding_links : DriverSpecs
    {
        internal override void Specs()
        {
            it["finds link by text"] = () =>
            {
                driver.FindLink("first link").Id.should_be("firstLinkId");
                driver.FindLink("second link").Id.should_be("secondLinkId");
            };

            it["does not find display:none"] = () =>
             {
                 Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am an invisible link by display"));
             };

            it["does not find visibility:hidden links"] =    () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am an invisible link by visibility"));
            };
        }
    }
}