namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected readonly Driver Driver;
        private readonly string _locator;
        protected readonly DriverScope Scope;

        protected ElementFinder(Driver driver, string locator, DriverScope scope)
        {
            Driver = driver;
            _locator = locator;
            Scope = scope;
        }

        internal string Locator { get { return _locator; } }
        internal abstract Element Find();
    }
}