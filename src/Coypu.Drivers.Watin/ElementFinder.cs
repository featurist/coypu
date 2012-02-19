using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public bool ConsiderInvisibleElements { get; set; }

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

            var byText = Find.ByText(locator);
            var byIdNameValueOrAlt = Find.ById(locator)
                                     | Find.ByName(locator)
                                     | Find.ByValue(locator)
                                     | Find.ByAlt(locator);
            var byPartialId = Constraints.WithPartialId(locator);
            var hasLocator = byText | byIdNameValueOrAlt | byPartialId;

            var isVisible = Constraints.IsVisible(ConsiderInvisibleElements);

            var candidates = Scope.Elements.Filter(isButton & hasLocator & isVisible);
            return candidates.FirstMatching(byText, byIdNameValueOrAlt, byPartialId);
        }

        public WatiN.Core.Element FindElement(string id)
        {
            return Scope.Elements.First(Find.ById(id) & Constraints.IsVisible(ConsiderInvisibleElements));
        }

        public WatiN.Core.Element FindField(string locator)
        {
            var field = FindFieldByLabel(locator);
            if (field == null)
            {
                var isField = Constraints.IsField();

                var byIdOrName = Find.ById(locator) | Find.ByName(locator);
                var byPlaceholder = Find.By("placeholder", locator);
                var radioButtonByValue = Constraints.OfType<RadioButton>() & Find.ByValue(locator);
                var byPartialId = Constraints.WithPartialId(locator);

                var hasLocator = byIdOrName | byPlaceholder | radioButtonByValue | byPartialId;

                var isVisible = Constraints.IsVisible(ConsiderInvisibleElements);

                var candidates = Scope.Elements.Filter(isField & hasLocator & isVisible);
                field = candidates.FirstMatching(byIdOrName, byPlaceholder, radioButtonByValue, byPartialId);
            }

            return field;
        }

        private WatiN.Core.Element FindFieldByLabel(string locator)
        {
            WatiN.Core.Element field = null;

            var label = Scope.Labels.First(Find.ByText(new Regex(locator)));
            if (label != null)
            {
                var isVisible = Constraints.IsVisible(ConsiderInvisibleElements);

                if (!string.IsNullOrEmpty(label.For))
                    field = Scope.Elements.First(Find.ById(label.For) & isVisible);

                if (field == null)
                    field = label.Elements.First(Constraints.IsField() & isVisible);
            }
            return field;
        }

        public WatiN.Core.Element FindFieldset(string locator)
        {
            var withId = Find.ById(locator);
            var withLegend = Constraints.HasElement("legend", Find.ByText(locator));
            var hasLocator = withId | withLegend;

            var isVisible = Constraints.IsVisible(ConsiderInvisibleElements);

            return Scope.Fieldsets().First(hasLocator & isVisible);
        }

        public Frame FindFrame(string locator)
        {
            return GetDocument().Frames.First(Find.ByTitle(locator) | Find.ById(locator) | Constraints.HasElement("h1", Find.ByText(locator)));
        }

        public WatiN.Core.Element FindLink(string linkText)
        {
            return Scope.Links.First(Find.ByText(linkText) & Constraints.IsVisible(ConsiderInvisibleElements));
        }

        public WatiN.Core.Element FindSection(string locator)
        {
            var isSection = Constraints.OfType(typeof(Section), typeof(Div));

            var hasLocator = Find.ById(locator)
                             | Constraints.HasElement(new[] { "h1", "h2", "h3", "h4", "h5", "h6" }, Find.ByText(locator));

            var isVisible = Constraints.IsVisible(ConsiderInvisibleElements);

            return Scope.Elements.First(isSection & hasLocator & isVisible);
        }

        public IEnumerable<WatiN.Core.Element> FindAllCss(string cssSelector)
        {
            // TODO: This is restricting by hidden items, but there are no tests for that!
            var isVisible = Constraints.IsVisible(ConsiderInvisibleElements);
            return (from element in GetDocument().Elements.Filter(Find.BySelector(cssSelector))
                    where element.Matches(isVisible)
                    select element).ToList();
        }

        public WatiN.Core.Element FindCss(string cssSelector)
        {
            return GetDocument().Elements.Filter(Find.BySelector(cssSelector)).First(Constraints.IsVisible(ConsiderInvisibleElements));
        }

        public bool HasCss(string cssSelector)
        {
            var element = FindCss(cssSelector);
            return element != null && element.Exists;
        }
    }
}