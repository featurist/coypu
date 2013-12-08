using System;
using System.Linq;
using Coypu.Queries;
using Coypu.Timing;

namespace Coypu.Finders
{
    internal class StateFinder
    {
        private readonly TimingStrategy timingStrategy;

        public StateFinder(TimingStrategy timingStrategy)
        {
            this.timingStrategy = timingStrategy;
        }

        internal State FindState(State[] states, Scope scope, Options options)
        {
            var query = new LambdaPredicateQuery(() =>
            {
                var was = timingStrategy.ZeroTimeout;
                timingStrategy.ZeroTimeout = true;
                try
                {
                    return ((Func<bool>)(() => states.Any(s => s.CheckCondition())))();
                }
                finally
                {
                    timingStrategy.ZeroTimeout = was;
                }
            }, options);

            var foundState = timingStrategy.Synchronise(query);

            if (!foundState)
                throw new MissingHtmlException("None of the given states was reached within the configured timeout.");

            return states.First(e => e.ConditionWasMet);
        }
    }
}