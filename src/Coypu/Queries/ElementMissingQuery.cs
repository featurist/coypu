namespace Coypu.Queries
{
    internal class ElementMissingQuery : DriverScopeQuery<bool>
    {
        public override object ExpectedResult
        {
            get { return true; }
        }

        protected internal ElementMissingQuery(DriverScope driverScope, Options options)
            : base(driverScope, options)
        {
        }

        public override void Run()
        {
            try
            {
                DriverScope.Now();
                Result = false;
            }
            catch (MissingHtmlException)
            {
                Result = true;
            }

        }
    }
}