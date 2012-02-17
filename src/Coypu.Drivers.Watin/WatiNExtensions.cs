using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Drivers.Watin
{
    internal static class WatiNExtensions
    {
        private static bool IsDisplayed(this WatiN.Core.Element element)
        {
            if (string.Equals(element.Style.Display, "none"))
            {
                return false;
            }
            return element.Parent == null || IsDisplayed(element.Parent);
        }

        internal static WatiN.Core.Element FirstDisplayedOrDefault(this IEnumerable<WatiN.Core.Element> elements)
        {
            return elements.FirstOrDefault(IsDisplayed);
        }

        internal static WatiN.Core.Element FirstWithinScopeOrDefault(this IEnumerable<WatiN.Core.Element> elements, WatiN.Core.Element scope)
        {
            return elements.FirstOrDefault(e => IsWithinScope(e, scope));
        }

        internal static WatiN.Core.Element FirstDisplayedOrDefault(this IEnumerable<WatiN.Core.Element> elements, WatiN.Core.Element scope)
        {
            return elements.FirstOrDefault(e => IsDisplayed(e) && IsWithinScope(e, scope));
        }

        internal static WatiN.Core.Element FirstDisplayedOrDefault(this IEnumerable<WatiN.Core.Element> elements, WatiN.Core.Element scope, Func<WatiN.Core.Element, bool> predicate)
        {
            return elements.FirstOrDefault(e => predicate(e) && IsDisplayed(e) && IsWithinScope(e, scope));
        }

        internal static IEnumerable<WatiN.Core.Element> WithinScope(this IEnumerable<WatiN.Core.Element> elements, WatiN.Core.Element scope)
        {
            return elements.Where(e => IsWithinScope(e, scope));
        }

        private static bool IsWithinScope(WatiN.Core.Element element, WatiN.Core.Element scope)
        {
            if (scope == null)
                return true;

            var parent = element.Parent;
            while (parent != null)
            {
                if (parent.Equals(scope))
                    return true;

                parent = parent.Parent;
            }
            return false;
        }

        internal static WatiN.Core.Element FirstDisplayedOrDefault(this IEnumerable<WatiN.Core.Element> elements, Func<WatiN.Core.Element, bool> predicate)
        {
            return elements.Where(predicate).FirstOrDefault(IsDisplayed);
        }
    }
}