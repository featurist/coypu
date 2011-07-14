using System;
using System.Collections.Generic;
using Coypu.Robustness;
using Coypu.Tests.TestDoubles;
using Coypu.Tests.When_making_browser_interactions_robust;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public abstract class BrowserInteractionTests
    {
        protected FakeDriver driver;
        protected SpyRobustWrapper spyRobustWrapper;
        protected Session session;
        protected FakeWaiter FakeWaiter;

        [SetUp]
        public void SetUp()
        {
            driver = new FakeDriver();
            spyRobustWrapper = new SpyRobustWrapper();
            FakeWaiter = new FakeWaiter();
            session = TestSessionBuilder.Build(driver, spyRobustWrapper, FakeWaiter);
        }
    }

    public class FakeWaiter : Waiter
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