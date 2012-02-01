using Coypu.Finders;

namespace Coypu
{
    internal class CssFinder : ElementFinder
    {
        internal CssFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindCss(Locator, Scope);
        }
    }
}