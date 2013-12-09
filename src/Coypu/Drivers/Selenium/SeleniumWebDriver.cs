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
    public class SeleniumWebDriver : Driver
    {
        public bool Disposed { get; private set; }
        private IWebDriver webDriver;
        private readonly ElementFinder elementFinder;
        private readonly FrameFinder frameFinder;
        private readonly TextMatcher textMatcher;
        private readonly Dialogs dialogs;
        private readonly MouseControl mouseControl;
        private readonly OptionSelector optionSelector;
        private readonly XPath xPath;
        private readonly Browser browser;
        private readonly WindowHandleFinder windowHandleFinder;

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
            frameFinder = new FrameFinder(this.webDriver, elementFinder,xPath);
            textMatcher = new TextMatcher();
            windowHandleFinder = new WindowHandleFinder(this.webDriver);
            dialogs = new Dialogs(this.webDriver);
            mouseControl = new MouseControl(this.webDriver);
            optionSelector = new OptionSelector();
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

        public ElementFound Window
        {
            get
            {
                return new SeleniumWindow(webDriver, webDriver.CurrentWindowHandle);
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

        public IEnumerable<ElementFound> FindFrames(string locator, Scope scope, Options options)
        {
            return frameFinder.FindFrame(locator, scope, options).Select(BuildElement);
        }

        public IEnumerable<ElementFound> FindLinks(string linkText, Scope scope, Options options)
        {
            var by = options.Exact ? 
                By.LinkText(linkText) : 
                By.PartialLinkText(linkText);

            return FindAll(by, scope, options).Select(BuildElement);
        }

        public IEnumerable<ElementFound> FindId(string id, Scope scope, Options options)
        {
            return FindAll(By.Id(id), scope, options).Select(BuildElement);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope, Options options, Regex textPattern = null)
        {
            var textMatches = (textPattern == null)
                ? (Func<IWebElement, bool>)null
                : e => textMatcher.TextMatches(e, textPattern);

            if (textPattern != null && options.ConsiderInvisibleElements)
                throw new NotSupportedException("Cannot inspect the text of invisible elements.");

            return FindAll(By.CssSelector(cssSelector), scope, options, textMatches).Select(BuildElement);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope, Options options)
        {
            return FindAll(By.XPath(xpath), scope, options).Select(BuildElement);
        }

        private IEnumerable<IWebElement> FindAll(By by, Scope scope, Options options, Func<IWebElement, bool> predicate = null)
        {
            return elementFinder.FindAll(by, scope, options, predicate);
        }

        private ElementFound BuildElement(IWebElement element)
        {
            return new[] {"iframe", "frame"}.Contains(element.TagName.ToLower()) 
                ? new SeleniumFrame(element, webDriver) 
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

        public IEnumerable<ElementFound> FindWindows(string titleOrName, Scope scope, Options options)
        {
            elementFinder.SeleniumScope(scope);
            return windowHandleFinder.FindWindowHandles(titleOrName, options.Exact)
                                     .Select(h => new SeleniumWindow(webDriver, h))
                                     .Cast<ElementFound>();
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

        public void Select(Element element, string option)
        {
            optionSelector.Select(element, option);
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

        public string ExecuteScript(string javascript, Scope scope)
        {
            if (NoJavascript)
                throw new NotSupportedException("Javascript is not supported by " + browser);

            elementFinder.SeleniumScope(scope);
            var result = JavaScriptExecutor.ExecuteScript(javascript);
            return result == null ? null : result.ToString();
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
                webDriver.SwitchTo().Alert().Accept();
            }
            catch (WebDriverException){}
            catch (KeyNotFoundException){} // Chrome
            catch (InvalidOperationException){}
        }
    }
}