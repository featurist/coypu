namespace Coypu.Queries
{
    internal class ElementExistsQuery : DriverScopeQuery<bool>
    {
        protected internal ElementExistsQuery(DriverScope driverScope) : base(driverScope, driverScope.ElementFinder.Options)
        {
        }

        public override bool ExpectedResult
        {
            get { return true; }
        }

        public override bool Run()
        {
            try
            {
                DriverScope.FindElement();
                return true;
            }
            catch (MissingHtmlException)
            {
                return false;
            }
            catch (MissingWindowException)
            {
                return false;
            }
        }
    }
}