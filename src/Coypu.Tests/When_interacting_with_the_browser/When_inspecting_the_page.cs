using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_the_page : When_inspecting
    {
        [Test]
        public void HasContent_queries_robustly_Positive_example()
        {
            Queries_robustly(true, browserSession.HasContent, driver.StubHasContent);
        }

        [Test]
        public void HasContent_queries_robustly_Negative_example()
        {
            Queries_robustly(false, browserSession.HasContent, driver.StubHasContent);
        }
        
        [Test]
        public void HasContentMatch_queries_robustly_Positive_example()
        {
            Queries_robustly(true, browserSession.HasContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasContentMatch_queries_robustly_Negative_example()
        {
            Queries_robustly(false, browserSession.HasContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasNoContent_queries_robustly_Positive_example()
        {
            Queries_robustly_reversing_result(true, browserSession.HasNoContent, driver.StubHasContent);
        }

        [Test]
        public void HasNoContent_queries_robustly_Negative_example()
        {
            Queries_robustly_reversing_result(false, browserSession.HasNoContent, driver.StubHasContent);
        }
        
        [Test]
        public void HasNoContentMatch_queries_robustly_Positive_example()
        {
            Queries_robustly_reversing_result(true, browserSession.HasNoContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasNoContentMatch_queries_robustly_Negative_example()
        {
            Queries_robustly_reversing_result(false, browserSession.HasNoContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
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

    }
}