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
                                        Browser = Browser.Firefox
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

            var iframe1 = browser.FindFrame("iframe1");
            var iframe2 = browser.FindFrame("iframe2");
            var button = iframe1.FindButton("scoped button");

            Assert.That(button.Exists());

            Assert.That(iframe1.HasContent("I am iframe one"));

            VisitTestPage("tricky.htm");

            Assert.That(iframe1.Missing());
            Assert.That(button.Missing());

            browser.ExecuteScript("window.setTimeout(function() {window.location.href = '" + originalLocation + "'},1000);");

            Assert.That(iframe1.HasContent("I am iframe one"));
            Assert.That(iframe2.HasContent("I am iframe two"));

            Assert.That(browser.Title, Is.EqualTo("Coypu interaction tests page"));

            button.Click();
        }

        [Test]
        public void Scope_becomes_stale_window()
        {
            VisitTestPage("InteractionTestsPage.htm");

            browser.ClickLink("Open pop up window");

            var timeToClickManuallyInIE = new Options {Timeout = TimeSpan.FromSeconds(10)};

            var popUp = browser.FindWindow("Pop Up Window");
            var button = popUp.FindButton("scoped button", timeToClickManuallyInIE);

            Assert.That(button.Exists());
            Assert.That(popUp.HasContent("I am a pop up window"));

            CloseWindow(popUp);

            Assert.That(button.Missing());

            browser.ClickLink("Open pop up window");

            Assert.That(popUp.HasContent("I am a pop up window", timeToClickManuallyInIE));
            button.Click();
        }

        private static void CloseWindow(BrowserWindow popUp)
        {
            try
            {
                popUp.ExecuteScript("self.close();");
            }
            catch (Exception InvalidCastException)
            {
                // IE permissions
            }
        }
    }
}