namespace Coypu.Queries
{
    internal abstract class ElementPresenceQuery : Query<bool>
    {
        private readonly DriverScope driverScope;
        public abstract bool ExpectedResult { get; }
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
                Result = ExpectedResult;
            }
            catch (MissingHtmlException)
            {
                Result = !ExpectedResult;
            }
        }
    }
}