using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class HoverDriverAction : DriverAction
    {
        private readonly ElementFinder elementFinder;
        private readonly DriverScope driverScope;

        internal HoverDriverAction(ElementFinder elementFinder, DriverScope driverScope)
        {
            this.elementFinder = elementFinder;
            this.driverScope = driverScope;
        }

        public void Act()
        {
            var element = elementFinder.Find();
            driverScope.Hover(element);
        }
    }
}