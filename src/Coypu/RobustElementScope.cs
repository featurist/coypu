using Coypu.Robustness;

namespace Coypu
{
    internal class RobustElementScope : ElementScope
    {
        private readonly ElementFinder elementFinder;
        private readonly RobustWrapper robustWrapper;

        internal RobustElementScope(ElementFinder elementFinder, DriverScope driverScope, RobustWrapper robustWrapper)
            : base(elementFinder, driverScope, robustWrapper)
        {
            this.elementFinder = elementFinder;
            this.robustWrapper = robustWrapper;
        }

        public override Element Now()
        {
            return robustWrapper.RobustlyFind(elementFinder);
        }
    }
}