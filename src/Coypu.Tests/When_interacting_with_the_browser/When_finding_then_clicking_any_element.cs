using System;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_finding_then_clicking_any_element : BrowserInteractionTests
    {
        [Test]
        public void It_makes_robust_call_to_find_then_clicks_element_on_underlying_driver()
        {
            var element = new StubElement();
            driver.StubCss("something.to click", element);
            spyRobustWrapper.AlwaysReturnFromRobustly(element);

            var elementScope = session.FindCss("something.to click");

            Assert.That(driver.FindCssRequests, Is.Empty, "Finder not called robustly");

            elementScope.Click();

            spyRobustWrapper.QueriesRan<ElementFound>().Single().Run();
            Assert.That(driver.FindCssRequests.Single(), Is.EqualTo("something.to click"));

            Assert.That(driver.ClickedElements, Has.Member(element));
        }
    }
}