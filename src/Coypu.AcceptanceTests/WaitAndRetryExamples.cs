using System;
using System.IO;
using Coypu.Drivers.Selenium;
using Coypu.Drivers.Watin;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    public class WaitAndRetryExamples
    {
        protected BrowserSession browser;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            var configuration = new SessionConfiguration
                {
                    Timeout = TimeSpan.FromMilliseconds(2000),
                    Browser = Drivers.Browser.Chrome,
                    Driver = typeof(SeleniumWebDriver)
                };
            browser = new BrowserSession(configuration);

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            browser.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            ReloadTestPageWithDelay();
        }

        protected void ApplyAsyncDelay()
        {
            // Hide the HTML then bring back after a short delay to test robustness
            browser.ExecuteScript("window.document.body.innerHTML = '';");
            browser.ExecuteScript("window.holdIt = window.document.body.innerHTML;");
            browser.ExecuteScript("setTimeout(function() {document.body.innerHTML = window.holdIt},250)");
        }

        protected void ReloadTestPage()
        {
            browser.Visit(TestPageLocation("InteractionTestsPage.htm"));
        }

        protected static string TestPageLocation(string page)
        {
            var fileLocation = new FileInfo(@"html\" + page).FullName.Replace("\\", "/");
            if (fileLocation.StartsWith("/"))
            {
                fileLocation = fileLocation.Substring(1);
            }

            var testPageLocation = String.Format("file:///{0}", fileLocation);
            return testPageLocation;
        }

        protected void ReloadTestPageWithDelay()
        {
            ReloadTestPage();
            ApplyAsyncDelay();
        }   
    }
}