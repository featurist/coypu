using WatiN.Core;
using WatiN.Core.Comparers;
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
            return new ComponentConstraint(new HasElementComparer(tagName, locator));
        }

        private class StringEndsWithComparer : Comparer<string>
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

        private class DisplayNotNoneComparer : Comparer<WatiN.Core.Element>
        {
            public override bool Compare(WatiN.Core.Element value)
            {
                return value.Style.Display != "none";
            }
        }

        private class HasElementComparer : Comparer<Component>
        {
            private readonly string tagName;
            private readonly Constraint locator;

            public HasElementComparer(string tagName, Constraint locator)
            {
                this.tagName = tagName;
                this.locator = locator;
            }

            public override bool Compare(Component component)
            {
                var container = component as IElementContainer;
                return container != null && container.ElementWithTag(tagName, locator).Exists;
            }
        }
    }
}