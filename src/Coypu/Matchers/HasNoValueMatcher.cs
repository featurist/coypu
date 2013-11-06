using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu.Matchers
{
    public class HasNoValueMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasNoValueMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var elementScope = ((ElementScope)actual);
            var hasNoValue = elementScope.HasNoValue(_expectedContent, _options);
            if (!hasNoValue)
                _actualContent = elementScope.Value;
            return hasNoValue;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("Expected NOT to find value: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }
}