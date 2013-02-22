using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Cookie = System.Net.Cookie;

namespace Coypu.Drivers.Selenium
{
    public class SeleniumWebDriver : Driver
    {
        public bool Disposed { get; private set; }
        private IWebDriver webDriver;
        private readonly ElementFinder elementFinder;
        private readonly FieldFinder fieldFinder;
        private readonly FrameFinder frameFinder;
        private readonly ButtonFinder buttonFinder;
        private readonly SectionFinder sectionFinder;
        private readonly TextMatcher textMatcher;
        private readonly Dialogs dialogs;
        private readonly MouseControl mouseControl;
        private readonly OptionSelector optionSelector;
        private readonly XPath xPath;
        private readonly Browser browser;

        public SeleniumWebDriver(Browser browser)
            : this(new DriverFactory().NewWebDriver(browser),  browser)
        {
            
        }

        public SeleniumWebDriver(Browser browser, IDictionary<Browser, object> browserOptions)
            : this(new DriverFactory().NewWebDriver(browser, browserOptions), browser)
        {
        }

        protected SeleniumWebDriver(IWebDriver webDriver, Browser browser)
        {
            this.webDriver = webDriver;
            this.browser = browser;
            xPath = new XPath();
            elementFinder = new ElementFinder(xPath);
            fieldFinder = new FieldFinder(elementFinder, xPath);
            frameFinder = new FrameFinder(this.webDriver, elementFinder,xPath);
            textMatcher = new TextMatcher();
            buttonFinder = new ButtonFinder(elementFinder, textMatcher, xPath);
            sectionFinder = new SectionFinder(elementFinder, textMatcher);
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
                return new WindowHandle(webDriver, webDriver.CurrentWindowHandle);
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

        public ElementFound FindField(string locator, Scope scope)
        {
            return BuildElement(fieldFinder.FindField(locator,scope), "No such field: " + locator);
        }

        public ElementFound FindButton(string locator, Scope scope)
        {
            return BuildElement(buttonFinder.FindButton(locator, scope), "No such button: " + locator);
        }


        public ElementFound FindFrame(string locator, Scope scope)
        {
            var element = frameFinder.FindFrame(locator, scope);

            if (element == null)
                throw new MissingHtmlException("Failed to find frame: " + locator);

            return new SeleniumFrame(element, webDriver);
        }

        public ElementFound FindLink(string linkText, Scope scope)
        {
            return BuildElement(Find(By.LinkText(linkText), scope).FirstOrDefault(), "No such link: " + linkText);
        }

        public ElementFound FindId(string id,Scope scope ) 
        {
            return BuildElement(Find(By.Id(id), scope).FirstOrDefault(), "Failed to find id: " + id);
        }

        public ElementFound FindFieldset(string locator, Scope scope)
        {
            var fieldset =
                Find(By.XPath(xPath.Format(".//fieldset[legend[text() = {0}]]", locator)),scope).FirstOrDefault() ??
                Find(By.Id(locator),scope).FirstOrDefault(e => e.TagName == "fieldset");

            return BuildElement(fieldset, "Failed to find fieldset: " + locator);
        }

        public ElementFound FindSection(string locator, Scope scope)
        {
            return BuildElement(sectionFinder.FindSection(locator,scope), "Failed to find section: " + locator);
        }

        public ElementFound FindCss(string cssSelector,Scope scope)
        {
            return BuildElement(Find(By.CssSelector(cssSelector),scope).FirstOrDefault(),"No element found by css: " + cssSelector);
        }

        public ElementFound FindXPath(string xpath, Scope scope)
        {
            return BuildElement(Find(By.XPath(xpath),scope).FirstOrDefault(),"No element found by xpath: " + xpath);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope)
        {
            return Find(By.CssSelector(cssSelector),scope).Select(e => BuildElement(e)).Cast<ElementFound>();
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope)
        {
            return Find(By.XPath(xpath), scope).Select(e => BuildElement(e)).Cast<ElementFound>();
        }

        private IEnumerable<IWebElement> Find(By by, Scope scope)
        {
            return elementFinder.Find(by, scope);
        }

        private ElementFound BuildElement(IWebElement element, string failureMessage)
        {
            if (element == null)
                throw new MissingHtmlException(failureMessage);

            return BuildElement(element);
        }

        private SeleniumElement BuildElement(IWebElement element)
        {
            return new SeleniumElement(element);
        }

        public bool HasContent(string text, Scope scope)
        {
            return GetContent(scope).Contains(text);
        }

        public bool HasContentMatch(Regex pattern, Scope scope)
        {
            return pattern.IsMatch(GetContent(scope));
        }

        private string GetContent(Scope scope)
        {
            var seleniumScope = elementFinder.SeleniumScope(scope);
            return seleniumScope is RemoteWebDriver
                       ? GetText(By.CssSelector("body"), seleniumScope)
                       : GetText(By.XPath("."), seleniumScope);
        }

        private string GetText(By xpath, ISearchContext seleniumScope)
        {   
            var pageText = seleniumScope.FindElement(xpath).Text;
            return NormalizeCRLFBetweenBrowserImplementations(pageText);
        }

        public bool HasCss(string cssSelector, Scope scope)
        {
            return Find(By.CssSelector(cssSelector), scope).Any();
        }

        public bool HasXPath(string xpath, Scope scope)
        {
            return Find(By.XPath(xpath), scope).Any();
        }

        public bool HasDialog(string withText, Scope scope)
        {
            return dialogs.HasDialog(withText);
        }

        public void Visit(string url) 
        {
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

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return webDriver.Manage().Cookies.AllCookies.Select(c => new Cookie(c.Name, c.Value, c.Path, c.Domain));
        }

        public ElementFound FindWindow(string titleOrName, Scope scope)
        {
            return new WindowHandle(webDriver, FindWindowHandle(titleOrName));
        }

        private string FindWindowHandle(string titleOrName)
        {
            var currentHandle = GetCurrentWindowHandle();
            string matchingWindowHandle = null;

            try
            {
                webDriver.SwitchTo().Window(titleOrName);
                matchingWindowHandle = webDriver.CurrentWindowHandle;
            }
            catch (NoSuchWindowException)
            {
                foreach (var windowHandle in webDriver.WindowHandles)
                {
                    webDriver.SwitchTo().Window(windowHandle);
                    if (windowHandle == titleOrName || webDriver.Title == titleOrName)
                    {
                        matchingWindowHandle = windowHandle;
                        break;
                    }
                }
            }

            if (matchingWindowHandle == null)
                throw new MissingWindowException("No such window found: " + titleOrName);

            webDriver.SwitchTo().Window(currentHandle);
            return matchingWindowHandle;
        }

        private string GetCurrentWindowHandle()
        {
            try
            {
                return webDriver.CurrentWindowHandle;
            }
            catch(NoSuchWindowException)
            {
                return null;  
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public void Set(Element element, string value, bool forceAllEvents) 
        {
            var seleniumElement = SeleniumElement(element);
            try
            {
                seleniumElement.Clear();
            }
            catch (InvalidElementStateException) // Non user-editable elements (file inputs) - chrome/IE
            {
                seleniumElement.SendKeys(value);
                return;
            }
            catch(InvalidOperationException)  // Non user-editable elements (file inputs) - firefox
            {
                seleniumElement.SendKeys(value);
                return;
            }
            SetByIdOrSendKeys(value, seleniumElement, forceAllEvents);
        }

        private void SetByIdOrSendKeys(string value, IWebElement seleniumElement, bool forceAllEvents)
        {
            var id = seleniumElement.GetAttribute("id");
            if (string.IsNullOrEmpty(id) || forceAllEvents || NoJavascript)
                seleniumElement.SendKeys(value);
            else
                JavaScriptExecutor.ExecuteScript(string.Format("document.getElementById('{0}').value = {1}", id, Newtonsoft.Json.JsonConvert.ToString(value)));
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