using System;
using System.IO;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Comparers;
using WatiN.Core.Constraints;
using WatiN.Core.Interfaces;

namespace Coypu.Drivers.Watin
{
    internal class ElementFinder
    {
        private readonly WatiNDriver driver;
        private Func<IElementContainer> findScope;

        public ElementFinder(WatiNDriver driver)
        {
            this.driver = driver;
            ClearScope();
        }

        internal IElementContainer Scope
        {
            get
            {
                var f = findScope;
                ClearScope();
                var scope = f();
                SetScope(f);
                return scope;
            }
        }

        public void SetScope(Func<IElementContainer> findElementContainer)
        {
            findScope = findElementContainer;
        }

        public void ClearScope()
        {
            findScope = GetDocument;
        }

        private IElementContainer GetDocument()
        {
            return driver.Watin;
        }

        public WatiN.Core.Element FindFirst<TComponent>(IComponentCollection<TComponent> collection, Predicate<TComponent> predicate)
            where TComponent : Component
        {
            return driver.Filter(collection, predicate).FirstDisplayedOrDefault(driver.Scope);
        }

        public WatiN.Core.Element FindLink(string linkText)
        {
            return Scope.Links.First(Find.ByText(linkText) & NotHidden());
        }

        private static Constraint NotHidden()
        {
            return new ElementConstraint(new DisplayNotNoneComparer());
        }

        private class DisplayNotNoneComparer : Comparer<WatiN.Core.Element>
        {
            public override bool Compare(WatiN.Core.Element value)
            {
                return value.Style.Display != "none";
            }
        }
    }
}