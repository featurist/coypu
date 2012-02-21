namespace Coypu.Queries
{
    internal abstract class ElementPresenceQuery : Query<bool>
    {
        private readonly DriverScope driverScope;
        public abstract object ExpectedResult { get; }
        public bool Result { get; private set; }

        protected ElementPresenceQuery(DriverScope driverScope)
        {
            this.driverScope = driverScope;
        }

        public void Run()
        {
            try
            {
                driverScope.Now();
                Result = (bool) ExpectedResult;
            }
            catch (MissingHtmlException)
            {
                Result = !(bool)ExpectedResult;
            }
        }
    }
}