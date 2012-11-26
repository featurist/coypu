using System;
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

        public override ElementFound Now()
        {
            return robustWrapper.Robustly(new ElementQuery(this, options));
        }

        public override ElementScope Click(Options options = null)
        {
            RetryUntilTimeout(ClickAction(options));
            return this;
        }

        public override ElementScope Hover(Options options = null)
        {
            RetryUntilTimeout(HoverAction(options));
            return this;
        }

        public override ElementScope SendKeys(string keys, Options options = null)
        {
            RetryUntilTimeout(SendKeysAction(keys, options));
            return this;
        }

        public override bool Exists(Options options = null)
        {
            return robustWrapper.Robustly(ExistsQuery(options));
        }

        public override bool Missing(Options options = null)
        {
            return robustWrapper.Robustly(MissingQuery(options));
        }
    }
}