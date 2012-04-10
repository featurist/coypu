using System;

namespace Coypu.Queries
{
    public abstract class PredicateQuery : Query<bool>
    {

        protected PredicateQuery()
        {
        }

        protected PredicateQuery(Options options)
        {
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public abstract bool Predicate();

        public bool Run()
        {
            return Predicate();
        }

        public bool ExpectedResult 
        {
            get { return true; }
        }

        public TimeSpan Timeout { get; private set; }

        public TimeSpan RetryInterval { get; private set; }
    }
}