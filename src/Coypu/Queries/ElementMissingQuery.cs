namespace Coypu.Queries
{
    public class ElementMissingQuery : Query<bool>
    {
        protected readonly DriverScope driverScope;
        public bool ExpectedResult { get; private set; }
        public bool Result { get; private set; }

        public ElementMissingQuery(DriverScope driverScope)
        {
            this.driverScope = driverScope;
            ExpectedResult = true;
        }

        public void Run()
        {
            try
            {
                driverScope.Now();
                Result = false;
            }
            catch (MissingHtmlException)
            {
                Result = true;
            }
        }
    }
}