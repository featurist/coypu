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

        public Uri Location
        {
            get
            {
                return new Uri(selenium.Url);
            }
        }

        public bool ConsiderInvisibleElements
        {
            get { return elementFinder.ConsiderInvisibleElements; }
            set { elementFinder.ConsiderInvisibleElements = value; }
        }

        private RemoteWebDriver selenium;
        private readonly ElementFinder elementFinder;
        private readonly FieldFinder fieldFinder;
        private readonly IFrameFinder iframeFinder;
        private readonly ButtonFinder buttonFinder;
        private readonly SectionFinder sectionFinder;
        private readonly TextMatcher textMatcher;
        private readonly Dialogs dialogs;
        private readonly MouseControl mouseControl;
        private readonly OptionSelector optionSelector;
        private readonly XPath xPath;

        public SeleniumWebDriver()
            : this(new DriverFactory().NewRemoteWebDriver())
        {
        }

        protected SeleniumWebDriver(RemoteWebDriver webDriver)
        {
            selenium = webDriver;
            xPath = new XPath();
            elementFinder = new ElementFinder(xPath);
            fieldFinder = new FieldFinder(elementFinder, xPath);
            iframeFinder = new IFrameFinder(selenium, elementFinder,xPath);
            textMatcher = new TextMatcher();
            buttonFinder = new ButtonFinder(elementFinder, textMatcher, xPath);
            sectionFinder = new SectionFinder(elementFinder, textMatcher);
            dialogs = new Dialogs(selenium);
            mouseControl = new MouseControl(selenium);
            optionSelector = new OptionSelector();
        }

        public object Native
        {
            get { return selenium; }
        }

        public Element FindField(string locator, DriverScope scope)
        {
            return BuildElement(fieldFinder.FindField(locator,scope), "No such field: " + locator);
        }

        public Element FindButton(string locator, DriverScope scope)
        {
            return BuildElement(buttonFinder.FindButton(locator, scope), "No such button: " + locator);
        }

        public Element FindIFrame(string locator, DriverScope scope) 
        {
            return BuildElement(iframeFinder.FindIFrame(locator, scope), "Failed to find frame: " + locator);
        }

        public Element FindLink(string linkText, DriverScope scope)
        {
            return BuildElement(Find(By.LinkText(linkText), scope).FirstOrDefault(), "No such link: " + linkText);
        }

        public Element FindId(string id,DriverScope scope ) 
        {
            return BuildElement(Find(By.Id(id), scope).FirstDisplayedOrDefault(), "Failed to find id: " + id);
        }

        public Element FindFieldset(string locator, DriverScope scope)
        {
            var fieldset =
                Find(By.XPath(xPath.Format(".//fieldset[legend[text() = {0}]]", locator)),scope).FirstOrDefault() ??
                Find(By.Id(locator),scope).FirstOrDefault(e => e.TagName == "fieldset");

            return BuildElement(fieldset, "Failed to find fieldset: " + locator);
        }

        public Element FindSection(string locator, DriverScope scope)
        {
            return BuildElement(sectionFinder.FindSection(locator), "Failed to find section: " + locator);
        }

        public Element FindCss(string cssSelector,DriverScope scope)
        {
            return BuildElement(Find(By.CssSelector(cssSelector),scope).FirstOrDefault(),"No element found by css: " + cssSelector);
        }

        public Element FindXPath(string xpath, DriverScope scope)
        {
            return BuildElement(Find(By.XPath(xpath),scope).FirstOrDefault(),"No element found by xpath: " + xpath);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector, DriverScope scope)
        {
            return Find(By.CssSelector(cssSelector),scope).Select(e => BuildElement(e)).Cast<Element>();
        }

        public IEnumerable<Element> FindAllXPath(string xpath, DriverScope scope)
        {
            return Find(By.XPath(xpath), scope).Select(e => BuildElement(e)).Cast<Element>();
        }

        private IEnumerable<IWebElement> Find(By by, DriverScope scope)
        {
            return elementFinder.Find(by, scope);
        }

        private Element BuildElement(IWebElement element, string failureMessage)
        {
            if (element == null)
                throw new MissingHtmlException(failureMessage);

            return BuildElement(element);
        }

        private SeleniumElement BuildElement(IWebElement element)
        {
            return new SeleniumElement(element);
        }

        public bool HasContent(string text, DriverScope scope)
        {
            return GetContent(scope).Contains(text);
        }

        public bool HasContentMatch(Regex pattern, DriverScope scope)
        {
            return pattern.IsMatch(GetContent(scope));
        }

        private string GetContent(DriverScope scope)
        {
            return scope is DocumentDriverScope
                       ? GetText(By.XPath("."),scope)
                       : GetText(By.TagName("body"), scope);
        }

        private string GetText(By xPath, DriverScope scope)
        {
            
            var pageText = ElementFinder.SeleniumScope(scope).FindElement(xPath).Text;
            return NormalizeCRLFBetweenBrowserImplementations(pageText);
        }

        public bool HasCss(string cssSelector, DriverScope scope)
        {
            return Find(By.CssSelector(cssSelector), scope).Any();
        }

        public bool HasXPath(string xpath, DriverScope scope)
        {
            return Find(By.XPath(xpath), scope).Any();
        }

        public bool HasDialog(string withText)
        {
            return dialogs.HasDialog(withText);
        }

        public void Visit(string url) 
        {
            selenium.Navigate().GoToUrl(url);
        }

        public void Click(Element element) 
        {
            SeleniumElement(element).Click();
        }

        public void Hover(Element element)
        {
            mouseControl.Hover(element);
        }

        public IEnumerable<Cookie> GetBrowserCookies()
        {
            return selenium.Manage().Cookies.AllCookies.Select(c => new Cookie(c.Name, c.Value, c.Path, c.Domain));
        }

        public void Set(Element element, string value) 
        {
            var seleniumElement = SeleniumElement(element);

            seleniumElement.Clear();
            seleniumElement.SendKeys(value);
        }

        public void Select(Element element, string option)
        {
            optionSelector.Select(element, option);
        }

        public void AcceptModalDialog()
        {
            dialogs.AcceptModalDialog();
        }

        public void CancelModalDialog()
        {
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

        public string ExecuteScript(string javascript)
        {
            var result = selenium.ExecuteScript(javascript);
            return result == null ? null : result.ToString();
        }

        private string NormalizeCRLFBetweenBrowserImplementations(string text)
        {
            if (selenium is ChromeDriver) // Which adds extra whitespace around CRLF
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
            if (selenium == null)
                return;

            AcceptAnyAlert();

            selenium.Close();
            selenium.Dispose();
            selenium = null;
            Disposed = true;
        }

        private void AcceptAnyAlert()
        {
            try
            {
                selenium.SwitchTo().Alert().Accept();
            }
            catch (WebDriverException){}
            catch (KeyNotFoundException){} // Chrome
            catch (InvalidOperationException){}
        }
    }
}