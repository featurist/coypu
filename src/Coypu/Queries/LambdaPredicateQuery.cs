using System;

namespace Coypu.Queries
{
    public class LambdaPredicateQuery : PredicateQuery
    {
        private readonly Func<bool> query;

        public LambdaPredicateQuery(Func<bool> query)
        {
            this.query = query;
        }

        public LambdaPredicateQuery(Func<bool> query, Options options) : base(options)
        {
            this.query = query;
        }

        public override void Run()
        {
            Result = query();
        }
    }
}