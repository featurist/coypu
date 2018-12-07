using System.Text.RegularExpressions;
using Coypu.NUnit.Matchers;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class HasExamples : WaitAndRetryExamples
    {
        [Test]
        public void HasContent_example()
        {
            Assert.That(Browser, Shows.Content("This is what we are looking for"));
            Assert.That(Browser.HasContent("This is not in the page"), Is.False);
            Assert.Throws<AssertionException>(() => Assert.That(Browser, Shows.Content("This is not in the page")));
        }

        [Test]
        public void HasContentMatch_example()
        {
            Assert.IsTrue(Browser.HasContentMatch(new Regex("This is what (we are|I am) looking for")));
            Assert.IsFalse(Browser.HasContentMatch(new Regex("This is ?n[o|']t in the page")));
        }

        [Test]
        public void HasContentMatching_example()
        {
            Assert.That(Browser, Shows.Content(new Regex(@"This.is.what.we.are.looking.for")));
            Assert.That(Browser.HasContentMatch(new Regex(@"This.is.not.in.the.page")), Is.False);
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.Content(new Regex(@"This.is.not.in.the.page"))));
        }

        [Test]
        public void HasNoContent_example()
        {
            Browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.That(Browser, Shows.No.Content("This is not in the page"));

            ReloadTestPage();
            Assert.That(Browser.HasNoContent("This is what we are looking for"), Is.False);
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.No.Content("This is what we are looking for")));
        }

        [Test]
        public void HasNoContentMatch_example()
        {
            Browser.ExecuteScript(
                "document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.IsTrue(Browser.HasNoContentMatch(new Regex("This is ?n[o|']t in the page")));

            ReloadTestPage();
            Assert.IsFalse(Browser.HasNoContentMatch(new Regex("This is what (we are|I am) looking for")));
        }

        [Test]
        public void HasNoValue_example()
        {
            var field = Browser.FindField("find-this-field");
            Assert.That(field, Shows.No.Value("This is not the value"));
            Assert.IsFalse(field.HasNoValue("This value is what we are looking for"));
        }

        [Test]
        public void HasValue_example()
        {
            var field = Browser.FindField("find-this-field");
            Assert.That(field, Shows.Value("This value is what we are looking for"));
            Assert.IsFalse(field.HasValue("This is not the value"));
        }
    }
}