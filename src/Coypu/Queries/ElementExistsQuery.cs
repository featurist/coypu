namespace Coypu.Queries
{
    internal class ElementExistsQuery : DriverScopeQuery<bool>
    {
        public override object ExpectedResult
        {
            get { return true; }
        }

        protected internal ElementExistsQuery(DriverScope driverScope, Options options) : base(driverScope,options)
        {
        }

        public override void Run()
        {
            try
            {
                DriverScope.Now();
                Result = true;
            }
            catch (MissingHtmlException)
            {
                Result = false;
            }
        }
    }
}