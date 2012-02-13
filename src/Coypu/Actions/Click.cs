using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Click : DriverAction
    {
        private readonly ElementFinder elementFinder;
        private readonly DriverScope driverScope;


        internal Click(ElementFinder elementFinder, DriverScope driverScope)
        {
            this.elementFinder = elementFinder;
            this.driverScope = driverScope;
        }

        public void Act()
        {
            var element = elementFinder.Find();
            driverScope.Click(element);
        }
    }
}