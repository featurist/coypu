namespace Coypu.Queries
{
    internal class HasNoContentQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string text;
        public object ExpectedResult { get { return true; } }
        public bool Result { get; private set; }

        protected internal HasNoContentQuery(Driver driver, DriverScope scope, string text)
        {
            this.driver = driver;
            this.scope = scope;
            this.text = text;
        }

        public void Run()
        {
            Result = !driver.HasContent(text, scope);
        }
    }
}