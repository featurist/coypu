using System;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu
{
    public class RobustElementScope : DeferredElementScope
    {
        private readonly Options options;

        internal RobustElementScope(ElementFinder elementFinder, DriverScope outerScope, Options options)
            : base(elementFinder, outerScope)
        {
            this.options = options;
        }

        public override ElementFound Find()
        {
            return robustWrapper.Robustly(new ElementQuery(this, options));
        }

        internal override void Try(DriverAction action)
        {
            RetryUntilTimeout(action);
        }

        internal override bool Try(Query<bool> query)
        {
            return Query(query);
        }
    }
}