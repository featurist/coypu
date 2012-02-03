namespace Coypu.Queries
{
    public class ElementExistsQuery : Query<bool>
    {
        protected readonly DriverScope driverScope;
        public bool ExpectedResult { get; private set; }
        public bool Result { get; private set; }

        public ElementExistsQuery(DriverScope driverScope)
        {
            this.driverScope = driverScope;
            ExpectedResult = true;
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
    }
}