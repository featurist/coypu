using System;
using System.IO;
using System.Linq;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using Xunit;

namespace Coypu.AcceptanceTests
{
    /// <summary>
    /// Simple examples for each API method - to show usage and check everything is wired up properly
    /// </summary>
    public class StaleScopeExamples : IClassFixture<StaleScopeBrowserSessionFixture>
    {
        private BrowserSession browser;

        public StaleScopeExamples(StaleScopeBrowserSessionFixture fixture)
        {
            browser = fixture.BrowserSession;
        }

        private void VisitTestPage(string page)
        {
            browser.Visit("file:///" + new FileInfo(@"html\" + page).FullName.Replace("\\", "/"));
        }

        [Fact]
        public void Scope_becomes_stale()
        {
            VisitTestPage("tricky.htm");

            var section1 = browser.FindSection("section 1");
            Assert.True(section1.FindLink("the link").Exists());

            var originalLocation = browser.Location;

            VisitTestPage("iFrame1.htm");

            Assert.True(section1.FindLink("the link").Missing());

            browser.ExecuteScript("window.setTimeout(function() {window.location.href = '" + originalLocation + "'},1000);");

            section1.ClickLink("the link");
        }


        [Fact]
        public void Scope_becomes_stale_looking_for_all_xpath()
        {
            VisitTestPage("tricky.htm");

            var section1 = browser.FindSection("section 1");
            Assert.True(section1.FindLink("the link").Exists());

            VisitTestPage("iFrame1.htm");
            VisitTestPage("tricky.htm");
            Assert.True(section1.FindAllXPath("*").Count() > 0);
        }
        [Fact]
        public void Scope_becomes_stale_looking_for_all_css()
        {
            VisitTestPage("tricky.htm");

            var section1 = browser.FindSection("section 1");
            Assert.True(section1.FindLink("the link").Exists());

            VisitTestPage("iFrame1.htm");
            VisitTestPage("tricky.htm");
            Assert.True(section1.FindAllCss("*").Count() > 0);
        }


        [Fact]
        public void Scope_becomes_stale_iframe()
        {
            VisitTestPage("InteractionTestsPage.htm");

            var originalLocation = browser.Location;

            var iframe1 = browser.FindFrame("iframe1");
            var iframe2 = browser.FindFrame("iframe2");
            var button = iframe1.FindButton("scoped button");

            Assert.True(button.Exists());

            Assert.True(iframe1.HasContent("I am iframe one"));

            VisitTestPage("tricky.htm");

            Assert.True(iframe1.Missing());
            Assert.True(button.Missing());

            browser.ExecuteScript("window.setTimeout(function() {window.location.href = '" + originalLocation + "'},1000);");

            Assert.True(iframe1.HasContent("I am iframe one"));
            Assert.True(iframe2.HasContent("I am iframe two"));

            Assert.Equal("Coypu interaction tests page", browser.Title);

            button.Click();
        }

        [Fact]
        public void Scope_becomes_stale_window()
        {
            VisitTestPage("InteractionTestsPage.htm");

            browser.ClickLink("Open pop up window");

            var timeToClickManuallyInIE = new Options {Timeout = TimeSpan.FromSeconds(10)};

            var popUp = browser.FindWindow("Pop Up Window");
            var button = popUp.FindButton("scoped button", timeToClickManuallyInIE);

            Assert.True(button.Exists());
            Assert.True(popUp.HasContent("I am a pop up window"));

            CloseWindow(popUp);
            Assert.True(popUp.Missing(), "Expected popUp window to be missing");
            Assert.True(button.Missing(), "Expected button in popup to be missing");

            browser.ClickLink("Open pop up window");

            Assert.True(popUp.HasContent("I am a pop up window", timeToClickManuallyInIE));
            button.Click();
        }

        [Fact]
        public void Window_is_not_refound_unless_stale()
        {
            VisitTestPage("InteractionTestsPage.htm");
            browser.ClickLink("Open pop up window");

            var popUp = browser.FindWindow("Pop Up Window");
            var popUpBody = popUp.FindCss("body");

            popUp.ExecuteScript("document.title = 'Changed title';");
            Assert.True(browser.FindLink("Open pop up window").Exists());

            Assert.True(popUp.HasContent("I am a pop up window"));

            var findPopUpAgain = browser.FindWindow("Pop Up Window");
            Assert.True(findPopUpAgain.Missing(), "Expected pop-up not to be found now title has changed");
        }


        private static void CloseWindow(BrowserWindow popUp)
        {
            try
            {
                popUp.ExecuteScript("self.close();");
            }
            catch (Exception)
            {
                // IE permissions
            }
        }
    }

    public class StaleScopeBrowserSessionFixture : IDisposable
    {
        public BrowserSession BrowserSession;

        public StaleScopeBrowserSessionFixture()
        {
            var configuration = new SessionConfiguration
            {
                Timeout = TimeSpan.FromMilliseconds(2000),
                Driver = typeof(SeleniumWebDriver),
                Browser = Browser.Chrome
            };
            BrowserSession = new BrowserSession(configuration);
        }

        public void Dispose()
        {
            BrowserSession.Dispose();
        }
    }
}