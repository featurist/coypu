using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    public class SeleniumWebDriver : Driver
    {
        public bool Disposed { get; private set; }

        private RemoteWebDriver selenium;
        private readonly SeleniumScoping scoping;
        private readonly SeleniumElementFinder elementFinder;
        private readonly SeleniumFieldFinder fieldFinder;
        private readonly IFrameFinder iframeFinder;
        private readonly ButtonFinder buttonFinder;
        private readonly SectionFinder sectionFinder;
        private readonly TextMatcher textMatcher;

        public SeleniumWebDriver()
        {
            selenium = new SeleniumDriverFactory().NewRemoteWebDriver();
            scoping = new SeleniumScoping(selenium);
            elementFinder = new SeleniumElementFinder(scoping);
            fieldFinder = new SeleniumFieldFinder(elementFinder);
            iframeFinder = new IFrameFinder(selenium, elementFinder);
            textMatcher = new TextMatcher();
            buttonFinder = new ButtonFinder(elementFinder, textMatcher);
            sectionFinder = new SectionFinder(selenium, elementFinder,textMatcher);
        }

        public object Native
        {
            get { return selenium; }
        }

        private ISearchContext Scope
        {
            get { return scoping.Scope; }
        }

        private bool ScopeDefined
        {
            get { return scoping.ScopeDefined(); }
        }

        public void SetScope(Func<Element> find)
        {
            scoping.SetScope(find);
        }

        public void ClearScope()
        {
            scoping.ClearScope();
        }

        public Element FindField(string locator)
        {
            return BuildElement(fieldFinder.FindField(locator), "No such field: " + locator);
        }

        public Element FindButton(string locator)
        {
            return BuildElement(buttonFinder.FindButton2(locator),"No such button: " + locator);
        }

        public Element FindIFrame(string locator) 
        {
            return BuildElement(iframeFinder.FindIFrame(locator), "Failed to find frame: " + locator);
        }

        public Element FindLink(string locator)
        {
            return BuildElement(Find(By.LinkText(locator)).FirstOrDefault(), "No such link: " + locator);
        }

        public Element FindId(string id) 
        {
            return BuildElement(Find(By.Id(id)).FirstDisplayedOrDefault(), "Failed to find id: " + id);
        }

        public Element FindFieldset(string locator)
        {
            var fieldset =
                Find(By.XPath(string.Format(".//fieldset[legend[text() = \"{0}\"]]", locator))).FirstOrDefault() ??
                Find(By.Id(locator)).FirstOrDefault(e => e.TagName == "fieldset");

            return BuildElement(fieldset, "Failed to find fieldset: " + locator);
        }

        public Element FindSection(string locator)
        {
            return BuildElement(sectionFinder.FindSection(locator), "Failed to find section: " + locator);
        }

        public Element FindCss(string cssSelector)
        {
            return BuildElement(Find(By.CssSelector(cssSelector)).FirstOrDefault(),"No element found by css: " + cssSelector);
        }

        public Element FindXPath(string xpath)
        {
            return BuildElement(Find(By.XPath(xpath)).FirstOrDefault(),"No element found by xpath: " + xpath);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return Find(By.CssSelector(cssSelector)).Select(e => BuildElement(e)).Cast<Element>();
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            return Find(By.XPath(xpath)).Select(e => BuildElement(e)).Cast<Element>();
        }

        private IEnumerable<IWebElement> Find(By by)
        {
            return elementFinder.Find(by);
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

        public bool HasContent(string text)
        {
            return GetContent().Contains(text);
        }

        public bool HasContentMatch(Regex pattern)
        {
            return pattern.IsMatch(GetContent());
        }

        private string GetContent()
        {
            return ScopeDefined
                       ? GetText(".")
                       : GetText("/html/body");
        }

        private string GetText(string xpath)
        {
            var pageText = Scope.FindElement(By.XPath(xpath)).Text;
            return NormalizeCRLFBetweenBrowserImplementations(pageText);
        }

        public bool HasCss(string cssSelector)
        {
            return Find(By.CssSelector(cssSelector)).Any();
        }

        public bool HasXPath(string xpath)
        {
            return Find(By.XPath(xpath)).Any();
        }

        public bool HasDialog(string withText)
        {
            return selenium.SwitchTo() != null &&
                   selenium.SwitchTo().Alert() != null &&
                   selenium.SwitchTo().Alert().Text == withText;
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
            var sequenceBuilder = new DefaultActionSequenceBuilder(selenium);
            var actionSequenceBuilder = sequenceBuilder.MoveToElement((IWebElement) element.Native);
            var action = actionSequenceBuilder.Build();
            action.Perform();
        }

        public void Set(Element element, string value) 
        {
            var seleniumElement = SeleniumElement(element);

            seleniumElement.Clear();
            seleniumElement.SendKeys(value);
        }

        public void Select(Element element, string option)
        {
            var select = SeleniumElement(element);
            var optionToSelect =
                select.FindElements(By.TagName("option"))
                    .FirstOrDefault(e => e.Text == option || e.GetAttribute("value") == option);

            if (optionToSelect == null)
                throw new MissingHtmlException("No such option: " + option);

            optionToSelect.Select();
        }

        public void AcceptModalDialog()
        {
            selenium.SwitchTo().Alert().Accept();
        }

        public void CancelModalDialog()
        {
            selenium.SwitchTo().Alert().Dismiss();
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