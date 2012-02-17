using System;
using WatiN.Core;
using WatiN.Core.Comparers;

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

        private Document GetDocument()
        {
            return driver.Watin;
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

        public WatiN.Core.Element FindButton(string locator)
        {
            var isButton = Find.ByElement(new TypeComparer(typeof (Button)))
                           | Find.ByElement(e => e.TagName == "INPUT" && e.GetAttributeValue("type") == "image")
                           | Find.By("role", "button");

            var hasLocator = Constraints.WithId(locator)
                             | Find.ByName(locator)
                             | Find.ByText(locator)
                             | Find.ByValue(locator)
                             | Find.ByAlt(locator);

            var notHidden = Constraints.NotHidden();

            return Scope.Elements.First(isButton & hasLocator & notHidden);
        }

        public WatiN.Core.Element FindElement(string id)
        {
            return Scope.Elements.First(Constraints.WithId(id) & Constraints.NotHidden());
        }

        public WatiN.Core.Element FindFieldset(string locator)
        {
            var withId = Constraints.WithId(locator);
            var withLegend = Constraints.HasElement("legend", Find.ByText(locator));
            var hasLocator = withId | withLegend;

            var notHidden = Constraints.NotHidden();

            return Scope.Fieldsets().First(hasLocator & notHidden);
        }

        public Frame FindFrame(string locator)
        {
            return GetDocument().Frames.First(Find.ByTitle(locator) | Find.ById(locator) | Constraints.HasElement("h1", Find.ByText(locator)));
        }

        public WatiN.Core.Element FindLink(string linkText)
        {
            return Scope.Links.First(Find.ByText(linkText) & Constraints.NotHidden());
        }
    }
}