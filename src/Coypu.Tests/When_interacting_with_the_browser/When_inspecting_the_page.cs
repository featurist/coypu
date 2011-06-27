using System;
using System.Linq;
using Coypu.Drivers;
using Coypu.Tests.TestDoubles;
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
            Queries_robustly(true, true, session.HasContent, driver.StubHasContent);
        }

        [Test]
        public void HasContent_queries_robustly_Negative_example()
        {
            Queries_robustly(true, false, session.HasContent, driver.StubHasContent);
        }
        
        [Test]
        public void HasContentMatch_queries_robustly_Positive_example()
        {
            Queries_robustly(true, true, session.HasContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasContentMatch_queries_robustly_Negative_example()
        {
            Queries_robustly(true, false, session.HasContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasNoContent_queries_robustly_Positive_example()
        {
            Queries_robustly(false, true, session.HasNoContent, driver.StubHasContent);
        }

        [Test]
        public void HasNoContent_queries_robustly_Negative_example()
        {
            Queries_robustly(false, false, session.HasNoContent, driver.StubHasContent);
        }
        
        [Test]
        public void HasNoContentMatch_queries_robustly_Positive_example()
        {
            Queries_robustly(false, true, session.HasNoContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasNoContentMatch_queries_robustly_Negative_example()
        {
            Queries_robustly(false, false, session.HasNoContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasCss_queries_robustly_Positive_example()
        {
            Queries_robustly(true, true, session.HasCss, driver.StubHasCss);
        }

        [Test]
        public void HasCss_queries_robustly_Negative_example()
        {
            Queries_robustly(true, false, session.HasCss, driver.StubHasCss);
        }

        [Test]
        public void HasNoCss_queries_robustly_Positive_example()
        {
            Queries_robustly(false, true, session.HasNoCss, driver.StubHasCss);
        }

        [Test]
        public void HasNoCss_queries_robustly_Negative_example()
        {
            Queries_robustly(false, false, session.HasNoCss, driver.StubHasCss);
        }

        [Test]
        public void HasXPath_queries_robustly_Positive_example()
        {
            Queries_robustly(true, true, session.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasXPath_queries_robustly_Negative_example()
        {
            Queries_robustly(true, false, session.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_queries_robustly_Positive_example()
        {
            Queries_robustly(false, true, session.HasNoXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_queries_robustly_Negative_example()
        {
            Queries_robustly(false, false, session.HasNoXPath, driver.StubHasXPath);
        }

    }
}