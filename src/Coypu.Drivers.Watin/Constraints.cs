using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Comparers;
using WatiN.Core.Constraints;

namespace Coypu.Drivers.Watin
{
    internal static class Constraints
    {
        public static Constraint IsVisible(bool considerInvisibleElements)
        {
            return considerInvisibleElements ? (Constraint) Find.Any : new ElementConstraint(new IsVisibleComparer());
        }

        public static Constraint HasElement(string tagName, Constraint locator)
        {
            return new ComponentConstraint(new HasElementComparer(new[] { tagName }, locator));
        }

        public static Constraint HasElement(IEnumerable<string> tagNames, Constraint locator)
        {
            return new ComponentConstraint(new HasElementComparer(tagNames, locator));
        }

        public static Constraint OfType<T>()
            where T : WatiN.Core.Element
        {
            return new ElementConstraint(new TypeComparer(typeof(T)));
        }

        public static Constraint OfType(params Type[] types)
        {
            return new ElementConstraint(new TypesComparer(types));
        }

        public static Constraint IsField()
        {
            return OfType(typeof (TextField), typeof (SelectList), typeof (CheckBox), typeof (RadioButton), typeof (FileUpload))
                   | new AttributeConstraint("type", "email") | new AttributeConstraint("type", "tel") | new AttributeConstraint("type", "url");
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
                return value != null && id != null && value.EndsWith("_" + id);
            }
        }

        private class IsVisibleComparer : WatiN.Core.Comparers.Comparer<WatiN.Core.Element>
        {
            public override bool Compare(WatiN.Core.Element element)
            {
                return element.Style.Display != "none" && element.Style.GetAttributeValue("visibility") != "hidden";
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

        private class AttrbiuteComparer : WatiN.Core.Comparers.Comparer<Component>
        {
            private readonly string attributeName;
            private readonly string attributeValue;

            public AttrbiuteComparer(string attributeName, string attributeValue)
            {
                this.attributeName = attributeName;
                this.attributeValue = attributeValue;
            }

            public override bool Compare(Component component)
            {
                var element = component as Element;
                return element != null && element[attributeName] == attributeValue;
            }
        }

        private class TypesComparer : WatiN.Core.Comparers.Comparer<WatiN.Core.Element>
        {
            private readonly Type[] types;

            public TypesComparer(params Type[] types)
            {
                this.types = types;
            }

            public override bool Compare(WatiN.Core.Element element)
            {
                return types.Contains(element.GetType());
            }
        }
    }
}