namespace Coypu.Finders
{
    internal class FieldFinder : ElementFinder
    {
        internal FieldFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override ElementFound Find()
        {
            return Driver.FindField(Locator, Scope);
        }
    }
}