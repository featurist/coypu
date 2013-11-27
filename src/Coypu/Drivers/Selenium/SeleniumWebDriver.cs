using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
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
            fieldFinder = new FieldFinder(elementFinder, xPath);
            frameFinder = new FrameFinder(this.webDriver, elementFinder,xPath);
            textMatcher = new TextMatcher();
            buttonFinder = new ButtonFinder(elementFinder, xPath);
            sectionFinder = new SectionFinder(elementFinder, xPath);
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

        public ElementFound FindField(string locator, Scope scope)
        {
            return BuildElement(fieldFinder.FindField(locator, scope));
        }

        public ElementFound FindButton(string locator, Scope scope)
        {
            return BuildElement(buttonFinder.FindButton(locator, scope));
        }

        public ElementFound FindFrame(string locator, Scope scope)
        {
            return BuildElement(frameFinder.FindFrame(locator, scope));
        }

        public ElementFound FindLink(string linkText, Scope scope)
        {
            return BuildElement(Find(
                new [] {
                     By.LinkText(linkText),
                     By.PartialLinkText(linkText),
                    }, scope, "link: " + linkText));
        }

        public ElementFound FindId(string id,Scope scope ) 
        {
            return BuildElement(Find(By.Id(id), scope, "id: " + id));
        }

        public ElementFound FindFieldset(string locator, Scope scope)
        {
            var by = By.XPath(xPath.Format(".//fieldset[legend[text() = {0}] or @id = {0}]", locator.Trim()));
            var fieldset = Find(by, scope, "fieldset: " + locator);

            return BuildElement(fieldset);
        }

        public ElementFound FindSection(string locator, Scope scope)
        {
            return BuildElement(sectionFinder.FindSection(locator,scope));
        }

        public ElementFound FindCss(string cssSelector, Scope scope, Regex textPattern = null)
        {
            var textMatches = (textPattern == null)
                ? (Func<IWebElement, bool>) null
                : e => textMatcher.TextMatches(e, textPattern);

            if (textPattern != null && scope.ConsiderInvisibleElements)
                throw new NotSupportedException("Cannot inspect the text of invisible elements.");
            
            var queryDesciption = "css: " + cssSelector;
            if (textPattern != null)
                queryDesciption += " with text matching /" + textPattern + "/";

            return BuildElement(Find(By.CssSelector(cssSelector), scope, queryDesciption, textMatches));
        }

        public ElementFound FindXPath(string xpath, Scope scope)
        {
            return BuildElement(Find(By.XPath(xpath), scope, "xpath: " + xpath));
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Scope scope)
        {
            return FindAll(By.CssSelector(cssSelector),scope).Select(BuildElement).Cast<ElementFound>();
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Scope scope)
        {
            return FindAll(By.XPath(xpath), scope).Select(BuildElement).Cast<ElementFound>();
        }

        private IWebElement Find(By by, Scope scope, string queryDescription, Func<IWebElement, bool> predicate = null)
        {
            return elementFinder.Find(by, scope, queryDescription, predicate);
        }

        private IWebElement Find(IEnumerable<By> by, Scope scope, string queryDescription, Func<IWebElement, bool> predicate = null)
        {
            return elementFinder.Find(by, scope, queryDescription, predicate);
        }

        private IEnumerable<IWebElement> FindAll(By by, Scope scope)
        {
            return elementFinder.FindAll(by, scope);
        }

        private SeleniumElement BuildElement(IWebElement element)
        {
            if (new[] {"iframe", "frame"}.Contains(element.TagName.ToLower()))
            {
                return new SeleniumFrame(element, webDriver);
            }
            return new SeleniumElement(element, webDriver);
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

        public bool HasXPath(string xpath, Scope scope)
        {
            return FindAll(By.XPath(xpath), scope).Any();
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

        public ElementFound FindWindow(string titleOrName, Scope scope)
        {
            return new SeleniumWindow(webDriver, windowHandleFinder.FindWindowHandle(titleOrName));
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