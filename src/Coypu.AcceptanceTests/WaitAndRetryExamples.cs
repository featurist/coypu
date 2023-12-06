using System;
using System.IO;
using Coypu.Drivers.Playwright;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Coypu.AcceptanceTests
{
    public class WaitAndRetryExamples
    {
        protected BrowserSession Browser;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            var configuration = new SessionConfiguration
                                {
                                    Browser = Drivers.Browser.Chrome,
                                    Driver = typeof(PlaywrightDriver),
                                    Headless = false
                                };
            Browser = new BrowserSession(configuration);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Browser.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            ReloadTestPageWithDelay();
        }

        [TearDown]
        public void TearDownOnFail()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed) return;
            TearDown();
            SetUpFixture();
        }

        protected void ApplyAsyncDelay()
        {
            // Hide the HTML then bring back after a short delay to test robustness
            Browser.ExecuteScript("window.holdIt = window.document.body.innerHTML;");
            Browser.ExecuteScript("window.document.body.innerHTML = '';");
            Browser.ExecuteScript("setTimeout(function() {document.body.innerHTML = window.holdIt},250)");
        }

        protected void ReloadTestPage()
        {
            Browser.Visit(PathHelper.GetPageHtmlPath("InteractionTestsPage.htm"));
        }

        protected void ReloadTestPageWithDelay()
        {
            ReloadTestPage();
            ApplyAsyncDelay();
        }
    }
}
