using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.Matchers {
    public static class Shows {
        public static Constraint Content(string expectedContent, Options options = null) {
            return new HasContentMatcher(expectedContent, options);
        }

        public static Constraint Css(string expectedCssSelector, Options options = null)
        {
            return new HasCssMatcher(expectedCssSelector, options);
        }

        public static Constraint Css(string expectedCssSelector, string text, Options options = null)
        {
            return new HasCssMatcher(expectedCssSelector, text, options);
        }

        public static Constraint Css(string expectedCssSelector, Regex text, Options options = null)
        {
            return new HasCssMatcher(expectedCssSelector, text, options);
        }

        public static ShowsNo No = new ShowsNo();
    }

    public class ShowsNo
    {
        public Constraint Content(string expectedContent, Options options = null)
        {
            return new HasNoContentMatcher(expectedContent, options);
        }

        public Constraint Css(string expectedCssSelector, Options options = null)
        {
            return new HasNoContentMatcher(expectedCssSelector, options);
        }

        public Constraint Css(string expectedCssSelector, string text, Options options = null)
        {
            return new HasNoCssMatcher(expectedCssSelector, text, options);
        }

        public Constraint Css(string expectedCssSelector, Regex text, Options options = null)
        {
            return new HasNoCssMatcher(expectedCssSelector, text, options);
        }
    }
}