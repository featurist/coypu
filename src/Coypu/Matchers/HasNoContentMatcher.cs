using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class AggregateMatcher<T> : Constraint where T:Constraint
    {
        readonly IEnumerable<T> _innerConstraints;

        public AggregateMatcher(IEnumerable<T> innerConstraints)
        {
            _innerConstraints = innerConstraints;

        }

        public override bool Matches(object actual)
        {
            return _innerConstraints.All(c => c.Matches(actual));
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            foreach (var constraint in _innerConstraints)
                constraint.WriteDescriptionTo(writer);
        }

    }

    public class HasNoContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasNoContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasNoContent = scope.HasNoContent(_expectedContent, _options);
            if (!hasNoContent)
                _actualContent = scope.Now().Text;
            return hasNoContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("Expected NOT to find content: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }
}