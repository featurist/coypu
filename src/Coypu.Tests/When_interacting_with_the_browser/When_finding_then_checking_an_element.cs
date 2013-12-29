using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser {
    [TestFixture]
    public class When_finding_then_checking_an_element : BrowserInteractionTests
    {
        [Test]
        public void It_finds_then_synchronises_check_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something_to_click", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromRobustly(element);

            var toCheck = browserSession.FindCss("something_to_click");

            toCheck.Check();

            Assert.That(driver.CheckedElements, Has.No.Member(toCheck), "Check was not synchronised");

            RunQueryAndCheckTiming();

            Assert.That(driver.CheckedElements, Has.Member(toCheck));
        }
    }
}