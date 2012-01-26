using Coypu.Drivers.Selenium;

namespace Coypu
{
    internal class LinkFinder : ElementFinder
    {
        internal LinkFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindLink(Locator, Scope);
        }
    }

    internal class ButtonFinder : ElementFinder
    {
        internal ButtonFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindButton(Locator, Scope);
        }
    }

    internal class FieldFinder : ElementFinder
    {
        internal FieldFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindField(Locator, Scope);
        }
    }

    internal class CssFinder : ElementFinder
    {
        internal CssFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindCss(Locator, Scope);
        }
    }

    internal class XPathFinder : ElementFinder
    {
        internal XPathFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindXPath(Locator, Scope);
        }
    }

    internal class SectionFinder : ElementFinder
    {
        internal SectionFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindSection(Locator, Scope);
        }
    }

    internal class FieldsetFinder : ElementFinder
    {
        internal FieldsetFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindFieldset(Locator, Scope);
        }
    }

    internal class IdFinder : ElementFinder
    {
        internal IdFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        internal override Element Find()
        {
            return Driver.FindId(Locator, Scope);
        }
    }
}