using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Coypu.Drivers.Selenium;
using Coypu.Should;
using Machine.Specifications;

namespace Coypu.AcceptanceTests
{
    [Subject("Coypu.Should")]
    public class Given_browser : ShouldExamples
    {
        // ShouldHave

        It should_have_content = () =>
        {
            browser.ShouldHaveContent("This is what we are looking for");
            browser.ShouldHaveContent(new Regex("This is what (we are|I am) looking for"));
        };

        It should_have_css = () =>
        {
            browser.ShouldHaveCss(".css-test");
            browser.ShouldHaveCss(".css-test", "This is what we are looking for");
            browser.ShouldHaveCss(".css-test", new Regex("This is what (we are|I am) looking for"));
        };

        It should_have_content_containing = () =>
        {
            browser.ShouldHaveContentContaining("Some", "text", "in", "a", "list");
            browser.ShouldHaveContentContaining(new[] { "Some", "text", "in", "a", "list" }, Options.Exact);
        };

        It should_have_css_containing = () =>
        {
            browser.ShouldHaveCssContaining("#inspectingContent ul li", "Some", "text", "in", "a", "list");
            browser.ShouldHaveCssContaining("#inspectingContent ul li", new[] { "Some", "text", "in", "a", "list" }, Options.Exact);
            browser.ShouldHaveCssContaining("#inspectingContent ul li", new[] { new Regex("(table row|list)") });
            browser.ShouldHaveCssContaining("#inspectingContent ul li", new[] { new Regex("(table row|list)") }, Options.Exact);
        };

        It should_have_all_css_in_order = () =>
        {
            browser.ShouldHaveAllCssInOrder("#inspectingContent ul li", "Some", "text", "in", "a", "list", "one", "two", "Me! Pick me!");
            browser.ShouldHaveAllCssInOrder("#inspectingContent ul li", new[] { "Some", "text", "in", "a", "list", "one", "two", "Me! Pick me!" }, Options.Exact);
            browser.ShouldHaveAllCssInOrder("#inspectingContent ul#cssTest li", new Regex("(one|1)"), new Regex("(two|2)"), new Regex("Me! Pick me!"));
            browser.ShouldHaveAllCssInOrder("#inspectingContent ul#cssTest li", new[] { new Regex("(one|1)"), new Regex("(two|2)"), new Regex("Me! Pick me!") }, Options.Exact);
        };

        It should_have_value = () => browser.FindField("find-this-field").ShouldHaveValue("This value is what we are looking for");

        // ShouldNotHave

        It should_not_have_content = () => browser.ShouldNotHaveContent("This is not in the page");

        It should_not_have_css = () =>
        {
            browser.ShouldNotHaveCss(".this-css-selector-does-not-exist");
            browser.ShouldNotHaveCss(".css-test", "This is not in the page");
            browser.ShouldNotHaveCss(".css-test", new Regex("This is not in the (page|frame)"));
        };

        It should_not_have_value = () => browser.FindField("find-this-field").ShouldNotHaveValue("This is not the value");

        // Machine.Specifications.Should

        It should_work_with_classic_should_extension_metods = () =>
        {
            browser.FindCss(".css-test").Exists().ShouldBeTrue();
            browser.FindCss(".this-css-selector-does-not-exist").Missing().ShouldBeTrue();
            browser.HasContent("This is what we are looking for").ShouldBeTrue();
            browser.FindAllCss("form").Count().ShouldEqual(2);
            browser.Location.ToString().ShouldEndWith("InteractionTestsPage.htm");
            browser.Title.ShouldEqual("Coypu interaction tests page");
        };
    }

    public class ShouldExamples
    {
        Establish context = () =>
        {
            var configuration = new SessionConfiguration
            {
                Timeout = TimeSpan.FromMilliseconds(2000),
                Browser = Drivers.Browser.Chrome,
                Driver = typeof(SeleniumWebDriver)
            };
            browser = new BrowserSession(configuration);

            ReloadTestPageWithDelay();
        };

        Cleanup after = () => browser.Dispose();

        protected static void ApplyAsyncDelay()
        {
            // Hide the HTML then bring back after a short delay to test robustness
            browser.ExecuteScript("window.holdIt = window.document.body.innerHTML;");
            browser.ExecuteScript("window.document.body.innerHTML = '';");
            browser.ExecuteScript("setTimeout(function() {document.body.innerHTML = window.holdIt},250)");
        }

        protected static void ReloadTestPage()
        {
            browser.Visit(TestPageLocation("InteractionTestsPage.htm"));
        }

        protected static string TestPageLocation(string page)
        {
            var testPageLocation = "file:///" + new FileInfo(@"html\" + page).FullName.Replace("\\", "/");
            return testPageLocation;
        }

        protected static void ReloadTestPageWithDelay()
        {
            ReloadTestPage();
            ApplyAsyncDelay();
        }

        protected static BrowserSession browser;
    }
}
