using System;
using System.Linq;
using Coypu.Robustness;

namespace Coypu
{
    internal class StateFinder
    {
        private readonly RobustWrapper robustWrapper;
        private readonly TemporaryTimeouts temporaryTimeouts;

        public StateFinder(RobustWrapper robustWrapper, TemporaryTimeouts temporaryTimeouts)
        {
            this.robustWrapper = robustWrapper;
            this.temporaryTimeouts = temporaryTimeouts;
        }

        internal State FindState(params State[] states)
        {
            var foundState = robustWrapper.Query(() => temporaryTimeouts.WithIndividualTimeout(TimeSpan.Zero,() => states.Any(s => s.CheckCondition())), true);
            
            if (!foundState)
                throw new MissingHtmlException("None of the given states was reached within the configured timeout.");

            return states.First(e => e.ConditionWasMet);
        }
    }
}