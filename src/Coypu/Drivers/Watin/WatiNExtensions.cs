using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Drivers.Watin
{
	public static class WatiNExtensions
	{
		public static bool Displayed(this WatiN.Core.Element element)
		{
			if (string.Equals(element.Style.Display, "none"))
			{
				return false;
			}
			return element.Parent == null || Displayed(element.Parent);
		}

		public static WatiN.Core.Element FirstDisplayedOrDefault(this IEnumerable<WatiN.Core.Element> elements)
		{
			return elements.FirstOrDefault(Displayed);
		}

		public static bool AnyDisplayed(this IEnumerable<WatiN.Core.Element> elements)
		{
			return elements.Any(Displayed);
		}

		public static WatiN.Core.Element FirstDisplayedOrDefault(this IEnumerable<WatiN.Core.Element> elements, Func<WatiN.Core.Element, bool> predicate)
		{
			return elements.Where(predicate).FirstOrDefault(Displayed);
		}
	}
}