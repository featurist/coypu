using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var scope = ((DriverScope)actual);
            var hasContent = scope.HasContent(_expectedContent, _options);
            if (!hasContent)
            {
                _actualContent = scope.Text;
                hasContent = _actualContent.Contains(_expectedContent);
            }
            return hasContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("Expected to find content: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }

    public class HasContentMatchMatcher : Constraint
    {
        private readonly Regex _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasContentMatchMatcher(Regex expectedContent, Options options)
        {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            var scope = ((DriverScope)actual);
            var hasContent = scope.HasContentMatch(_expectedContent, _options);
            if (!hasContent)
            {
                _actualContent = scope.Text;
                hasContent = _expectedContent.IsMatch(_actualContent);
            }
            return hasContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Expected to find content: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }
}