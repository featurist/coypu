using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class RobustElementScope : ElementScope
    {
        private readonly ElementFinder elementFinder;
        private readonly RobustWrapper robustWrapper;

        internal RobustElementScope(ElementFinder elementFinder, DriverScope outerScope, RobustWrapper robustWrapper)
            : base(elementFinder, outerScope, robustWrapper)
        {
            this.elementFinder = elementFinder;
            this.robustWrapper = robustWrapper;
        }   

        public override Element Now()
        {
            return robustWrapper.Robustly(new ElementQuery(elementFinder, DriverScope.Timeout));
        }
    }
}