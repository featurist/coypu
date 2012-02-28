using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    internal class ElementFinder
    {
        internal static IElementContainer WatiNScope(DriverScope scope)
        {
            return (IElementContainer)scope.Now().Native;
        }

        private static Document WatiNDocumentScope(DriverScope scope)
        {
            var nativeScope = WatiNScope(scope);
            while (!(nativeScope is Document) && nativeScope is WatiN.Core.Element)
            {
                nativeScope = ((WatiN.Core.Element) nativeScope).DomContainer;
            }

            var documentScope = nativeScope as Document;
            if (documentScope == null)
                throw new InvalidOperationException("Cannot find frame from a scope that isn't a document or a frame");
            return documentScope;
        }

        public WatiN.Core.Element FindButton(string locator, DriverScope scope)
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

            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

            var candidates = WatiNScope(scope).Elements.Filter(isButton & hasLocator & isVisible);
            return candidates.FirstMatching(byText, byIdNameValueOrAlt, byPartialId);
        }

        public WatiN.Core.Element FindElement(string id, DriverScope scope)
        {
            return WatiNScope(scope).Elements.First(Find.ById(id) & Constraints.IsVisible(scope.ConsiderInvisibleElements));
        }

        public WatiN.Core.Element FindField(string locator, DriverScope scope)
        {
            var field = FindFieldByLabel(locator,scope);
            if (field == null)
            {
                var isField = Constraints.IsField();

                var byIdOrName = Find.ById(locator) | Find.ByName(locator);
                var byPlaceholder = Find.By("placeholder", locator);
                var radioButtonByValue = Constraints.OfType<RadioButton>() & Find.ByValue(locator);
                var byPartialId = Constraints.WithPartialId(locator);

                var hasLocator = byIdOrName | byPlaceholder | radioButtonByValue | byPartialId;

                var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

                var candidates = WatiNScope(scope).Elements.Filter(isField & hasLocator & isVisible);
                field = candidates.FirstMatching(byIdOrName, byPlaceholder, radioButtonByValue, byPartialId);
            }

            return field;
        }

        private WatiN.Core.Element FindFieldByLabel(string locator, DriverScope scope)
        {
            WatiN.Core.Element field = null;

            var label = WatiNScope(scope).Labels.First(Find.ByText(new Regex(locator)));
            if (label != null)
            {
                var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

                if (!string.IsNullOrEmpty(label.For))
                    field = WatiNScope(scope).Elements.First(Find.ById(label.For) & isVisible);

                if (field == null)
                    field = label.Elements.First(Constraints.IsField() & isVisible);
            }
            return field;
        }

        public WatiN.Core.Element FindFieldset(string locator, DriverScope scope)
        {
            var withId = Find.ById(locator);
            var withLegend = Constraints.HasElement("legend", Find.ByText(locator));
            var hasLocator = withId | withLegend;

            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

            return WatiNScope(scope).Fieldsets().First(hasLocator & isVisible);
        }

        public Frame FindFrame(string locator, DriverScope scope)
        {
            return WatiNDocumentScope(scope).Frames.First(Find.ByTitle(locator) | Find.ById(locator) | Constraints.HasElement("h1", Find.ByText(locator)));
        }

        public WatiN.Core.Element FindLink(string linkText, DriverScope scope)
        {
            return WatiNScope(scope).Links.First(Find.ByText(linkText) & Constraints.IsVisible(scope.ConsiderInvisibleElements));
        }

        public WatiN.Core.Element FindSection(string locator, DriverScope scope)
        {
            var isSection = Constraints.OfType(typeof(Section), typeof(Div));

            var hasLocator = Find.ById(locator)
                             | Constraints.HasElement(new[] { "h1", "h2", "h3", "h4", "h5", "h6" }, Find.ByText(locator));

            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

            return WatiNScope(scope).Elements.First(isSection & hasLocator & isVisible);
        }

        private IEnumerable<WatiN.Core.Element> FindAllCssDeferred(string cssSelector, DriverScope scope)
        {
            // TODO: This is restricting by hidden items, but there are no tests for that!
            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);
            return from element in WatiNScope(scope).Elements.Filter(Find.BySelector(cssSelector))
                   where element.Matches(isVisible)
                   select element;
        }

        public IEnumerable<WatiN.Core.Element> FindAllCss(string cssSelector, DriverScope scope)
        {
            return FindAllCssDeferred(cssSelector, scope).ToList();
        }

        public WatiN.Core.Element FindCss(string cssSelector, DriverScope scope)
        {
            return FindAllCssDeferred(cssSelector, scope).FirstOrDefault();
        }

        public bool HasCss(string cssSelector, DriverScope scope)
        {
            var element = FindCss(cssSelector, scope);
            return element != null && element.Exists;
        }
    }
}