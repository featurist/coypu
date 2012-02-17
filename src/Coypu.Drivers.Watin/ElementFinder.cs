using System;
using WatiN.Core;

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
            var isButton = Constraints.OfType<Button>()
                           | Find.ByElement(e => e.TagName == "INPUT" && e.GetAttributeValue("type") == "image")
                           | Find.By("role", "button");

            var hasLocator = Find.ById(locator)
                             | Constraints.WithPartialId(locator)
                             | Find.ByName(locator)
                             | Find.ByText(locator)
                             | Find.ByValue(locator)
                             | Find.ByAlt(locator);

            var notHidden = Constraints.NotHidden();

            return Scope.Elements.First(isButton & hasLocator & notHidden);
        }

        public WatiN.Core.Element FindElement(string id)
        {
            return Scope.Elements.First(Find.ById(id) & Constraints.NotHidden());
        }

        public WatiN.Core.Element FindField(string locator)
        {
            // Find all fields -> TextFields, SelectLists, CheckBoxes, RadioButtons, FileUploads

            // Find field by label
            //   Check all labels with text locator
            //   If find label, find the field (from all fields) in scope with same id as for
            //                  or, find field (from all fields) within the scope of the label

            // Or, all fields in scope with matching id, name, value or placeholder

            var field = FindFieldByLabel(locator);
            if (field == null)
            {
                var isField = Constraints.IsField();

                var hasLocator = Find.ById(locator)
                                 | Constraints.WithPartialId(locator)
                                 | Find.ByName(locator)
                                 | Find.ByValue(locator)
                                 | Find.By("placeholder", locator);

                var notHidden = Constraints.NotHidden();

                field = Scope.Elements.First(isField & hasLocator & notHidden);
            }

            return field;
        }

        private WatiN.Core.Element FindFieldByLabel(string locator)
        {
            WatiN.Core.Element field = null;

            var label = Scope.Labels.First(Find.ByText(locator));
            if (label != null)
            {
                var notHidden = Constraints.NotHidden();

                if (!string.IsNullOrEmpty(label.For))
                    field = Scope.Elements.First(Find.ById(label.For) & notHidden);

                if (field == null)
                    field = label.Elements.First(Constraints.IsField() & notHidden);
            }
            return field;
        }

        public WatiN.Core.Element FindFieldset(string locator)
        {
            var withId = Find.ById(locator);
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

        public WatiN.Core.Element FindSection(string locator)
        {
            var isSection = Constraints.OfType(typeof(Section), typeof(Div));

            var hasLocator = Find.ById(locator)
                             | Constraints.HasElement(new[] { "h1", "h2", "h3", "h4", "h5", "h6" }, Find.ByText(locator));

            var notHidden = Constraints.NotHidden();

            return Scope.Elements.First(isSection & hasLocator & notHidden);
        }
    }
}