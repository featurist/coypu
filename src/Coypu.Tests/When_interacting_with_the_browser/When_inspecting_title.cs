using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_title : BrowserInteractionTests
    {
        [Test]
        public void It_returns_the_driver_page_title()
        {
            var pageTitle = "Coypu interaction tests page";
            driver.StubTitle(pageTitle, browserSession);
            Assert.That(browserSession.Title, Is.EqualTo(pageTitle));
        }
    }
}
