namespace Coypu.Queries
{
    internal class ElementMissingQuery : DriverScopeQuery<bool>
    {
        protected internal ElementMissingQuery(DriverScope driverScope)
            : base(driverScope, driverScope.ElementFinder.Options)
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
                return false;
            }
            catch (MissingHtmlException)
            {
                return true;
            }
            catch (MissingWindowException)
            {
                return true;
            }
        }
    }
}