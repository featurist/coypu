using System;
using System.Text.RegularExpressions;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_the_page : When_inspecting
    {
        [Test]
        public void HasContent_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasContent, "content in which");
        }

        [Test]
        public void HasContent_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasContent, "content not there");
        }

        [Test]
        public void HasContentMatch_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasContentMatch, new Regex("in wh[iI]ch to look$"));
        }

        [Test]
        public void HasContentMatch_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasContentMatch, new Regex("some r[eE]gex"));
        }

        [Test]
        public void HasNoContent_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasNoContent, "content not there");
        }

        [Test]
        public void HasNoContent_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasNoContent, "content in which");
        }

        [Test]
        public void HasNoContentMatch_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasNoContentMatch, new Regex("some r[eE]gex"));
        }

        [Test]
        public void HasNoContentMatch_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasNoContentMatch, new Regex("in wh[iI]ch to look$"));
        }


        [Test]
        public void HasXPath_queries_robustly_Positive_example()
        {
            Queries_robustly(true, browserSession.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasXPath_queries_robustly_Negative_example()
        {
            Queries_robustly(false, browserSession.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_queries_robustly_Positive_example()
        {
            Queries_robustly_reversing_result(true, browserSession.HasNoXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_queries_robustly_Negative_example()
        {
            Queries_robustly_reversing_result(false, browserSession.HasNoXPath, driver.StubHasXPath);
        }

        private void Check_robust_content_query<T>(bool stubResult, string actualContent, Func<T, Options, bool> subject, T toLookFor)
        {
            var window = new StubElement {Text = actualContent};
            driver.StubCurrentWindow(window);

            spyRobustWrapper.StubQueryResult(true, !stubResult);

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);
            var options = new SessionConfiguration {Timeout = individualTimeout};

            var actualImmediateResult = subject(toLookFor, options);

            Assert.That(actualImmediateResult, Is.EqualTo(!stubResult), "Result was not found robustly");

            var queryResult = RunQueryAndCheckTiming<bool>(individualTimeout);

            Assert.That(queryResult, Is.EqualTo(stubResult));
        }
    }
}