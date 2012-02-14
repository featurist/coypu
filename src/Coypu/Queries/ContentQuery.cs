namespace Coypu.Queries
{
    internal abstract class ContentQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string text;
        public abstract bool ExpectedResult { get; }
        public bool Result { get; private set; }

        protected ContentQuery(Driver driver, DriverScope scope, string text)
        {
            this.driver = driver;
            this.scope = scope;
            this.text = text;
        }

        public void Run()
        {
            Result = driver.HasContent(text, scope) == ExpectedResult;
        }
    }
}