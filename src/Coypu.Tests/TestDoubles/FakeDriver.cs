using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using Coypu.Drivers;
using OpenQA.Selenium;
using Cookie = System.Net.Cookie;

namespace Coypu.Tests.TestDoubles
{
    public class FakeDriver : IDriver
    {
        public readonly IList<Element> CheckedElements = new List<Element>();
        public readonly IList<Element> ChosenElements = new List<Element>();
        public readonly List<SelectOptionParams> SelectedOptions = new List<SelectOptionParams>();
        public readonly IList<Element> ClickedElements = new List<Element>();
        public readonly IList<FindXPathParams> FindXPathRequests = new List<FindXPathParams>();
        public readonly IList<Scope> GoBackCalls = new List<Scope>();
        public readonly IList<Scope> GoForwardCalls = new List<Scope>();
        public readonly IList<Element> HoveredElements = new List<Element>();
        public readonly IList<Scope> MaximiseWindowCalls = new List<Scope>();
        public List<Scope> ModalDialogsAccepted = new List<Scope>();
        public List<Scope> ModalDialogsCancelled = new List<Scope>();
        public readonly IList<Scope> RefreshCalls = new List<Scope>();
        public readonly IList<ScopedRequest<Size>> ResizeToCalls = new List<ScopedRequest<Size>>();
        public readonly IList<ScopedRequest<string>> SaveScreenshotCalls = new List<ScopedRequest<string>>();
        public readonly IDictionary<Element, string> SentKeys = new Dictionary<Element, string>();
        public readonly IDictionary<Element, SetFieldParams> SetFields = new Dictionary<Element, SetFieldParams>();
        public readonly IList<Element> UncheckedElements = new List<Element>();
        public readonly IList<ScopedRequest<string>> Visits = new List<ScopedRequest<string>>();
        private readonly IList<ScopedStubResult> _stubbedAllCssResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> _stubbedAllXPathResults = new List<ScopedStubResult>();
        private IList<Cookie> _stubbedCookies;
        private readonly IList<ScopedStubResult> _stubbedExecuteScriptResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> _stubbedFrames = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> _stubbedHasDialogResults = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> _stubbedLocations = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> _stubbedTitles = new List<ScopedStubResult>();
        private readonly IList<ScopedStubResult> _stubbedWindows = new List<ScopedStubResult>();

        public FakeDriver() { }

        public FakeDriver(Browser browser, bool headless)
        {
            Browser = browser;
        }

        public FakeDriver(IWebDriver driver, bool headless)
        {
            Cookies = new FakeCookies();
        }

        public Browser Browser { get; }

        public void ClearBrowserCookies()
        {
            _stubbedCookies.Clear();
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
            return _stubbedCookies;
        }

        public void Visit(string url,
                          Scope scope)
        {
            Visits.Add(new ScopedRequest<string> {Request = url, Scope = scope});
        }

        public void Dispose()
        {
            Disposed = true;
        }

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
            return Find<Uri>(_stubbedLocations, null, scope);
        }

        public string Title(Scope scope)
        {
            return Find<string>(_stubbedTitles, null, scope);
        }

        public Element Window { get; private set; }

        public void AcceptModalDialog(Scope scope)
        {
            ModalDialogsAccepted.Add(scope);
        }

        public void CancelModalDialog(Scope scope)
        {
            ModalDialogsCancelled.Add(scope);
        }

        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            return Find<string>(_stubbedExecuteScriptResults, javascript, scope);
        }

        public IEnumerable<Element> FindFrames(string locator,
                                               Scope scope,
                                               Options options)
        {
            return Find<IEnumerable<Element>>(_stubbedFrames, locator, scope, options);
        }

        public void SendKeys(Element element,
                             string keys)
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

        public void ResizeTo(Size size,
                             Scope scope)
        {
            ResizeToCalls.Add(new ScopedRequest<Size> {Request = size, Scope = scope});
        }

        public void SaveScreenshot(string fileName,
                                   Scope scope)
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

        public void Set(Element element,
                        string value)
        {
            SetFields.Add(element, new SetFieldParams {Value = value});
        }

        public Cookies Cookies { get; set; }

        public object Native => "Native driver on fake driver";

        public bool HasDialog(string withText,
                              Scope scope)
        {
            return Find<bool>(_stubbedHasDialogResults, withText, scope);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            return Find<IEnumerable<Element>>(_stubbedAllCssResults, cssSelector, scope, options, textPattern);
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            FindXPathRequests.Add(new FindXPathParams {XPath = xpath, Scope = scope, Options = options});
            return Find<IEnumerable<Element>>(_stubbedAllXPathResults, xpath, scope, options);
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

        public void SelectOption(Element select, Element option, string optionToSelect)
        {
            SelectedOptions.Add(new SelectOptionParams {Select = select, Option = option, OptionToSelect = optionToSelect});
        }

        public IEnumerable<Element> FindWindows(string locator,
                                                Scope scope,
                                                Options options)
        {
            return Find<IEnumerable<Element>>(_stubbedWindows, locator, scope, options);
        }

        public void SetBrowserCookies(Cookie cookie) { }

        public void StubDialog(string text,
                               bool result,
                               Scope scope)
        {
            _stubbedHasDialogResults.Add(new ScopedStubResult {Locator = text, Scope = scope, Result = result});
        }

        public void StubAllCss(string cssSelector,
                               IEnumerable<Element> result,
                               Scope scope,
                               Options options)
        {
            _stubbedAllCssResults.Add(new ScopedStubResult {Locator = cssSelector, Scope = scope, Result = result, Options = options});
        }

        public void StubAllXPath(string xpath,
                                 IEnumerable<Element> result,
                                 Scope scope,
                                 Options options)
        {
            _stubbedAllXPathResults.Add(new ScopedStubResult {Locator = xpath, Scope = scope, Result = result, Options = options});
        }

        public void StubExecuteScript(string script,
                                      string scriptReturnValue,
                                      Scope scope)
        {
            _stubbedExecuteScriptResults.Add(new ScopedStubResult {Locator = script, Scope = scope, Result = scriptReturnValue});
        }

        public void StubFrames(string locator,
                               Element frame,
                               Scope scope,
                               Options options)
        {
            _stubbedFrames.Add(new ScopedStubResult {Locator = locator, Scope = scope, Result = frame, Options = options});
        }

        public void StubId(string id,
                           Element element,
                           Scope scope,
                           SessionConfiguration options)
        {
            StubAllXPath(new Html(options.Browser.UppercaseTagNames).Id(id, options), new[] {element}, scope, options);
        }

        public void StubCookies(List<Cookie> cookies)
        {
            _stubbedCookies = cookies;
        }

        public void StubLocation(Uri location,
                                 Scope scope)
        {
            _stubbedLocations.Add(new ScopedStubResult {Result = location, Scope = scope});
        }

        public void StubTitle(string title,
                              Scope scope)
        {
            _stubbedTitles.Add(new ScopedStubResult {Result = title, Scope = scope});
        }

        public void StubWindow(string locator,
                               Element window,
                               Scope scope,
                               Options options)
        {
            _stubbedWindows.Add(new ScopedStubResult {Locator = locator, Scope = scope, Result = window, Options = options});
        }

        public void StubCurrentWindow(Element window)
        {
            Window = window;
        }

        private T Find<T>(IEnumerable<ScopedStubResult> stubbed,
                          string locator,
                          Scope scope,
                          Options options = null,
                          Regex textPattern = null)
        {
            var stubResult = stubbed
                .FirstOrDefault(
                    r =>
                        r.Locator == locator && (r.Scope == scope || scope.Now() == r.Scope.Now()) && r.TextPattern == textPattern && options == r.Options);

            if (stubResult == null)
                throw new MissingHtmlException("No stubbed result found for: " + locator);

            return (T) stubResult.Result;
        }

        private IEnumerable<T> FindAll<T>(IEnumerable<ScopedStubResult> stubbed,
                                          object locator,
                                          Scope scope,
                                          Options options = null,
                                          Regex textPattern = null)
        {
            var stubResult = stubbed.FirstOrDefault(r => r.Locator == locator.ToString() && r.Scope == scope && r.TextPattern == textPattern && options == r.Options);
            if (stubResult == null)
                return Enumerable.Empty<T>();

            return (IEnumerable<T>) stubResult.Result;
        }

        private static bool RegexEqual(Regex a,
                                       Regex b)
        {
            return a.ToString() == b.ToString() && a.Options == b.Options;
        }

    public void AcceptAlert(string text, DriverScope root, Action trigger)
    {
      throw new NotImplementedException();
    }

    public void AcceptConfirm(string text, DriverScope root, Action trigger)
    {
      throw new NotImplementedException();
    }

    public void CancelConfirm(string text, DriverScope root, Action trigger)
    {
      throw new NotImplementedException();
    }

    public void AcceptPrompt(string text, string promptValue, DriverScope root, Action trigger)
    {
      throw new NotImplementedException();
    }

    public void CancelPrompt(string text, DriverScope root, Action trigger)
    {
      throw new NotImplementedException();
    }

    public class SaveScreenshotParams
        {
            public ImageFormat ImageFormat;
            public string SaveAs;
        }

        private class ScopedStubResult
        {
            public string Locator;
            public Options Options;
            public object Result;
            public Scope Scope;
            public readonly Regex TextPattern;
            public ScopedStubResult() { }

            public ScopedStubResult(Regex textPattern)
            {
                TextPattern = textPattern;
            }
        }

        public class ScopedRequest<T>
        {
            public T Request;
            public Scope Scope;
        }

    public class SelectOptionParams
    {
      public Element Select;
      public Element Option;
      public string OptionToSelect;
    }
  }

    public class SetFieldParams
    {
        public string Value { get; set; }
    }

    public class FindXPathParams
    {
        public Options Options { get; set; }
        public Scope Scope { get; set; }
        public Regex TextPattern { get; set; }
        public string XPath { get; set; }
    }

    // Implementation of Cookies interface with in memory stores for cookies that behaves like a real browser might
    public class FakeCookies : Cookies {
        private readonly List<Cookie> _cookies = new List<Cookie>();

        public void AddCookie(Cookie cookie, Options options = null)
        {
            _cookies.Add(cookie);
        }

        public void DeleteAll()
        {
            _cookies.Clear();
        }

        public void DeleteCookie(Cookie cookie)
        {
            _cookies.Remove(cookie);
        }

        public void DeleteCookieNamed(string cookieName)
        {
            _cookies.RemoveAll(c => c.Name == cookieName);
        }

        public IEnumerable<Cookie> GetAll()
        {
            return _cookies;
        }

        public Cookie GetCookieNamed(string cookieName)
        {
            return _cookies.FirstOrDefault(c => c.Name == cookieName);
        }

        public void WaitUntilCookieExists(Cookie cookie, Options options)
        {
            System.Threading.Thread.Sleep(1);
        }
    }
}
