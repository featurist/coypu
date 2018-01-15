using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class InnerAndOuterHtml
    {
        private SessionConfiguration SessionConfiguration;
        private BrowserSession browser;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.Timeout = TimeSpan.FromMilliseconds(1000);
            browser = new BrowserSession(SessionConfiguration);
            browser.Visit("file:///" + Path.Combine(TestContext.CurrentContext.TestDirectory, @"html\table.htm").Replace("\\", "/"));
        }

        [OneTimeTearDown]
        public void TearDownFixture()
        {
            browser.Dispose();
        }

        [Test]
        public void GrabsTheOuterHTMLFromAnElement()
        {
            var outerHTML = Normalise(browser.FindCss("table").OuterHTML);
            Assert.That(outerHTML, Is.EqualTo("<table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table>"));
        }

        [Test]
        public void GrabsTheInnerHTMLFromAnElement()
        {
            var innerHTML = Normalise(browser.FindCss("table").InnerHTML);
            Assert.That(innerHTML, Is.EqualTo("<tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody>"));
        }

        private static string Normalise(string innerHtml)
        {
            return new Regex(@"\s+", RegexOptions.Multiline).Replace(innerHtml, "").ToLower();
        }
    }
}
