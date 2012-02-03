using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu
{
    internal class RobustElementScope : ElementScope
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
            elementFinder.Timeout = DriverScope.IndividualTimeout;
            return robustWrapper.RobustlyFind(elementFinder);
        }
    }
}