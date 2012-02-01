namespace Coypu.Finders
{
    internal class FieldsetFinder : ElementFinder
    {
        internal FieldsetFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindFieldset(Locator, Scope);
        }
    }
}