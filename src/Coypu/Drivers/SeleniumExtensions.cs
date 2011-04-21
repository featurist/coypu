using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers
{
	public static class SeleniumExtensions
	{
		public static bool IsVisible(this IWebElement webElement)
		{
			var renderedElement = webElement as IRenderedWebElement;
			return renderedElement != null && renderedElement.Displayed;
		}
		public static IWebElement FirstVisibleOrDefault(this IEnumerable<IWebElement> elements)
		{
			return elements.FirstOrDefault(IsVisible);
		}

		public static IWebElement FirstVisibleOrDefault(this IEnumerable<IWebElement> elements, Func<IWebElement, bool> predicate)
		{
			return elements.Where(IsVisible).FirstOrDefault(predicate);
		}
	}
}