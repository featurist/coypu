using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Coypu.Tests.TestDoubles
{
    public class FakeDriver : Driver
    {
        public readonly IList<Element> ClickedElements = new List<Element>();
        public readonly IList<Element> HoveredElements = new List<Element>();
        public readonly IList<Element> CheckedElements = new List<Element>();
        public readonly IList<Element> UncheckedElements = new List<Element>();
        public readonly IList<Element> ChosenElements = new List<Element>();
        public readonly IList<string> HasContentQueries = new List<string>();
        public readonly IList<Regex> HasContentMatchQueries = new List<Regex>();
        public readonly IList<string> HasCssQueries = new List<string>();
        public readonly IList<string> HasXPathQueries = new List<string>();
        public readonly IList<string> Visits = new List<string>();
        public readonly IDictionary<Element, string> SetFields = new Dictionary<Element, string>();
        public readonly IDictionary<Element, string> SelectedOptions = new Dictionary<Element, string>();
        private readonly IList<ScopedStubResult> stubbedButtons = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedLinks = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedTextFields = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedAllCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedAllXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedExecuteScriptResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedFieldsets = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedSections = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedIFrames = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedIDs = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasContentResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasContentMatchResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasDialogResults = new List<ScopedStubResult>();
        private readonly List<ScopedStubResult> stubbedWindows = new List<ScopedStubResult>();
        public readonly IList<string> FindButtonRequests = new List<string>();
        public readonly IList<string> FindLinkRequests = new List<string>();
        public readonly IList<string> FindCssRequests = new List<string>();
        private IList<Cookie> stubbedCookies;
        private Uri stubbedLocation;

        public List<DriverScope> ModalDialogsAccepted = new List<DriverScope>();
        public List<DriverScope> ModalDialogsCancelled = new List<DriverScope>();


        public FakeDriver() {}
        public FakeDriver(Drivers.Browser browser)
        {
            Browser = browser;
        }

        
        class ScopedStubResult
        {
            public object Locator;
            public object Result;
            public DriverScope Scope;
        }


        public Drivers.Browser Browser { get; private set; }

        public ElementFound FindButton(string locator, DriverScope scope)
        {
            FindButtonRequests.Add(locator);
            return Find<ElementFound>(stubbedButtons,locator,scope);
        }

        private T Find<T>(IEnumerable<ScopedStubResult> stubbed, object locator, DriverScope scope)
        {
            var scopedStubResult = stubbed.FirstOrDefault(r => r.Locator == locator && r.Scope == scope);
            if (scopedStubResult == null)
            {
                throw new MissingHtmlException("Element not found: " + locator);
            }
            return (T) scopedStubResult.Result;
        }

        public ElementFound FindLink(string linkText, DriverScope scope)
        {
            FindLinkRequests.Add(linkText);

            return Find<ElementFound>(stubbedLinks, linkText, scope);
        }

        public ElementFound FindField(string locator, DriverScope scope)
        {
            return Find<ElementFound>(stubbedTextFields, locator, scope);
        }

        public void Click(Element element)
        {
            ClickedElements.Add(element);
        }

        public void Hover(Element element)
        {
            HoveredElements.Add(element);
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return stubbedCookies;
        }

        public void SetBrowserCookies(Cookie cookie)
        {
        }

        public void Visit(string url)
        {
            Visits.Add(url);
        }

        public void StubButton(string locator, ElementFound element, DriverScope scope)
        {
            stubbedButtons.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = element});
        }

        public void StubLink(string locator, ElementFound element, DriverScope scope)
        {
            stubbedLinks.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = element});
        }

        public void StubField(string locator, ElementFound element, DriverScope scope)
        {
            stubbedTextFields.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = element});
        }

        public void StubHasContent(string text, bool result, DriverScope scope)
        {
            stubbedHasContentResults.Add(new ScopedStubResult { Locator = text, Scope = scope, Result = result });
        }

        public void StubHasContentMatch(Regex pattern, bool result, DriverScope scope)
        {
            stubbedHasContentMatchResults.Add(new ScopedStubResult { Locator = pattern, Scope = scope, Result = result });
        }

        public void StubHasCss(string cssSelector, bool result, DriverScope scope)
        {
            stubbedHasCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubHasXPath(string xpath, bool result, DriverScope scope)
        {
            stubbedHasXPathResults.Add(new ScopedStubResult { Locator = xpath, Scope = scope, Result = result });
        }

        public void StubDialog(string text, bool result, DriverScope scope)
        {
            stubbedHasDialogResults.Add(new ScopedStubResult { Locator = text, Scope = scope, Result = result });
        }

        public void StubCss(string cssSelector, ElementFound result, DriverScope scope)
        {
            stubbedCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubXPath(string cssSelector, ElementFound result, DriverScope scope)
        {
            stubbedXPathResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubAllCss(string cssSelector, IEnumerable<ElementFound> result, DriverScope scope)
        {
            stubbedAllCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result });
        }

        public void StubAllXPath(string xpath, IEnumerable<ElementFound> result, DriverScope scope)
        {
            stubbedAllXPathResults.Add(new ScopedStubResult { Locator = xpath, Scope = scope, Result = result });
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public bool Disposed { get; private set; }

        public Uri Location
        {
            get { return stubbedLocation; }
        }

        public ElementFound Window
        {
            get { throw new NotImplementedException(); }
        }

        public void AcceptModalDialog(DriverScope scope)
        {
            ModalDialogsAccepted.Add(scope);
        }

        public void CancelModalDialog(DriverScope scope)
        {
            ModalDialogsCancelled.Add(scope);
        }

        public string ExecuteScript(string javascript, DriverScope scope)
        {
            return Find<string>(stubbedExecuteScriptResults, javascript, scope);
        }

        public ElementFound FindFieldset(string locator, DriverScope scope)
        {
            return Find<ElementFound>(stubbedFieldsets, locator, scope);
        }

        public ElementFound FindSection(string locator, DriverScope scope)
        {
            return Find<ElementFound>(stubbedSections, locator, scope);
        }

        public ElementFound FindId(string id, DriverScope scope)
        {
            return Find<ElementFound>(stubbedIDs, id, scope);
        }

        public ElementFound FindIFrame(string locator, DriverScope scope)
        {
            return Find<ElementFound>(stubbedIFrames, locator, scope);
        }

        public void Set(Element element, string value)
        {
            SetFields.Add(element, value);
        }

        public void Select(Element element, string option)
        {
            SelectedOptions.Add(element, option);
        }

        public object Native
        {
            get { return "Native driver on fake driver"; }
        }

        public bool HasContent(string text, DriverScope scope)
        {
            HasContentQueries.Add(text);
            return Find<bool>(stubbedHasContentResults, text, scope);
        }

        public bool HasContentMatch(Regex pattern, DriverScope scope)
        {
            HasContentMatchQueries.Add(pattern);
            return Find<bool>(stubbedHasContentMatchResults, pattern, scope);
        }

        public bool HasCss(string cssSelector, DriverScope scope)
        {
            return Find<bool>(stubbedHasCssResults, cssSelector, scope);
        }

        public bool HasXPath(string xpath, DriverScope scope)
        {
            return Find<bool>(stubbedHasXPathResults, xpath, scope);
        }

        public bool HasDialog(string withText, DriverScope scope)
        {
            return Find<bool>(stubbedHasDialogResults, withText, scope);
        }

        public ElementFound FindCss(string cssSelector, DriverScope scope)
        {
            FindCssRequests.Add(cssSelector);
            return Find<ElementFound>(stubbedCssResults, cssSelector, scope);
        }

        public ElementFound FindXPath(string xpath, DriverScope scope)
        {
            return Find<ElementFound>(stubbedXPathResults, xpath, scope);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, DriverScope scope)
        {
            return Find<IEnumerable<ElementFound>>(stubbedAllCssResults, cssSelector, scope);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, DriverScope scope)
        {
            return Find<IEnumerable<ElementFound>>(stubbedAllXPathResults, xpath, scope);
        }

        public void Check(Element field)
        {
            CheckedElements.Add(field);
        }

        public void Uncheck(Element field)
        {
            UncheckedElements.Add(field);
        }

        public void Choose(Element field)
        {
            ChosenElements.Add(field);
        }

        public void StubExecuteScript(string script, string scriptReturnValue, DriverScope scope)
        {
            stubbedExecuteScriptResults.Add(new ScopedStubResult{Locator = script, Scope =  scope, Result = scriptReturnValue});
        }

        public void StubFieldset(string locator, ElementFound fieldset, DriverScope scope)
        {
            stubbedFieldsets.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = fieldset});
        }

        public void StubSection(string locator, ElementFound section, DriverScope scope)
        {
            stubbedSections.Add(new ScopedStubResult{Locator = locator, Scope =  scope, Result = section});
        }

        public void StubIFrame(string locator, ElementFound iframe, DriverScope scope)
        {
            stubbedIFrames.Add(new ScopedStubResult { Locator = locator, Scope = scope, Result = iframe });
        }

        public void StubId(string id, ElementFound element, DriverScope scope)
        {
            stubbedIDs.Add(new ScopedStubResult { Locator = id, Scope = scope, Result = element });
        }

        public void StubCookies(List<Cookie> cookies)
        {
            stubbedCookies = cookies;
        }

        public void StubLocation(Uri location)
        {
            stubbedLocation = location;
        }

        public void StubWindow(string locator, ElementFound window, DriverScope scope)
        {
            stubbedWindows.Add(new ScopedStubResult {Locator = locator, Scope = scope, Result = window});
        }

        public ElementFound FindWindow(string locator, DriverScope scope)
        {
            return Find<ElementFound>(stubbedWindows, locator, scope);
        }
    }


}