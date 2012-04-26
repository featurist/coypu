using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_title : BrowserInteractionTests
    {
        [Test]
        public void It_returns_the_driver_page_title()
        {
            var pageTitle = "Test page title";
            driver.StubTitle(pageTitle);
            Assert.That(driver.Title, Is.EqualTo(pageTitle));
        }
    }
}
