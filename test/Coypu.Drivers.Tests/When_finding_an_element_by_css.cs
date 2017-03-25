using Shouldly;
using System.Text.RegularExpressions;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_an_element_by_css : DriverSpecs
    { 
            [Fact]
            public void Finds_present_examples()
            {
                var shouldFind = "#inspectingContent p.css-test span";
                Css(shouldFind).Text.ShouldBe("This");

                shouldFind = "ul#cssTest li:nth-child(3)";
                Css(shouldFind).Text.ShouldBe("Me! Pick me!");
            }

            [Fact]
            public void Finds_present_examples_by_text()
            {
                var shouldFind = "#inspectingContent p.css-test span";
                Css(shouldFind, new Regex("^This$")).Text.ShouldBe("This");

                shouldFind = "ul#cssTest li:nth-child(3)";
                Css(shouldFind, new Regex("Pick me")).Text.ShouldBe("Me! Pick me!");
            }

            [Fact]
            public void Does_not_find_missing_examples()
            {
                const string shouldNotFind = "#inspectingContent p.css-missing-test";
                Assert.Throws<MissingHtmlException>(() => Css(shouldNotFind, Root));
            }

            [Fact]
            public void Does_not_find_missing_examples_by_text()
            {
                const string shouldFind = "ul#cssTest li:nth-child(3)";
                var missingTextPattern = new Regex("Drop me");
                Assert.Throws<MissingHtmlException>(() => Css(shouldFind, missingTextPattern));
            }

            
            [Fact]
            public void Only_finds_visible_elements()
            {
                const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
                Assert.Throws<MissingHtmlException>(() => Css(shouldNotFind,Root));
            }
        }
    
}