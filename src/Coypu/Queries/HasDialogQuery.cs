namespace Coypu.Queries
{
    internal class HasDialogQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string text;
        public override object ExpectedResult { get { return true; } }

        protected internal HasDialogQuery(Driver driver, string text, DriverScope driverScope, Options options) : base(driverScope,options)
        {
            this.driver = driver;
            this.text = text;
        }

        public override void Run()
        {
            Result = driver.HasDialog(text,DriverScope);
        }
    }
}