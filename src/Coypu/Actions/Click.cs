using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Click : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Click(DriverScope driverScope)
        {
            this.driverScope = driverScope;
        }

        public void Act()
        {
            var element = driverScope.Now();
            driverScope.Click(element);
        }
    }
}