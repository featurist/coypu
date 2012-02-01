using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class ClickDriverAction : DriverAction
    {
        private readonly ElementFinder elementFinder;
        private readonly DriverScope driverScope;


        internal ClickDriverAction(ElementFinder elementFinder, DriverScope driverScope)
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