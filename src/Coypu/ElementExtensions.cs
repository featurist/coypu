using System;
using OpenQA.Selenium;

namespace Coypu
{
    /// <summary>
    /// Extensions to Element
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        /// Gets the outer HTML of the element
        /// </summary>
        /// <param name="element">the element</param>
        /// <returns>the outer HTML</returns>
        public static string OuterHTML(this Element element)
        {
            if (element == null) throw new ArgumentNullException("element");
            var webElement = element.Native as IWebElement;
            if (webElement == null) throw new ArgumentException("element is not an IWebElement");
            return webElement.GetAttribute("outerHTML");
        }

        /// <summary>
        /// Gets the inner HTML of the element
        /// </summary>
        /// <param name="element">the element</param>
        /// <returns>the inner HTML</returns>
        public static string InnerHTML(this Element element)
        {
            if (element == null) throw new ArgumentNullException("element");
            var webElement = element.Native as IWebElement;
            if (webElement == null) throw new ArgumentException("element is not an IWebElement");
            return webElement.GetAttribute("innerHTML");
        }
    }
}