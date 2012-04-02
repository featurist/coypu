using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Queries;
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
        protected BrowserSession browserSession;
        protected SpyRobustWrapper spyRobustWrapper;
        protected StubUrlBuilder stubUrlBuilder;
        protected SessionConfiguration SessionConfiguration;
        protected ElementScope elementScope;
        protected object queryResult;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            spyRobustWrapper = new SpyRobustWrapper();
            fakeWaiter = new FakeWaiter();
            stubUrlBuilder = new StubUrlBuilder();
            SessionConfiguration = new SessionConfiguration();
            browserSession = TestSessionBuilder.Build(SessionConfiguration, driver, spyRobustWrapper, fakeWaiter, new SpyRestrictedResourceDownloader(),
                                                      stubUrlBuilder);

            elementScope = browserSession.FindXPath(".");
        }

        protected object RunQueryAndCheckTiming()
        {
            return RunQueryAndCheckTiming<object>();
        }

        protected object RunQueryAndCheckTiming(TimeSpan timeout)
        {
            return RunQueryAndCheckTiming<object>(timeout);
        }

        protected T RunQueryAndCheckTiming<T>()
        {
            return RunQueryAndCheckTiming<T>(SessionConfiguration.Timeout);
        }

        protected T RunQueryAndCheckTiming<T>(TimeSpan timeout)
        {
            var query = spyRobustWrapper.QueriesRan<T>().Single();
            RunQueryAndCheckTiming(query, timeout);

            return query.Result;
        }

        protected T RunQueryAndCheckTiming<T>(TimeSpan timeout, int index)
        {
            var query = spyRobustWrapper.QueriesRan<T>().ElementAt(index);
            RunQueryAndCheckTiming(query, timeout);

            return query.Result;
        }

        protected T RunQueryAndCheckTiming<T>(Query<T> query)
        {
            return RunQueryAndCheckTiming(query, SessionConfiguration.Timeout);
        }

        protected T RunQueryAndCheckTiming<T>(Query<T> query, TimeSpan timeout)
        {
            query.Run();

            queryResult = query.Result;

            Assert.That(query.Timeout, Is.EqualTo(timeout));
            Assert.That(query.RetryInterval, Is.EqualTo(SessionConfiguration.RetryInterval));

            return query.Result;
        }
    }

    public class StubDriverFactory : DriverFactory
    {
        private readonly Driver driver;

        public StubDriverFactory(Driver driver)
        {
            this.driver = driver;
        }

        public Driver NewWebDriver(Type driverType, Drivers.Browser browser)
        {
            return driver;
        }
    }

    public class StubUrlBuilder : UrlBuilder
    {
        private readonly Dictionary<string, string> urls = new Dictionary<string, string>();

        public string GetFullyQualifiedUrl(string virtualPath, SessionConfiguration SessionConfiguration)
        {
            return urls[virtualPath];
        }

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