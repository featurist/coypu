using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_the_page : When_inspecting
    {
        [Test]
        public void HasContent_should_wait_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasContent, driver.StubHasContent);
        }

        [Test]
        public void HasContent_should_wait_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasContent, driver.StubHasContent);
        }

        [Test]
        public void HasNoContent_should_wait_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoContent, driver.StubHasContent);
        }

        [Test]
        public void HasNoContent_should_wait_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoContent, driver.StubHasContent);
        }

        [Test]
        public void HasCss_should_wait_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasCss, driver.StubHasCss);
        }

        [Test]
        public void HasCss_should_wait_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasCss, driver.StubHasCss);
        }

        [Test]
        public void HasNoCss_should_wait_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoCss, driver.StubHasCss);
        }

        [Test]
        public void HasNoCss_should_wait_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoCss, driver.StubHasCss);
        }

        [Test]
        public void HasXPath_should_wait_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(true, true, session.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasXPath_should_wait_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(true, false, session.HasXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_should_wait_for_robustly_Positive_example()
        {
            Should_wait_for_robustly(false, true, session.HasNoXPath, driver.StubHasXPath);
        }

        [Test]
        public void HasNoXPath_should_wait_for_robustly_Negative_example()
        {
            Should_wait_for_robustly(false, false, session.HasNoXPath, driver.StubHasXPath);
        }
    }
}