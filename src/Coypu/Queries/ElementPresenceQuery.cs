namespace Coypu.Queries
{
    public abstract class ElementPresenceQuery : Query<bool>
    {
        protected readonly DriverScope driverScope;
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
                Result = true;
            }
            catch (MissingHtmlException)
            {
                Result = false;
            }
        }

        public bool ExpectedOutcomeFound
        {
            get { return Result == ExpectedResult; }
        }
    }
}