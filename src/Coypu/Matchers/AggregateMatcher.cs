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
}