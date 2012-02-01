namespace Coypu.Queries
{
    public abstract class ElementQuery : Query<bool>
    {
        protected readonly DriverScope driverScope;
        public bool Result { get; private set; }

        protected ElementQuery(DriverScope driverScope)
        {
            this.driverScope = driverScope;
        }

        public void Run()
        {
            driverScope.Now();
        }

        public abstract bool ExpectingResult { get; }
    }
}