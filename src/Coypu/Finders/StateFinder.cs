using System;
using System.Linq;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Finders
{
    internal class StateFinder
    {
        private readonly RobustWrapper robustWrapper;

        public StateFinder(RobustWrapper robustWrapper)
        {
            this.robustWrapper = robustWrapper;
        }

        internal State FindState(params State[] states)
        {
            var query = new LambdaQuery<bool>(() => {
                                                        var defaultTimeout = Configuration.Timeout;
                                                        Configuration.Timeout = TimeSpan.Zero;
                                                        try
                                                        {
                                                            return ((Func<bool>)(() => states.Any(s => s.CheckCondition())))();
                                                        }
                                                        finally
                                                        {
                                                            Configuration.Timeout = defaultTimeout;
                                                        }
            },true);
            var foundState = robustWrapper.Robustly(query);
            
            if (!foundState)
                throw new MissingHtmlException("None of the given states was reached within the configured timeout.");

            return states.First(e => e.ConditionWasMet);
        }
    }
}