using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Comparers;
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
            return (IElementContainer)scope.Find().Native;
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

        public ElementCollection FindElements(string id, Scope scope)
        {
            return WatiNScope(scope).Elements.Filter(Find.ById(id) & Constraints.IsVisible(scope.ConsiderInvisibleElements));
        }

        public FrameCollection FindFrames(string locator, Scope scope, bool exact)
        {
            var byTitle = exact ? Find.ByTitle(locator) : Find.ByTitle(t => t.Contains(locator));
            var byText = exact ? Find.ByText(locator) : Find.ByText(t => t.Contains(locator));

            return WatiNDocumentScope(scope).Frames
                    .Filter(byTitle | Find.ByName(locator) | Find.ById(locator) | Constraints.HasElement("h1", byText));

        }

        public LinkCollection FindLinks(string linkText, Scope scope, bool exact)
        {
            var byLinkText = exact
                                 ? Find.ByText(linkText)
                                 : Find.ByText(t => t.Contains(linkText));

            return WatiNScope(scope).Links.Filter(byLinkText & Constraints.IsVisible(scope.ConsiderInvisibleElements));
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

        public IEnumerable<WatiN.Core.Element> FindAllCss(string cssSelector, Scope scope, Regex text = null)
        {
            return FindAllCssDeferred(cssSelector, scope, text);
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
    }
}