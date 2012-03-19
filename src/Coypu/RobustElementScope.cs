using System;
using Coypu.Finders;
using Coypu.Queries;

namespace Coypu
{
    public class RobustElementScope : ElementScope
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
    }
}