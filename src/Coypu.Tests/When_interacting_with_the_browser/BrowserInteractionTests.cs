using System;
using System.Collections.Generic;
using Coypu.Robustness;
using Coypu.Tests.TestBuilders;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public abstract class BrowserInteractionTests
    {
        protected FakeDriver driver;
        protected FakeWaiter fakeWaiter;
        protected Session session;
        protected SpyRobustWrapper spyRobustWrapper;
        protected StubUrlBuilder stubUrlBuilder;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            spyRobustWrapper = new SpyRobustWrapper();
            fakeWaiter = new FakeWaiter();
            stubUrlBuilder = new StubUrlBuilder();
            session = TestSessionBuilder.Build(driver, spyRobustWrapper, fakeWaiter, new SpyRestrictedResourceDownloader(),
                                               stubUrlBuilder);
        }
    }

    public class StubUrlBuilder : UrlBuilder
    {
        private readonly Dictionary<string, string> urls = new Dictionary<string, string>();

        #region UrlBuilder Members

        public string GetFullyQualifiedUrl(string virtualPath)
        {
            return urls[virtualPath];
        }

        #endregion

        public void SetStubUrl(string virtualPath, string url)
        {
            urls[virtualPath] = url;
        }
    }

    public class FakeWaiter : Waiter
    {
        private Action<TimeSpan> doOnWait = ms => { };

        #region Waiter Members

        public void Wait(TimeSpan duration)
        {
            doOnWait(duration);
        }

        #endregion

        public void DoOnWait(Action<TimeSpan> action)
        {
            doOnWait = action;
        }
    }
}