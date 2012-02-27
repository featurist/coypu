namespace Coypu.Actions
{
    internal class Click : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Click(DriverScope driverScope) : base(null,driverScope.Timeout)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Now();
            driverScope.Click(element);
        }
    }
}