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
                driver.FindLink("first link", Root).Id.should_be("firstLinkId");
                driver.FindLink("second link", Root).Id.should_be("secondLinkId");
            };

            it["does not find display:none"] = () =>
             {
                 Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am an invisible link by display", Root));
             };

            it["does not find visibility:hidden links"] = () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am an invisible link by visibility", Root));
            };

            it["finds a link with both types of quote in its text"] = () =>
            {
                var link = driver.FindLink("I'm a link with \"both\" types of quote in my text", Root);
                Assert.That(link.Id, Is.EqualTo("linkWithBothQuotesId"));
            };
        }
    }
}