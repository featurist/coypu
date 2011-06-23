using System;
using Coypu.Robustness;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    [TestFixture]
    public class When_I_want_to_use_the_wrapper_directly
    {
        [Test]
        public void It_is_exposed_on_the_session()
        {
            var robustWrapper = new SpyRobustWrapper();
            var session = new Session(new StubDriver(), robustWrapper);

            Assert.That(session.RobustWrapper, Is.SameAs(robustWrapper));
        }
    }
}