using System.Collections.Generic;
using System.Linq;

using WatiN.Core;
using WatiN.Core.Constraints;

namespace Coypu.Drivers.Watin
{
    internal static class Constraints
    {
        public static Constraint WithId(string id)
        {
            return Find.ById(new StringEndsWithComparer(id));
        }

        public static Constraint NotHidden()
        {
            return new ElementConstraint(new DisplayNotNoneComparer());
        }

        public static Constraint HasElement(string tagName, Constraint locator)
        {
            return new ComponentConstraint(new HasElementComparer(new[] { tagName }, locator));
        }

        public static Constraint HasElement(IEnumerable<string> tagNames, Constraint locator)
        {
            return new ComponentConstraint(new HasElementComparer(tagNames, locator));
        }

        private class StringEndsWithComparer : WatiN.Core.Comparers.Comparer<string>
        {
            private readonly string id;

            public StringEndsWithComparer(string id)
            {
                this.id = id;
            }

            public override bool Compare(string value)
            {
                return value != null && value.EndsWith(id);
            }
        }

        private class DisplayNotNoneComparer : WatiN.Core.Comparers.Comparer<WatiN.Core.Element>
        {
            public override bool Compare(WatiN.Core.Element value)
            {
                return value.Style.Display != "none";
            }
        }

        private class HasElementComparer : WatiN.Core.Comparers.Comparer<Component>
        {
            private readonly IList<ElementTag> tagNames;
            private readonly Constraint locator;

            public HasElementComparer(IEnumerable<string> tagNames, Constraint locator)
            {
                this.tagNames = (from tagName in tagNames
                                 from tag in ElementTag.ToElementTags(tagName)
                                 select tag).ToList();
                this.locator = locator;
            }

            public override bool Compare(Component component)
            {
                var container = component as IElementContainer;
                return container != null && container.ElementsWithTag(tagNames).Exists(locator);
            }
        }
    }
}