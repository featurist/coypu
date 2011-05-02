using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.WatiNDriver
{
	public static class WatiNExtensions
	{
		public static bool Displayed(this object webElement)
		{
			throw new NotImplementedException();
//			var renderedElement = webElement as IRenderedWebElement;
//			return renderedElement != null && renderedElement.Displayed;
		}

	}
}