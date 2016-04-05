using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class AggregateMatcher<T> : Constraint where T:Constraint
    {
        readonly IEnumerable<T> _innerConstraints;

        public AggregateMatcher(IEnumerable<T> innerConstraints)
        {
            _innerConstraints = innerConstraints;

        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            foreach (var innerConstraint in _innerConstraints)
            {
                var result = innerConstraint.ApplyTo(actual);

                if (!result.IsSuccess)
                    return new ConstraintResult(innerConstraint, result.ActualValue, false);
            }

            return new ConstraintResult(this, actual, true);
        }
    }
}