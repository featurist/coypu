using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_html : DriverSpecs
    {
        public void VisitTestPage()
        {
            Driver.Visit("file:///" + new FileInfo(@"html\table.htm").FullName.Replace("\\", "/"), Root);
        }

        [Fact]
        public void FindsElementOuterHTML()
        {
            VisitTestPage();

            var outerHTML = Normalise(Css("table").OuterHTML);
            Assert.That(outerHTML, Is.EqualTo("<table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table>"));
        }

        [Fact]
        public void FindsElementInnerHTML()
        {
            VisitTestPage();

            var innerHTML = Normalise(Css("table").InnerHTML);
            Assert.That(innerHTML, Is.EqualTo("<tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody>"));
        }

        [Fact]
        public void FindsWindowOuterHTML()
        {
            VisitTestPage();

            var outerHTML = Normalise(Driver.Window.OuterHTML);
            Assert.That(outerHTML, Is.EqualTo("<html><head><title>table</title></head><body><table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table></body></html>"));
        }

        [Fact]
        public void FindsWindowInnerHTML()
        {
            VisitTestPage();

            var innerHTML = Normalise(Driver.Window.InnerHTML);
            Assert.That(innerHTML, Is.EqualTo("<head><title>table</title></head><body><table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table></body>"));
        }

        private static string Normalise(string innerHtml)
        {
            return new Regex(@"\s+<", RegexOptions.Multiline).Replace(innerHtml, "<").Replace(" webdriver=\"true\"","").ToLower().TrimEnd();
        }
    }
}