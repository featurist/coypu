namespace Coypu.Queries
{
    internal class ElementMissingQuery : DriverScopeQuery<bool>
    {
        protected internal ElementMissingQuery(DriverScope driverScope, Options options)
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
            catch (StaleElementException)
            {
                return true;
            }
        }
    }
}