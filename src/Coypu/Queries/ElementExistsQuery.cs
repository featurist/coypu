namespace Coypu.Queries
{
    internal class ElementExistsQuery : DriverScopeQuery<bool>
    {
        protected internal ElementExistsQuery(DriverScope driverScope, Options options)
            : base(driverScope, options)
        {
        }

        public override object ExpectedResult => true;

        public override bool Run()
        {
            try
            {
                Scope.Stale = true;
                Scope.FindElement();
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
            catch (StaleElementException)
            {
                return false;
            }
        }
    }
}