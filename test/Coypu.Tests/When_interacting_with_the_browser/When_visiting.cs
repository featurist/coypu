using System.Linq;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_visiting : BrowserInteractionTests
    {
        [Fact]
        public void It_uses_a_fully_qualified_url_from_the_url_builder()
        {
            stubUrlBuilder.SetStubUrl("/some/resource", "http://blank.org");
            browserSession.Visit("/some/resource");
            Assert.Equal("http://blank.org", driver.Visits.Single().Request);
            Assert.Equal(browserSession, driver.Visits.Single().Scope);
        }

        [Fact]
        public void It_uses_the_current_scope()
        {
            stubUrlBuilder.SetStubUrl("/some/resource", "http://blank.org");
            popupScope.Visit("/some/resource");
            
            Assert.Equal("http://blank.org", driver.Visits.Single().Request);
            Assert.Equal(popupScope, driver.Visits.Single().Scope);
        }
    }
}