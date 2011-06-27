using System;
using System.Collections.Generic;
using Coypu.Robustness;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public abstract class BrowserInteractionTests
    {
        protected FakeDriver driver;
        protected SpyRobustWrapper spyRobustWrapper;
        protected Session session;
        protected MockSleepWaiter mockSleepWaiter;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            spyRobustWrapper = new SpyRobustWrapper();
            mockSleepWaiter = new MockSleepWaiter();
            session = new Session(driver, spyRobustWrapper, mockSleepWaiter);
        }
    }

    public class MockSleepWaiter : Waiter
    {
        private Action<TimeSpan> doOnWait = ms => { };

        public void Wait(TimeSpan duration)
        {
            doOnWait(duration);
        }

        public void DoOnWait(Action<TimeSpan> action)
        {
            doOnWait = action;
        }
    }
}