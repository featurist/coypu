using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Constraints;

namespace Coypu.Drivers.Watin
{
    internal class ElementFinder
    {
        private XPath xPath;

        internal ElementFinder()
        {
            xPath = new XPath();
        }

        internal static IElementContainer WatiNScope(Scope scope)
        {
            return (IElementContainer)scope.Now().Native;
        }

        private static Document WatiNDocumentScope(Scope scope)
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

        public WatiN.Core.Element FindButton(string locator, Scope scope)
        {
            return xPath.ButtonXPathsByPrecedence(locator, scope.Options)
                    .Select(xpath => FindXPath(xpath, scope))
                    .FirstOrDefault(element => element != null);
        }

        public WatiN.Core.Element FindElement(string id, Scope scope)
        {
            return WatiNScope(scope).Elements.First(Find.ById(id) & Constraints.IsVisible(scope.ConsiderInvisibleElements));
        }

        public WatiN.Core.Element FindField(string locator, Scope scope)
        {
            return xPath.FieldXPathsByPrecedence(locator, scope)
                    .Select(xpath => FindXPath(xpath, scope))
                    .FirstOrDefault(element => element != null);
        }
        
        public WatiN.Core.Element FindFieldset(string locator, Scope scope)
        {
            var withId = Find.ById(locator);
            var withLegend = Constraints.HasElement("legend", Find.ByText(locator));
            var hasLocator = withId | withLegend;

            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

            return WatiNScope(scope).Fieldsets().First(hasLocator & isVisible);
        }

        public Frame FindFrame(string locator, Scope scope)
        {
            return WatiNDocumentScope(scope).Frames.First(Find.ByTitle(locator) | Find.ByName(locator) | Find.ById(locator) | Constraints.HasElement("h1", Find.ByText(locator)));
        }

        public WatiN.Core.Element FindLink(string linkText, Scope scope)
        {
            return WatiNScope(scope).Links.First(Find.ByText(linkText) & Constraints.IsVisible(scope.ConsiderInvisibleElements));
        }

        public WatiN.Core.Element FindSection(string locator, Scope scope)
        {
            var isSection = Constraints.OfType(typeof(Section), typeof(Div));

            var hasLocator = Find.ById(locator)
                             | Constraints.HasElement(new[] { "h1", "h2", "h3", "h4", "h5", "h6" }, Find.ByText(locator));

            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);

            return WatiNScope(scope).Elements.First(isSection & hasLocator & isVisible);
        }

        private IEnumerable<WatiN.Core.Element> FindAllCssDeferred(string cssSelector, Scope scope, Regex textPattern = null)
        {
            // TODO: This is restricting by hidden items, but there are no tests for that!
            var whereConstraints = Constraints.IsVisible(scope.ConsiderInvisibleElements);
            Constraint querySelectorConstraint = Find.BySelector(cssSelector);

            if (textPattern != null)
                whereConstraints = whereConstraints & Find.ByText(textPattern);

            return from element in WatiNScope(scope).Elements.Filter(querySelectorConstraint)
                   where element.Matches(whereConstraints)
                   select element;
        }

        public IEnumerable<WatiN.Core.Element> FindAllCss(string cssSelector, Scope scope)
        {
            return FindAllCssDeferred(cssSelector, scope);
        }

        public WatiN.Core.Element FindCss(string cssSelector, Scope scope, Regex textPattern = null)
        {
            return FindAllCssDeferred(cssSelector, scope, textPattern).FirstOrDefault();
        }

        public bool HasCss(string cssSelector, Scope scope)
        {
            var element = FindCss(cssSelector, scope);
            return element != null && element.Exists;
        }

        private IEnumerable<WatiN.Core.Element> FindAllXPathDeferred(string xpath, Scope scope)
        {
            var isVisible = Constraints.IsVisible(scope.ConsiderInvisibleElements);
            return from element in WatiNScope(scope).XPath(xpath)
                   where element.Matches(isVisible)
                   select element;
        }

        public IEnumerable<WatiN.Core.Element> FindAllXPath(string xpath, Scope scope)
        {
            return FindAllXPathDeferred(xpath, scope);
        }

        public WatiN.Core.Element FindXPath(string xpath, Scope scope)
        {
            return FindAllXPathDeferred(xpath, scope).FirstOrDefault();
        }

        public bool HasXPath(string xpath, Scope scope)
        {
            var element = FindXPath(xpath, scope);
            return element != null && element.Exists;
        }
    }
}