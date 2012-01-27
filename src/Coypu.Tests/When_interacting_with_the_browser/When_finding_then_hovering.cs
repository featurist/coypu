using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_then_hovering : BrowserInteractionTests
    {
        [Test]
        public void It_makes_robust_call_to_find_then_hover_element_on_underlying_driver()
        {
            var element = new StubElement();
            var elementScope = new ElementScope(new AlwaysFindsElementFinder(element), session.DriverScope);
            var finderCalled = false;
            driver.StubCss("something.to hover", element);

            elementScope.Hover();

            Assert.That(finderCalled, Is.False, "Finder not called robustly");
            Assert.That(driver.HoveredElements, Is.Empty, "Hover not called robustly");

            spyRobustWrapper.DeferredActions.Single()();

            Assert.That(finderCalled, Is.True);
            Assert.That(driver.HoveredElements, Has.Member(element));
        }
    }
}