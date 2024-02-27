using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using Interactions = OpenQA.Selenium.Interactions;
using Cookie = System.Net.Cookie;

#pragma warning disable 1591

namespace Coypu.Drivers.Selenium
{
    public class SeleniumWebDriver : IDriver
    {
        private readonly Browser _browser;
        private readonly Dialogs _dialogs;
        private readonly ElementFinder _elementFinder;
        private readonly FrameFinder _frameFinder;
        private readonly MouseControl _mouseControl;
        private readonly SeleniumWindowManager _seleniumWindowManager;
        private readonly TextMatcher _textMatcher;
        private IWebDriver _webDriver;
        private readonly WindowHandleFinder _windowHandleFinder;

        public SeleniumWebDriver(Browser browser, bool headless)
            : this(new DriverFactory().NewWebDriver(browser, headless), browser) { }

        protected SeleniumWebDriver(IWebDriver webDriver,
                                    Browser browser)
        {
            _webDriver = webDriver;
            _browser = browser;
            _elementFinder = new ElementFinder();
            _textMatcher = new TextMatcher();
            _dialogs = new Dialogs(_webDriver);
            _mouseControl = new MouseControl(_webDriver);
            _seleniumWindowManager = new SeleniumWindowManager(_webDriver);
            _frameFinder = new FrameFinder(_webDriver, _elementFinder, new XPath(browser.UppercaseTagNames), _seleniumWindowManager);
            _windowHandleFinder = new WindowHandleFinder(_webDriver, _seleniumWindowManager);
            Cookies = new Cookies(_webDriver);
        }

        private IJavaScriptExecutor JavaScriptExecutor => _webDriver as IJavaScriptExecutor;

        protected bool NoJavascript => !_browser.Javascript;

        public bool Disposed { get; private set; }

        public Uri Location(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            return new Uri(_webDriver.Url);
        }

        public string Title(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            return _webDriver.Title;
        }

        public Element Window => new SeleniumWindow(_webDriver, _webDriver.CurrentWindowHandle, _seleniumWindowManager);

        public Coypu.Cookies Cookies { get; set; }
        public object Native => _webDriver;

        public IEnumerable<Element> FindFrames(string locator,
                                               Scope scope,
                                               Options options)
        {
            return _frameFinder.FindFrame(locator, scope, options)
                               .Select(BuildElement);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector,
                                               Scope scope,
                                               Options options,
                                               Regex textPattern = null)
        {
            return FindAll(By.CssSelector(cssSelector), scope, options, ValidateTextPattern(options, textPattern))
                .Select(BuildElement);
        }

        public IEnumerable<Element> FindAllXPath(string xpath,
                                                 Scope scope,
                                                 Options options)
        {
            return FindAll(By.XPath(xpath), scope, options)
                .Select(BuildElement);
        }

        public bool HasDialog(string withText,
                              Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            return _dialogs.HasDialog(withText);
        }

        public void Visit(string url,
                          Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _webDriver.Navigate()
                      .GoToUrl(url);
        }

        public void ClearBrowserCookies()
        {
            Cookies.DeleteAll();
        }

        public void Click(Element element)
        {
            SeleniumElement(element)
                .Click();
        }

        public void DblClick(Element element)
        {
          var act = new Interactions.Actions(_webDriver);
          act.DoubleClick();
        }

        public void Hover(Element element)
        {
            _mouseControl.Hover(element);
        }

        public void SendKeys(Element element,
                             string keys)
        {
            SeleniumElement(element)
                .SendKeys(keys);
        }

        public void MaximiseWindow(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _webDriver.Manage()
                      .Window.Maximize();
        }

        public void Refresh(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _webDriver.Navigate()
                      .Refresh();
        }

        public void ResizeTo(Size size,
                             Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _webDriver.Manage()
                      .Window.Size = size;
        }

        public void SaveScreenshot(string fileName,
                                   Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            var format = ImageFormatParser.GetImageFormat(fileName);

            var screenshot = ((ITakesScreenshot) _webDriver).GetScreenshot();
            screenshot.SaveAsFile(fileName, format);
        }

        public void GoBack(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _webDriver.Navigate()
                      .Back();
        }

        public void GoForward(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _webDriver.Navigate()
                      .Forward();
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return _webDriver.Manage()
                             .Cookies.AllCookies.Select(c => new Cookie(c.Name, c.Value, c.Path, c.Domain));
        }

        public IEnumerable<Element> FindWindows(string titleOrName,
                                                Scope scope,
                                                Options options)
        {
            _elementFinder.SeleniumScope(scope);
            return _windowHandleFinder.FindWindowHandles(titleOrName, options)
                                      .Select(h => new SeleniumWindow(_webDriver, h, _seleniumWindowManager));
        }

        public void Set(Element element,
                        string value)
        {
            try
            {
                SeleniumElement(element)
                    .Clear();
            }
            catch (InvalidElementStateException) { } // Non user-editable elements (file inputs) - chrome/IE
            catch (InvalidOperationException) { } // Non user-editable elements (file inputs) - firefox

            SendKeys(element, value);
        }

        public void AcceptModalDialog(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _dialogs.AcceptModalDialog();
        }

        public void CancelModalDialog(Scope scope)
        {
            _elementFinder.SeleniumScope(scope);
            _dialogs.CancelModalDialog();
        }

        public void Check(Element field)
        {
            var seleniumElement = SeleniumElement(field);

            if (!seleniumElement.Selected)
                seleniumElement.Click();
        }

        public void Uncheck(Element field)
        {
            var seleniumElement = SeleniumElement(field);

            if (seleniumElement.Selected)
                seleniumElement.Click();
        }

        public void Choose(Element field)
        {
            SeleniumElement(field)
                .Click();
        }

        public void SelectOption(Element select, Element option, string optionToSelect)
        {
            Click(option);
        }
        public object ExecuteScript(string javascript,
                                    Scope scope,
                                    params object[] args)
        {
            if (NoJavascript)
                throw new NotSupportedException("Javascript is not supported by " + _browser);

            _elementFinder.SeleniumScope(scope);
            return JavaScriptExecutor.ExecuteScript(javascript, ConvertScriptArgs(args));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            if (disposing)
            {
                AcceptAnyAlert();

                _webDriver.Quit();
                _webDriver = null;
            }

            Disposed = true;
        }

        private Func<IWebElement, bool> ValidateTextPattern(Options options,
                                                            Regex textPattern)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            var textMatches = textPattern == null
                                  ? (Func<IWebElement, bool>) null
                                  : e => _textMatcher.TextMatches(e, textPattern);

            if (textPattern != null && options.ConsiderInvisibleElements)
                throw new NotSupportedException("Cannot inspect the text of invisible elements.");
            return textMatches;
        }

        private IEnumerable<IWebElement> FindAll(By by,
                                                 Scope scope,
                                                 Options options,
                                                 Func<IWebElement, bool> predicate = null)
        {
            return _elementFinder.FindAll(by, scope, options, predicate);
        }

        private Element BuildElement(IWebElement element)
        {
            return new[] {"iframe", "frame"}.Contains(element.TagName.ToLower())
                       ? new SeleniumFrame(element, _webDriver, _seleniumWindowManager)
                       : new SeleniumElement(element, _webDriver);
        }

        private static object[] ConvertScriptArgs(object[] args)
        {
            for (var i = 0; i < args.Length; ++i)
                if (args[i] is Element argAsElement)
                    args[i] = argAsElement.Native;

            return args;
        }

        private IWebElement SeleniumElement(Element element)
        {
            return (IWebElement) element.Native;
        }

        private void AcceptAnyAlert()
        {
            try
            {
                _seleniumWindowManager.SwitchToWindow(_webDriver.WindowHandles[0]);
                if (_dialogs.HasAnyDialog())
                    _webDriver.SwitchTo()
                              .Alert()
                              .Accept();
            }
            catch (WebDriverException) { }
            catch (KeyNotFoundException) { } // Chrome
            catch (InvalidOperationException) { }
            catch (IndexOutOfRangeException) { } // No window handles
        }

    public void AcceptAlert(string text, DriverScope scope, Action trigger)
    {
        trigger.Invoke();
        _elementFinder.SeleniumScope(scope);
        if (text != null && _dialogs.HasAnyDialog() && !_dialogs.HasDialog(text)) {
            throw new MissingDialogException("A dialog was present but didn't match the expected text.");
        }
        _dialogs.AcceptModalDialog();
    }

    public void CancelAlert(string text, DriverScope scope, Action trigger)
    {
        trigger.Invoke();
        _elementFinder.SeleniumScope(scope);
        if (text != null && _dialogs.HasAnyDialog() && !_dialogs.HasDialog(text)) {
            throw new MissingDialogException("A dialog was present but didn't match the expected text.");
        }
        _dialogs.CancelModalDialog();
    }

    public void AcceptConfirm(string text, DriverScope scope, Action trigger)
    {
        // Webdriver cannot distinguish between confirm and alert
        AcceptAlert(text, scope, trigger);
    }

    public void CancelConfirm(string text, DriverScope scope, Action trigger)
    {
        // Webdriver cannot distinguish between confirm and alert
        CancelAlert(text, scope, trigger);
    }

    public void AcceptPrompt(string text, string promptValue, DriverScope scope, Action trigger)
    {
        trigger.Invoke();
        _elementFinder.SeleniumScope(scope);
        if (text != null && _dialogs.HasAnyDialog() && !_dialogs.HasDialog(text)) {
            throw new MissingDialogException("A dialog was present but didn't match the expected text.");
        }
        _dialogs.AcceptModalDialog(promptValue);
    }

    public void CancelPrompt(string text, DriverScope scope, Action trigger)
    {
        // Webdriver cannot distinguish between prompt and alert
        CancelAlert(text, scope, trigger);
    }
  }
}
