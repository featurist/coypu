namespace Coypu.Queries
{
    internal class ElementExistsQuery : DriverScopeQuery<bool>
    {
        public override bool ExpectedResult
        {
            get { return true; }
        }

        protected internal ElementExistsQuery(DriverScope driverScope, Options options) : base(driverScope,options)
        {
        }

        public override bool Run()
        {
            try
            {
                DriverScope.Find();
                return true;
            }
            catch (MissingHtmlException)
            {
                return false;
            }
        }
    }
}