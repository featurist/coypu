using Coypu.Timing;
using Coypu.Tests.When_interacting_with_the_browser;
using Coypu.WebRequests;

namespace Coypu.Tests.TestBuilders
{
    internal class TestSessionBuilder
    {
        internal static BrowserSession Build(SessionConfiguration sessionConfiguration, 
                                                Driver driver, 
                                                TimingStrategy timingStrategy, 
                                                Waiter waiter,
                                                RestrictedResourceDownloader restrictedResourceDownloader, 
                                                UrlBuilder urlBuilder, 
                                                DisambiguationStrategy disambiguationStrategy = null)
        {
            disambiguationStrategy = disambiguationStrategy ?? new FirstOrDefaultNoDisambiguationStrategy();
            
            return new BrowserSession(sessionConfiguration, 
                            new StubDriverFactory(driver), 
                            timingStrategy, 
                            waiter, 
                            urlBuilder, 
                            disambiguationStrategy,
                            restrictedResourceDownloader);
        }
    }
}