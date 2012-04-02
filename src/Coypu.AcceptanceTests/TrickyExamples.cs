using System;
using System.IO;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    /// <summary>
    /// Simple examples for each API method - to show usage and check everything is wired up properly
    /// </summary>
    [TestFixture]
    public class TrickyExamples
    {
        private BrowserSession browser;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            var configuration = new SessionConfiguration
                                    {
                                        Timeout = TimeSpan.FromMilliseconds(2000),
                                        Driver = typeof(SeleniumWebDriver),
                                        Browser = Drivers.Browser.Firefox
                                    };
            browser = new BrowserSession(configuration);

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            browser.Dispose();
        }

        
        private void VisitTestPage(string page)
        {
            browser.Visit("file:///" + new FileInfo(@"html\" + page).FullName.Replace("\\", "/"));
        }

        [Test]
        public void Scope_becomes_stale()
        {
            VisitTestPage("tricky.htm");

            var section1 = browser.FindSection("section 1");
            Assert.That(section1.FindLink("the link").Exists());

            var originalLocation = browser.Location;

            VisitTestPage("iFrame1.htm");

            Assert.That(section1.FindLink("the link").Missing());

            browser.ExecuteScript("window.setTimeout(function() {window.location.href = '" + originalLocation + "'},1000);");

            Assert.That(section1.FindLink("the link").Exists());
        }

        [Test]
        public void Scope_becomes_stale_iframe()
        {
            VisitTestPage("InteractionTestsPage.htm");

            var originalLocation = browser.Location;

            var iframe = browser.FindIFrame("iframe1");
            var button = iframe.FindButton("scoped button");

            Assert.That(iframe.HasContent("I am iframe one"));

            VisitTestPage("tricky.htm");

            Assert.That(iframe.Missing());
            Assert.That(button.Missing());

            browser.ExecuteScript("window.setTimeout(function() {window.location.href = '" + originalLocation + "'},1000);");

            Assert.That(iframe.HasContent("I am iframe one"));
            button.Click();
        }

        [Test]
        public void Scope_becomes_stale_window()
        {
            VisitTestPage("InteractionTestsPage.htm");

            browser.ClickLink("Open pop up window");
            
            var popUp = browser.FindWindow("Pop Up Window");
            var button = popUp.FindButton("scoped button");
            
            Assert.That(button.Exists());
            Assert.That(popUp.HasContent("I am a pop up window"));

            popUp.ExecuteScript("self.close()");

            Assert.That(button.Missing());

            browser.ClickLink("Open pop up window");

            Assert.That(popUp.HasContent("I am a pop up window"));
            button.Click();
        }
    }
}