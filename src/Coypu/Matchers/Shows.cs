using NUnit.Framework.Constraints;

namespace Coypu.Matchers {
    public static class Shows {
        public static Constraint Content(string expectedContent, Options options = null) {
            return new HasContentMatcher(expectedContent, options);
        }

        public static Constraint Css(string expectedCssSelector, Options options = null) {
            return new HasCssMatcher(expectedCssSelector, options);
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
    }
}