using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser {
    [TestFixture]
    public class When_finding_then_clicking_an_element_ : BrowserInteractionTests
    {
        [Test]
        public void It_finds_then_synchronises_click_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something_to_click", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromRobustly(element);

            var elementScope = browserSession.FindId("something_to_click");
            elementScope.Click();

            Assert.That(driver.ClickedElements, Has.No.Member(elementScope), "Click was not synchronised");

            RunQueryAndCheckTiming();

            Assert.That(driver.ClickedElements, Has.Member(elementScope));
        }
    }
}