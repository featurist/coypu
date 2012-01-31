using System;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_querying_with_any_finder : When_inspecting
    {
        [Test]
        public void Has_queries_robustly_with_zero_timeout() 
        {
            spyRobustWrapper.StubQueryResult(true, true);

            Configuration.Timeout = TimeSpan.FromSeconds(21);

            var queryTimeout = TimeSpan.MaxValue;
            session.Has(session.FindLink("Sign out"));
            ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();

            Assert.That(queryTimeout, Is.EqualTo(TimeSpan.Zero));
            Assert.That(Configuration.Timeout, Is.EqualTo(TimeSpan.FromSeconds(21)));
        }

        [Test]
        public void Has_resets_timeout_on_error() 
        {
            spyRobustWrapper.StubQueryResult(true, true);

            Configuration.Timeout = TimeSpan.FromSeconds(10);
            session.Has(new ElementScope(new AlwaysExceptionsErrorFinder(), session.DriverScope, spyRobustWrapper));
            try 
            {
                ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();
                Assert.Fail("Expected an ExplicitlyThrownTestException");
            } 
            catch (ExplicitlyThrownTestException) { }

            Assert.That(Configuration.Timeout, Is.EqualTo(TimeSpan.FromSeconds(10)));
        }

        [Test]
        public void Has_wraps_query_to_return_true_if_found() 
        {
            spyRobustWrapper.StubQueryResult(true, true);

            session.Has(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper));

            var deferredResult = ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();
            Assert.That(deferredResult, Is.True);
        }

        [Test]
        public void Has_wraps_query_to_return_false_if_not_found() 
        {
            spyRobustWrapper.StubQueryResult(true, true);

            session.Has(new ElementScope(new AlwaysMissingElementFinder(), session.DriverScope, spyRobustWrapper));

            var deferredResult = ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();
            Assert.That(deferredResult, Is.False);
        }

        [Test]
        public void Has_queries_robustly_expecting_element_found_Positive_case() 
        {
            spyRobustWrapper.StubQueryResult(true, true);

            Assert.That(session.Has(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper)), Is.True);
        }

        [Test]
        public void Has_queries_robustly_expecting_element_found_Negitive_case()
        {
            spyRobustWrapper.StubQueryResult(true, false);

            Assert.That(session.Has(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper)), Is.False);
        }

        [Test]
        public void HasNo_queries_robustly_with_zero_timeout() 
        {
            spyRobustWrapper.StubQueryResult(false, true);

            Configuration.Timeout = TimeSpan.FromSeconds(10);
            var queryTimeout = TimeSpan.MaxValue;
            session.HasNo(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper));
            ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();

            Assert.That(queryTimeout, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void HasNo_resets_timeout_on_error() 
        {
            spyRobustWrapper.StubQueryResult(false, true);

            Configuration.Timeout = TimeSpan.FromSeconds(10);

            session.HasNo(new ElementScope(new AlwaysExceptionsErrorFinder(), session.DriverScope, spyRobustWrapper));
            try
            {
                ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();
                Assert.Fail("Expected an ExplicitlyThrownTestException");
            } 
            catch (ExplicitlyThrownTestException) { }

            Assert.That(Configuration.Timeout, Is.EqualTo(TimeSpan.FromSeconds(10)));
        }

        [Test]
        public void HasNo_wraps_query_to_return_true_if_found() 
        {
            spyRobustWrapper.StubQueryResult(false, true);

            session.HasNo(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper));

            var deferredResult = ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();
            Assert.That(deferredResult, Is.True);
        }

        [Test]
        public void HasNo_wraps_query_to_return_false_if_not_found() 
        {
            spyRobustWrapper.StubQueryResult(false, true);

            session.HasNo(new ElementScope(new AlwaysMissingElementFinder(), session.DriverScope, spyRobustWrapper));

            var deferredResult = ((Func<bool>)spyRobustWrapper.DeferredQueries[0])();
            Assert.That(deferredResult, Is.False);
        }

        [Test]
        public void HasNo_queries_robustly_reversing_result_Positive_case() 
        {
            spyRobustWrapper.StubQueryResult(false, true);

            Assert.That(session.HasNo(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper)), Is.False);
        }

        [Test]
        public void HasNo_queries_robustly_reversing_result_Negitive_case() 
        {
            spyRobustWrapper.StubQueryResult(false, false);

            Assert.That(session.HasNo(new ElementScope(new AlwaysFindsElementFinder(), session.DriverScope, spyRobustWrapper)), Is.True);
        }

    }
}