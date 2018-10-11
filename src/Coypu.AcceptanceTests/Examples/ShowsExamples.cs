using Coypu.NUnit.Matchers;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class ShowsExamples : WaitAndRetryExamples
    {
        [Test]
        public void ShowsAllCssInOrder_example()
        {
            Assert.That(Browser,
                        Shows.AllCssInOrder("#inspectingContent ul li",
                                            "Some",
                                            "text",
                                            "in",
                                            "a",
                                            "list",
                                            "one",
                                            "two",
                                            "Me! Pick me!"));
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.AllCssInOrder("#inspectingContent ul li",
                                                                                    "Some",
                                                                                    "text",
                                                                                    "in",
                                                                                    "a",
                                                                                    "list",
                                                                                    "two",
                                                                                    "one",
                                                                                    "Me! Pick me!")));
        }

        [Test]
        public void ShowsContentContaining_example()
        {
            Assert.That(Browser, Shows.ContentContaining("Some", "text", "in", "a", "list"));
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.ContentContaining("this is not in the page",
                                                                                        "in",
                                                                                        "a",
                                                                                        "list")));
        }

        [Test]
        public void ShowsCssContaining_example()
        {
            Assert.That(Browser, Shows.CssContaining("#inspectingContent ul li", "Some", "text", "in", "a", "list"));
            Assert.Throws<AssertionException>(() => Assert.That(Browser,
                                                                Shows.CssContaining("#inspectingContent ul li",
                                                                                    "missing",
                                                                                    "from",
                                                                                    "a",
                                                                                    "list")));
        }
    }
}