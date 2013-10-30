using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu.Matchers
{
    public class HasValueMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasValueMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var elementScope = ((ElementScope)actual);
            var hasValue = elementScope.HasValue(_expectedContent, _options);
            if (!hasValue)
                _actualContent = elementScope.Value;
            return hasValue;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("Expected to find value: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }
}