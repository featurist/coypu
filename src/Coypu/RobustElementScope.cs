using System;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class RobustElementScope : ElementScope
    {
        private readonly Options options;

        internal RobustElementScope(ElementFinder elementFinder, DriverScope outerScope, RobustWrapper robustWrapper, Options options)
            : base(elementFinder, outerScope, robustWrapper)
        {
            this.options = options;
        }

        public override ElementFound Now()
        {
            return robustWrapper.Robustly(new ElementQuery(this, options));
        }
    }
}