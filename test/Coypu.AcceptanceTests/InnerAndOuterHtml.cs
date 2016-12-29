using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class InnerAndOuterHtml : IDisposable
    {
        private SessionConfiguration SessionConfiguration;
        private BrowserSession browser;

        public InnerAndOuterHtml()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.Timeout = TimeSpan.FromMilliseconds(1000);
            browser = new BrowserSession(SessionConfiguration);
            browser.Visit("file:///" + new FileInfo(@"html\table.htm").FullName.Replace("\\", "/"));
        }

        public void Dispose()
        {
            browser.Dispose();
        }

        [Fact]
        public void GrabsTheOuterHTMLFromAnElement()
        {
            var outerHTML = Normalise(browser.FindCss("table").OuterHTML);
            Assert.Equal("<table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table>", outerHTML);
        }

        [Fact]
        public void GrabsTheInnerHTMLFromAnElement()
        {
            var innerHTML = Normalise(browser.FindCss("table").InnerHTML);
            Assert.Equal("<tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody>", innerHTML);
        }

        private static string Normalise(string innerHtml)
        {
            return new Regex(@"\s+", RegexOptions.Multiline).Replace(innerHtml, "").ToLower();
        }
    }
}
