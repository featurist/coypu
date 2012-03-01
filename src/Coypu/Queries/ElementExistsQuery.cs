namespace Coypu.Queries
{
    internal class ElementExistsQuery : DriverScopeQuery<bool>
    {
        public override object ExpectedResult
        {
            get { return true; }
        }

        protected internal ElementExistsQuery(DriverScope driverScope) : base(driverScope)
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