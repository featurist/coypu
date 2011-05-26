using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_clicking_any_element : BrowserInteractionTests
    {
        [Test]
        public void It_makes_robust_call_to_underlying_driver()
        {
            var element = new StubElement();
            session.Click(element);

            Assert.That(driver.ClickedElements, Is.Empty);

            spyRobustWrapper.DeferredActions.Single()();

            Assert.That(driver.ClickedElements, Has.Member(element));
        }

    }
}