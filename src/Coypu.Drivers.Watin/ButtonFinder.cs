using System;

using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    public class ButtonFinder
    {
        private readonly ElementFinder elementFinder;
        private readonly WatiN.Core.Browser browser;

        public ButtonFinder(ElementFinder elementFinder, WatiN.Core.Browser browser)
        {
            this.elementFinder = elementFinder;
            this.browser = browser;
        }

        public WatiN.Core.Element FindButton(string locator)
        {
            var button = elementFinder.FindFirst(browser.Buttons, b => IsMatch(b, locator));
            if (button == null)
            {
                var candidates = browser.Elements.Filter(IsUnhandledButtonElement);
                button = elementFinder.FindFirst(candidates, e => IsMatch(e, locator));
            }

            return button;
        }

        private static bool IsUnhandledButtonElement(WatiN.Core.Element e)
        {
            // WatiN 2.1 doesn't include input type="image" in the list of known input types,
            // nor elements with the "button" role
            return e.GetAttributeValue("role") == "button"
                || (e.TagName == "INPUT" && e.GetAttributeValue("type") == "image");
        }

        private static bool IsMatch(WatiN.Core.Element element, string locator)
        {
            return element.HasId(locator)
                || element.HasName(locator)
                || element.HasText(locator)
                || element.HasValue(locator)
                || element.HasAltText(locator);
        }
    }
}