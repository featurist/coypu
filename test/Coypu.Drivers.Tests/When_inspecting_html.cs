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
            Assert.Equal("<table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table>", outerHTML);
        }

        [Fact]
        public void FindsElementInnerHTML()
        {
            VisitTestPage();

            var innerHTML = Normalise(Css("table").InnerHTML);
            Assert.Equal("<tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody>", innerHTML);
        }

        [Fact]
        public void FindsWindowOuterHTML()
        {
            VisitTestPage();

            var outerHTML = Normalise(Driver.Window.OuterHTML);
            Assert.Equal("<html><head><title>table</title></head><body><table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table></body></html>", outerHTML);
        }

        [Fact]
        public void FindsWindowInnerHTML()
        {
            VisitTestPage();

            var innerHTML = Normalise(Driver.Window.InnerHTML);
            Assert.Equal("<head><title>table</title></head><body><table><tbody><tr><th>name</th><th>age</th></tr><tr><td>bob</td><td>12</td></tr><tr><td>jane</td><td>79</td></tr></tbody></table></body>", innerHTML);
        }

        private static string Normalise(string innerHtml)
        {
            return new Regex(@"\s+<", RegexOptions.Multiline).Replace(innerHtml, "<").Replace(" webdriver=\"true\"","").ToLower().TrimEnd();
        }
    }
}