namespace Coypu.Actions
{
    internal class Hover : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Hover(DriverScope driverScope) : base(null, driverScope.IndividualTimeout)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Now();
            driverScope.Hover(element);
        }
    }
}