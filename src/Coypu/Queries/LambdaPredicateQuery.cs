using System;

namespace Coypu.Queries
{
    public class LambdaPredicateQuery : PredicateQuery
    {
        private readonly Func<bool> query;

        public LambdaPredicateQuery(Func<bool> query, Options options = null) : base(options)
        {
            this.query = query;
        }

        public override bool Predicate()
        {
            return query();
        }
    }
}