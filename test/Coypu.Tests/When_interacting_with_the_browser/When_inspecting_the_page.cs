using System;
using System.Text.RegularExpressions;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting_the_page : When_inspecting
    {
        [Fact]
        public void HasContent_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasContent, "content in which");
        }

        [Fact]
        public void HasContent_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasContent, "content not there");
        }

        [Fact]
        public void HasContentMatch_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasContentMatch, new Regex("in wh[iI]ch to look$"));
        }

        [Fact]
        public void HasContentMatch_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasContentMatch, new Regex("some r[eE]gex"));
        }

        [Fact]
        public void HasNoContent_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasNoContent, "content not there");
        }

        [Fact]
        public void HasNoContent_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasNoContent, "content in which");
        }

        [Fact]
        public void HasNoContentMatch_queries_robustly_Positive_example()
        {
            Check_robust_content_query(true, "some content in which to look", browserSession.HasNoContentMatch, new Regex("some r[eE]gex"));
        }

        [Fact]
        public void HasNoContentMatch_queries_robustly_Negative_example()
        {
            Check_robust_content_query(false, "some content in which to look", browserSession.HasNoContentMatch, new Regex("in wh[iI]ch to look$"));
        }


        private void Check_robust_content_query<T>(bool stubResult, string actualContent, Func<T, Options, bool> subject, T toLookFor)
        {
            var window = new StubElement { Text = actualContent };
            driver.StubCurrentWindow(window);

            SpyTimingStrategy.StubQueryResult(true, !stubResult);

            var individualTimeout = TimeSpan.FromMilliseconds(DateTime.UtcNow.Millisecond);
            var options = new SessionConfiguration { Timeout = individualTimeout };

            var actualImmediateResult = subject(toLookFor, options);

            Assert.Equal(!stubResult, actualImmediateResult);

            var queryResult = RunQueryAndCheckTiming<bool>(individualTimeout);

            Assert.Equal(stubResult, queryResult);
        }

    }
}