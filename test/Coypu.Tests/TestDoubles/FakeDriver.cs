using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Coypu.Drivers;

namespace Coypu.Tests.TestDoubles
{
    public class FakeDriver : Driver
    {
        public readonly IList<Element> ClickedElements = new List<Element>();
        public readonly IList<Element> HoveredElements = new List<Element>();
        public readonly IList<Element> CheckedElements = new List<Element>();
        public readonly IList<Element> UncheckedElements = new List<Element>();
        public readonly IList<Element> ChosenElements = new List<Element>();
        public readonly IList<ScopedRequest<string>> Visits = new List<ScopedRequest<string>>();
        public readonly IDictionary<Element, SetFieldParams> SetFields = new Dictionary<Element, SetFieldParams>();
        public readonly IDictionary<Element, string> SentKeys = new Dictionary<Element, string>();
        private readonly IList<ScopedStubResult> stubbedAllCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedAllXPathResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedExecuteScriptResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedFrames = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedHasDialogResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedWindows = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedLocations = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> stubbedTitles = new List<ScopedStubResult>();
        private Element stubbedCurrentWindow;
        public readonly IList<FindXPathParams> FindXPathRequests = new List<FindXPathParams>();
        public readonly IList<Scope> MaximiseWindowCalls = new List<Scope>();
        public readonly IList<Scope> RefreshCalls = new List<Scope>();
        public readonly IList<ScopedRequest<Size>> ResizeToCalls = new List<ScopedRequest<Size>>();
        public readonly IList<Scope> GoBackCalls = new List<Scope>();
        public readonly IList<Scope> GoForwardCalls = new List<Scope>();
        public readonly IList<ScopedRequest<string>> SaveScreenshotCalls = new List<ScopedRequest<string>>();

        private IList<Cookie> stubbedCookies;

        public List<Scope> ModalDialogsAccepted = new List<Scope>();
        public List<Scope> ModalDialogsCancelled = new List<Scope>();

        public FakeDriver() {}
        public FakeDriver(Drivers.Browser browser)
        {
            Browser = browser;
        }
        
        public class SaveScreenshotParams
        {
            public string SaveAs;
            public ImageFormat ImageFormat;
        }

        class ScopedStubResult
        {
            public string Locator;
            public object Result;
            public Scope Scope;
            public Regex TextPattern = null;
            public Options Options;
        }

        public class ScopedRequest<T>
        {
            public T Request;
            public Scope Scope;
        }

        public Drivers.Browser Browser { get; private set; }

        private T Find<T>(IEnumerable<ScopedStubResult> stubbed, string locator, Scope scope, Options options = null, Regex textPattern = null)
        {
            var stubResult = stubbed
                .FirstOrDefault(
                    r =>
                    r.Locator == locator && (r.Scope == scope || scope.Now() == r.Scope.Now()) && r.TextPattern == textPattern && options == r.Options);

            if (stubResult == null)
                throw new MissingHtmlException("No stubbed result found for: " + locator);

            return (T) stubResult.Result;
        }

        private IEnumerable<T> FindAll<T>(IEnumerable<ScopedStubResult> stubbed, object locator, Scope scope, Options options = null, Regex textPattern = null)
        {
            var stubResult = stubbed.FirstOrDefault(r => r.Locator == locator.ToString() && r.Scope == scope && r.TextPattern == textPattern && options == r.Options);
            if (stubResult == null)
                return Enumerable.Empty<T>();

            return (IEnumerable<T>) stubResult.Result;
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

        public void Visit(string url, Scope scope)
        {
            Visits.Add(new ScopedRequest<string>{Request = url, Scope = scope});
        }

        public void StubDialog(string text, bool result, Scope scope)
        {
            stubbedHasDialogResults.Add(new ScopedStubResult { Locator = text, Scope = scope, Result = result});
        }

        public void StubAllCss(string cssSelector, IEnumerable<Element> result, Scope scope, Options options)
        {
            stubbedAllCssResults.Add(new ScopedStubResult { Locator = cssSelector, Scope = scope, Result = result, Options = options });
        }

        public void StubAllXPath(string xpath, IEnumerable<Element> result, Scope scope, Options options)
        {
            stubbedAllXPathResults.Add(new ScopedStubResult { Locator = xpath, Scope = scope, Result = result, Options = options });
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
            return Find<Uri>(stubbedLocations, null, scope);
        }

        public string Title(Scope scope)
        {
            return Find<String>(stubbedTitles, null, scope);
        }

        public Element Window
        {
            get { return stubbedCurrentWindow; }
        }

        public void AcceptModalDialog(Scope scope)
        {
            ModalDialogsAccepted.Add(scope);
        }

        public void CancelModalDialog(Scope scope)
        {
            ModalDialogsCancelled.Add(scope);
        }

        public object ExecuteScript(string javascript, Scope scope, params object[] args)
        {
            return Find<string>(stubbedExecuteScriptResults, javascript, scope);
        }

        public IEnumerable<Element> FindFrames(string locator, Scope scope, Options options)
        {
            return Find<IEnumerable<Element>>(stubbedFrames, locator, scope, options);
        }

        public void SendKeys(Element element, string keys)
        {
            SentKeys.Add(element, keys);
        }

        public void MaximiseWindow(Scope scope)
        {
            MaximiseWindowCalls.Add(scope);
        }

        public void Refresh(Scope scope)
        {
            RefreshCalls.Add(scope);
        }

        public void ResizeTo(Size size, Scope Scope)
        {
            ResizeToCalls.Add(new ScopedRequest<Size>{Request = size, Scope = Scope});
        }

        public void SaveScreenshot(string fileName, Scope scope)
        {
            SaveScreenshotCalls.Add(new ScopedRequest<string>
                {
                    Request = fileName,
                    Scope = scope
                });
        }

        public void GoBack(Scope scope)
        {
            GoBackCalls.Add(scope);
        }

        public void GoForward(Scope scope)
        {
            GoForwardCalls.Add(scope);
        }

        public void Set(Element element, string value)
        {
            SetFields.Add(element, new SetFieldParams{Value = value});
        }

        public object Native
        {
            get { return "Native driver on fake driver"; }
        }

        public bool HasDialog(string withText, Scope scope)
        {
            return Find<bool>(stubbedHasDialogResults, withText, scope);
        }

        private static bool RegexEqual(Regex a, Regex b)
        {
            return (a.ToString() == b.ToString() && a.Options == b.Options);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector, Scope scope, Options options, Regex textPattern = null)
        {
            return Find<IEnumerable<Element>>(stubbedAllCssResults, cssSelector, scope, options, textPattern);
        }

        public IEnumerable<Element> FindAllXPath(string xpath, Scope scope, Options options)
        {
            FindXPathRequests.Add(new FindXPathParams{XPath = xpath, Scope = scope, Options = options});
            return Find<IEnumerable<Element>>(stubbedAllXPathResults, xpath, scope, options);
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

        public void StubExecuteScript(string script, string scriptReturnValue, Scope scope)
        {
            stubbedExecuteScriptResults.Add(new ScopedStubResult{Locator = script, Scope =  scope, Result = scriptReturnValue});
        }

        public void StubFrames(string locator, Element frame, Scope scope, Options options)
        {
            stubbedFrames.Add(new ScopedStubResult { Locator = locator, Scope = scope, Result = frame, Options = options });
        }

        public void StubId(string id, Element element, Scope scope, SessionConfiguration options)
        {
            StubAllXPath(new Html(options.Browser.UppercaseTagNames).Id(id, options), new[] { element }, scope, options);
        }

        public void StubCookies(List<Cookie> cookies)
        {
            stubbedCookies = cookies;
        }

        public void StubLocation(Uri location, Scope scope)
        {
            stubbedLocations.Add(new ScopedStubResult {Result = location, Scope = scope});
        }

        public void StubTitle(String title, Scope scope)
        {
            stubbedTitles.Add(new ScopedStubResult { Result = title, Scope = scope });
        }

        public void StubWindow(string locator, Element window, Scope scope, Options options)
        {
            stubbedWindows.Add(new ScopedStubResult {Locator = locator, Scope = scope, Result = window, Options = options});
        }

        public void StubCurrentWindow(Element window)
        {
            stubbedCurrentWindow = window;
        }

        public IEnumerable<Element> FindWindows(string locator, Scope scope, Options options)
        {
            return Find<IEnumerable<Element>>(stubbedWindows, locator, scope, options);
        }

    }

    public class SetFieldParams
    {
        public string Value { get; set; }
    }

    public class FindXPathParams
    {
        public string XPath { get; set; }
        public Regex TextPattern { get; set; }
        public Options Options { get; set; }
        public Scope Scope { get; set; }
    }
}