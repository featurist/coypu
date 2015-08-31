using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using _ = Coypu.NUnit.Matchers.Shows; // for fluent readability

namespace Coypu.Should
{
    public static class ShouldExtensions
    {
        // ShouldHave

        public static void ShouldHaveContent(this Scope scope, string expectedContent, Options options = null)
        {
            scope.ShouldHave(_.Content(expectedContent, options));
        }

        public static void ShouldHaveContent(this Scope scope, Regex expectedContent, Options options = null)
        {
            scope.ShouldHave(_.Content(expectedContent, options));
        }

        public static void ShouldHaveCss(this Scope scope, string expectedCssSelector, Options options = null)
        {
            scope.ShouldHave(_.Css(expectedCssSelector, options));
        }

        public static void ShouldHaveCss(this Scope scope, string expectedCssSelector, string text, Options options = null)
        {
            scope.ShouldHave(_.Css(expectedCssSelector, text, options));
        }

        public static void ShouldHaveCss(this Scope scope, string expectedCssSelector, Regex text, Options options = null)
        {
            scope.ShouldHave(_.Css(expectedCssSelector, text, options));
        }

        public static void ShouldHaveContentContaining(this Scope scope, params string[] expectedContent)
        {
            scope.ShouldHave(_.ContentContaining(expectedContent));
        }

        public static void ShouldHaveContentContaining(this Scope scope, IEnumerable<string> expectedContent, Options options)
        {
            scope.ShouldHave(_.ContentContaining(expectedContent, options));
        }

        public static void ShouldHaveCssContaining(this Scope scope, string expectedCssSelector, params string[] text)
        {
            scope.ShouldHave(_.CssContaining(expectedCssSelector, text));
        }

        public static void ShouldHaveCssContaining(this Scope scope, string expectedCssSelector, IEnumerable<string> text, Options options)
        {
            scope.ShouldHave(_.CssContaining(expectedCssSelector, text, options));
        }

        public static void ShouldHaveCssContaining(this Scope scope, string expectedCssSelector, Regex[] text)
        {
            scope.ShouldHave(_.CssContaining(expectedCssSelector, text));
        }

        public static void ShouldHaveCssContaining(this Scope scope, string expectedCssSelector, IEnumerable<Regex> text, Options options)
        {
            scope.ShouldHave(_.CssContaining(expectedCssSelector, text, options));
        }

        public static void ShouldHaveAllCssInOrder(this Scope scope, string expectedCssSelector, params string[] text)
        {
            scope.ShouldHave(_.AllCssInOrder(expectedCssSelector, text));
        }

        public static void ShouldHaveAllCssInOrder(this Scope scope, string expectedCssSelector, IEnumerable<string> text, Options options)
        {
            scope.ShouldHave(_.AllCssInOrder(expectedCssSelector, text, options));
        }

        public static void ShouldHaveAllCssInOrder(this Scope scope, string expectedCssSelector, params Regex[] textMatching)
        {
            scope.ShouldHave(_.AllCssInOrder(expectedCssSelector, textMatching));
        }

        public static void ShouldHaveAllCssInOrder(this Scope scope, string expectedCssSelector, IEnumerable<Regex> textMatching, Options options = null)
        {
            scope.ShouldHave(_.AllCssInOrder(expectedCssSelector, textMatching, options));
        }

        public static void ShouldHaveValue(this Scope scope, string expectedContent, Options options = null)
        {
            scope.ShouldHave(_.Value(expectedContent, options));
        }

        // ShouldNotHave

        public static void ShouldNotHaveContent(this Scope scope, string expectedContent, Options options = null)
        {
            scope.ShouldNotHave(_.Content(expectedContent, options));
        }

        public static void ShouldNotHaveCss(this Scope scope, string expectedCssSelector, Options options = null)
        {
            scope.ShouldNotHave(_.Css(expectedCssSelector, options));
        }

        public static void ShouldNotHaveCss(this Scope scope, string expectedCssSelector, string text, Options options = null)
        {
            scope.ShouldNotHave(_.Css(expectedCssSelector, text, options));
        }

        public static void ShouldNotHaveCss(this Scope scope, string expectedCssSelector, Regex text, Options options = null)
        {
            scope.ShouldNotHave(_.Css(expectedCssSelector, text, options));
        }

        public static void ShouldNotHaveValue(this Scope scope, string expectedContent, Options options = null)
        {
            scope.ShouldNotHave(_.Value(expectedContent, options));
        }

        // Assert

        private static void ShouldHave(this Scope scope, IResolveConstraint constraint)
        {
            scope.ShouldHave(constraint.Resolve());
        }

        private static void ShouldHave(this Scope scope, Constraint constraint)
        {
            if (constraint.Matches(scope)) return;

            Throw(constraint);
        }

        private static void ShouldNotHave(this Scope scope, Constraint constraint)
        {
            if (!constraint.Matches(scope)) return;

            Throw(constraint);
        }

        private static void Throw(Constraint constraint)
        {
            var writer = new TextMessageWriter();
            constraint.WriteMessageTo(writer);

            throw new ShouldException(writer.ToString());
        }
    }
}