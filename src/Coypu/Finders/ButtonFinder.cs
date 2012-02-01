namespace Coypu.Finders
{
    internal class ButtonFinder : ElementFinder
    {
        internal ButtonFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindButton(Locator, Scope);
        }
    }
}