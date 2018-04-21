using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Cookie = System.Net.Cookie;

namespace Coypu.Drivers.Selenium
{
    public class SeleniumWebDriver : IDriver
    {
        public bool Disposed { get; private set; }
        private IWebDriver webDriver;
        private readonly ElementFinder elementFinder;
        private readonly FrameFinder frameFinder;
        private readonly TextMatcher textMatcher;
        private readonly Dialogs dialogs;
        private readonly MouseControl mouseControl;
        private readonly XPath xPath;
        private readonly Browser browser;
        private readonly WindowHandleFinder windowHandleFinder;
        private SeleniumWindowManager seleniumWindowManager;

        public SeleniumWebDriver(Browser browser)
            : this(new DriverFactory().NewWebDriver(browser),  browser)
        {
        }

        protected SeleniumWebDriver(IWebDriver webDriver, Browser browser)
        {
            this.webDriver = webDriver;
            this.browser = browser;
            xPath = new XPath(browser.UppercaseTagNames);
            elementFinder = new ElementFinder();
            textMatcher = new TextMatcher();
            dialogs = new Dialogs(this.webDriver);
            mouseControl = new MouseControl(this.webDriver);
            seleniumWindowManager = new SeleniumWindowManager(this.webDriver);
            frameFinder = new FrameFinder(this.webDriver, elementFinder, xPath, seleniumWindowManager);
            windowHandleFinder = new WindowHandleFinder(this.webDriver, seleniumWindowManager);
        }

        public Uri Location(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            return new Uri(webDriver.Url);
        }

        public String Title(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            return webDriver.Title;
        }

        public Element Window
        {
            get
            {
                return new SeleniumWindow(webDriver, webDriver.CurrentWindowHandle, seleniumWindowManager);
            }
        }

        protected bool NoJavascript
        {
            get { return !browser.Javascript; }
        }

        private IJavaScriptExecutor JavaScriptExecutor
        {
            get { return webDriver as IJavaScriptExecutor; }
        }

        public object Native
        {
            get { return webDriver; }
        }

        public IEnumerable<Element> FindFrames(string locator, Scope scope, Options options)
        {
            return frameFinder.FindFrame(locator, scope, options).Select(BuildElement);
        }

        private Func<IWebElement, bool> ValidateTextPattern(Options options, Regex textPattern)
        {
            var textMatches = (textPattern == null)
                                  ? (Func<IWebElement, bool>) null
                                  : e => textMatcher.TextMatches(e, textPattern);

            if (textPattern != null && options.ConsiderInvisibleElements)
                throw new NotSupportedException("Cannot inspect the text of invisible elements.");
            return textMatches;
        }

        public IEnumerable<Element> FindAllCss(string cssSelector, Scope scope, Options options, Regex textPattern = null)
        {
            return FindAll(By.CssSelector(cssSelector), scope, options, ValidateTextPattern(options, textPattern)).Select(BuildElement);
        }

        public IEnumerable<Element> FindAllXPath(string xpath, Scope scope, Options options)
        {
            return FindAll(By.XPath(xpath), scope, options).Select(BuildElement);
        }

        private IEnumerable<IWebElement> FindAll(By by, Scope scope, Options options, Func<IWebElement, bool> predicate = null)
        {
            return elementFinder.FindAll(by, scope, options, predicate);
        }

        private Element BuildElement(IWebElement element)
        {
            return new[] {"iframe", "frame"}.Contains(element.TagName.ToLower()) 
                ? new SeleniumFrame(element, webDriver, seleniumWindowManager) 
                : new SeleniumElement(element, webDriver);
        }

        public bool HasDialog(string withText, Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            return dialogs.HasDialog(withText);
        }

        public void Visit(string url, Scope scope) 
        {
            elementFinder.SeleniumScope(scope);
            webDriver.Navigate().GoToUrl(url);
        }

        public void ClearBrowserCookies()
        {
            webDriver.Manage().Cookies.DeleteAllCookies();
        }

        public void Click(Element element) 
        {
            SeleniumElement(element).Click();
        }

        public void Hover(Element element)
        {
            mouseControl.Hover(element);
        }

        public void SendKeys(Element element, string keys)
        {
            SeleniumElement(element).SendKeys(keys);
        }

        public void MaximiseWindow(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            webDriver.Manage().Window.Maximize();
        }

        public void Refresh(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            webDriver.Navigate().Refresh();
        }

        public void ResizeTo(Size size, Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            webDriver.Manage().Window.Size = size;
        }

        public void SaveScreenshot(string fileName, Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            var format = ImageFormatParser.GetImageFormat(fileName);

            var screenshot = ((ITakesScreenshot) webDriver).GetScreenshot();
            screenshot.SaveAsFile(fileName, format);
        }

        public void GoBack(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            webDriver.Navigate().Back();
        }

        public void GoForward(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            webDriver.Navigate().Forward();
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return webDriver.Manage().Cookies.AllCookies.Select(c => new Cookie(c.Name, c.Value, c.Path, c.Domain));
        }

        public IEnumerable<Element> FindWindows(string titleOrName, Scope scope, Options options)
        {
            elementFinder.SeleniumScope(scope);
            return windowHandleFinder.FindWindowHandles(titleOrName, options)
                                     .Select(h => new SeleniumWindow(webDriver, h, seleniumWindowManager))
                                     .Cast<Element>();
        }

        public void Set(Element element, string value) 
        {
            try
            {
                SeleniumElement(element).Clear();
            }
            catch (InvalidElementStateException) { }// Non user-editable elements (file inputs) - chrome/IE
            catch (InvalidOperationException) {} // Non user-editable elements (file inputs) - firefox
            SendKeys(element, value);
        }

        public void AcceptModalDialog(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            dialogs.AcceptModalDialog();
        }

        public void CancelModalDialog(Scope scope)
        {
            elementFinder.SeleniumScope(scope);
            dialogs.CancelModalDialog();
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
            SeleniumElement(field).Click();
        }

        public object ExecuteScript(string javascript, Scope scope, params object[] args)
        {
            if (NoJavascript)
                throw new NotSupportedException("Javascript is not supported by " + browser);

            elementFinder.SeleniumScope(scope);
            return JavaScriptExecutor.ExecuteScript(javascript, ConvertScriptArgs(args));
        }

        private object[] ConvertScriptArgs (object[] args)
        {
            for(var i = 0; i < args.Length; ++i)
            {
                var argAsElement = args[i] as Element;
                if (argAsElement != null)
                    args[i] = argAsElement.Native;
            }

            return args;
        }

        private string NormalizeCRLFBetweenBrowserImplementations(string text)
        {
            if (webDriver is ChromeDriver) // Which adds extra whitespace around CRLF
                text = StripWhitespaceAroundCRLFs(text);

            return Regex.Replace(text, "(\r\n)+", "\r\n");
        }

        private string StripWhitespaceAroundCRLFs(string pageText)
        {
            return Regex.Replace(pageText, @"\s*\r\n\s*", "\r\n");
        }

        private IWebElement SeleniumElement(Element element)
        {
            return ((IWebElement) element.Native);
        }

        public void Dispose()
        {
            if (webDriver == null)
                return;

            AcceptAnyAlert();

            webDriver.Quit();
            webDriver = null;
            Disposed = true;
        }

        private void AcceptAnyAlert()
        {
            try
            {
                seleniumWindowManager.SwitchToWindow(webDriver.WindowHandles[0]);
                if (dialogs.HasAnyDialog())
                    webDriver.SwitchTo().Alert().Accept();
            }
            catch (WebDriverException){}
            catch (KeyNotFoundException){} // Chrome
            catch (InvalidOperationException){}
            catch (IndexOutOfRangeException){} // No window handles
        }


    }
}