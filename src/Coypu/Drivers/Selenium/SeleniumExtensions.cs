using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal static class SeleniumExtensions
    {
        internal static bool IsDisplayed(this IWebElement webElement)
        {
            return webElement.Displayed;
        }

        internal static IWebElement FirstDisplayedOrDefault(this IEnumerable<IWebElement> elements)
        {
            return elements.FirstOrDefault(IsDisplayed);
        }

        internal static bool AnyDisplayed(this IEnumerable<IWebElement> elements)
        {
            return elements.Any(IsDisplayed);
        }

        internal static bool AnyDisplayed(this IEnumerable<IWebElement> elements,
                                          Func<IWebElement, bool> predicate)
        {
            return elements.Any(e => predicate(e) && IsDisplayed(e));
        }

        internal static IWebElement FirstDisplayedOrDefault(this IEnumerable<IWebElement> elements,
                                                            Func<IWebElement, bool> predicate)
        {
            return elements.FirstOrDefault(e => predicate(e) && IsDisplayed(e));
        }
    }
}