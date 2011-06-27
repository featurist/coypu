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
        public void HasContent_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasContent, driver.StubHasContent);
        }

        [Test]
        public void HasContent_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasContent, driver.StubHasContent);
        }
        
        [Test]
        public void HasContentMatch_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasContentMatch_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasNoContent_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoContent, driver.StubHasContent);
        }

        [Test]
        public void HasNoContent_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoContent, driver.StubHasContent);
        }
        
        [Test]
        public void HasNoContentMatch_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasNoContentMatch_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoContentMatch, driver.StubHasContentMatch, new Regex("some r[eE]gex^"));
        }

        [Test]
        public void HasCss_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasCss, driver.StubHasCss);
        }

        [Test]
        public void HasCss_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasCss, driver.StubHasCss);
        }

        [Test]
        public void HasNoCss_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoCss, driver.StubHasCss);
        }

        [Test]
        public void HasNoCss_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoCss, driver.StubHasCss);
        }

        [Test]
        public void HasXPath_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasXPath_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_waits_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_waits_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoXPath, driver.StubHasXPath);
        }

        [Test]
        public void Has_is_true_when_finder_returns_element() 
        {
            var elementWeAreLookingFor = new StubElement();
            var result = session.Has(() => elementWeAreLookingFor);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Has_is_false_when_element_not_found() 
        {
            var result = session.Has(() => { throw new  MissingHtmlException("Failed to find something");});
            Assert.That(result, Is.False);
        }
    }
}