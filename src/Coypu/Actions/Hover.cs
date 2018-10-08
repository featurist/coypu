namespace Coypu.Actions
{
    internal class Hover : DriverAction
    {
        private readonly DriverScope _driverScope;

        internal Hover(DriverScope driverScope,
                       IDriver driver,
                       Options options) : base(driver, driverScope, options)
        {
            _driverScope = driverScope;
        }

        public override void Act()
        {
            var element = _driverScope.Now();
            Driver.Hover(element);
        }
    }
}