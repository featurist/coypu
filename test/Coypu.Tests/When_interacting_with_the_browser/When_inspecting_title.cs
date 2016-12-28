using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting_title : BrowserInteractionTests
    {
        [Fact]
        public void It_returns_the_driver_page_title()
        {
            var pageTitle = "Coypu interaction tests page";
            driver.StubTitle(pageTitle, browserSession);
            Assert.Equal(pageTitle, browserSession.Title);
        }
    }
}
