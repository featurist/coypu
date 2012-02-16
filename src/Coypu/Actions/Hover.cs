using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Hover : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Hover(DriverScope driverScope)
        {
            this.driverScope = driverScope;
        }

        public void Act()
        {
            var element = driverScope.Now();
            driverScope.Hover(element);
        }
    }
}