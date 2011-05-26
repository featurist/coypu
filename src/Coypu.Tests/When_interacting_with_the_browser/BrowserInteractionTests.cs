using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public abstract class BrowserInteractionTests
    {
        protected FakeDriver driver;
        protected SpyRobustWrapper spyRobustWrapper;
        protected Session session;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            spyRobustWrapper = new SpyRobustWrapper();
            session = new Session(driver, spyRobustWrapper);
        }


    }
}