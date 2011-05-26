using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    public static class SeleniumExtensions
    {
        public static bool IsDisplayed(this IWebElement webElement)
        {
            var renderedElement = webElement as IRenderedWebElement;
            return renderedElement != null && renderedElement.Displayed;
        }

        public static IWebElement FirstDisplayedOrDefault(this IEnumerable<IWebElement> elements)
        {
            return elements.FirstOrDefault(IsDisplayed);
        }

        public static bool AnyDisplayed(this IEnumerable<IWebElement> elements)
        {
            return elements.Any(IsDisplayed);
        }
        public static bool AnyDisplayed(this IEnumerable<IWebElement> elements, Func<IWebElement, bool> predicate)
        {
            return elements.Any(e => predicate(e) && IsDisplayed(e));
        }

        public static IWebElement FirstDisplayedOrDefault(this IEnumerable<IWebElement> elements, Func<IWebElement, bool> predicate)
        {
            return elements.FirstOrDefault(e => predicate(e) && IsDisplayed(e));
        }
    }
}