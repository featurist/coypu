using System;
using System.Threading;
using Coypu.Actions;
using Coypu.Timing;

namespace Coypu.Queries
{
    internal class ActionSatisfiesPredicateQuery : Query<bool>
    {
        private readonly BrowserAction tryThis;
        private readonly PredicateQuery until;
        private readonly TimingStrategy timingStrategy;

        public Options Options { get; private set; }

        internal ActionSatisfiesPredicateQuery(BrowserAction tryThis, PredicateQuery until, Options options, TimingStrategy timingStrategy)
        {
            this.tryThis = tryThis;
            this.until = until;
            this.timingStrategy = timingStrategy;
            Options = options;
        }

        public bool Run()
        {
            tryThis.Act();
            return timingStrategy.Synchronise(until);
        }

        public bool ExpectedResult
        {
            get { return true; }
        }
    }
}