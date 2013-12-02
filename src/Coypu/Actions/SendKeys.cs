namespace Coypu.Actions
{
    internal class SendKeys : DriverAction
    {
        private readonly string keys;
        private readonly DriverScope driverScope;

        internal SendKeys(string keys, DriverScope driverScope, Driver driver, Options options)
            : base(driver, options)
        {
            this.keys = keys;
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Find();
            Driver.SendKeys(element,keys);
        }
    }
}