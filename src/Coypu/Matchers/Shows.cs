using NUnit.Framework.Constraints;

namespace Coypu.Matchers {
    public static class Shows {
        public static Constraint Content(string expectedContent, Options options = null) {
            return new HasContentMatcher(expectedContent, options);
        }

        public static ShowsNo No = new ShowsNo();
    }

    public class ShowsNo {
        public Constraint Content(string expectedContent, Options options = null) {
            return new HasNoContentMatcher(expectedContent, options);
        }
    }

    public class HasContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        protected string FailureMessagePrefix = "Expected to find";
        private string _actualContent;

        public HasContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasContent = scope.HasContent(_expectedContent, _options);
            if (!hasContent)
                _actualContent = scope.Now().Text;
            return hasContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("{0} content: {1}\nin:\n{2}", FailureMessagePrefix, _expectedContent, _actualContent);
        }
    }

    public class HasNoContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        protected string FailureMessagePrefix = "Expected to find";
        private string _actualContent;

        public HasNoContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasContent = scope.HasNoContent(_expectedContent, _options);
            if (!hasContent)
                _actualContent = scope.Now().Text;
            return hasContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("{0} content: {1}\nin:\n{2}", FailureMessagePrefix, _expectedContent, _actualContent);
        }
    }
}