using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    public class SeleniumWebDriver : Driver
    {
        private static readonly string [] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file" };
        private static readonly string[] InputButtonTypes = new[] { "button", "submit", "image" };

        public bool Disposed { get; private set; }

        private RemoteWebDriver selenium;
        private Func<Element> findScope;
        private bool findingScope;
        private ISearchContext cachedScope;

        public SeleniumWebDriver()
        {
            selenium = NewRemoteWebDriver();
        }

        private RemoteWebDriver NewRemoteWebDriver()
        {
            switch (Configuration.Browser)
            {
                case (Browser.Firefox):
                    return new FirefoxDriver();
                case (Browser.InternetExplorer):
                    return new InternetExplorerDriver();
                case (Browser.Chrome):
                    return new ChromeDriver();
                default:
                    throw new BrowserNotSupportedException(Configuration.Browser, this);
            }
        }

        public object Native
        {
            get { return selenium; }
        }

        public void SetScope(Func<Element> find)
        {
            findScope = find;
        }

        private ISearchContext Scope
        {
            get
            {
                return findScope == null || findingScope
                       ? selenium
                       : FindScope();
            }
        }

        private ISearchContext FindScope()
        {
            findingScope = true;
            try
            {
                return cachedScope == null
                    ? (cachedScope = FindFreshScope()) 
                    : (cachedScope = CheckForStaleElement());
            }
            finally
            {
                findingScope = false;
            }
        }

        private ISearchContext FindFreshScope()
        {
            var findFreshScope = (IWebElement)findScope().Native;
            if (findFreshScope.TagName == "iframe")
            {
                selenium.SwitchTo().Frame(findFreshScope);
                return selenium;
            }

            return findFreshScope;
        }

        private ISearchContext CheckForStaleElement()
        {
            try
            {
                cachedScope.FindElement(By.XPath("."));
                return cachedScope;
            }
            catch (StaleElementReferenceException)
            {
                Console.WriteLine("Stale Element: " + cachedScope);
                return FindFreshScope();
            }
        }

        public void ClearScope()
        {
            cachedScope = null;
            findScope = null;
        }

        public string ExecuteScript(string javascript)
        {
            var result = selenium.ExecuteScript(javascript);
            return result == null ? null : result.ToString();
        }

        public Element FindFieldset(string locator)
        {
            var fieldset = Find(By.XPath(string.Format(".//fieldset[legend[text() = \"{0}\"]]", locator))).FirstOrDefault() ??
                           Find(By.Id(locator)).FirstOrDefault(e => e.TagName == "fieldset");

            return BuildElement(fieldset, "Failed to find fieldset: " + locator);
        }

        public Element FindSection(string locator)
        {
            var scope = Scope;
            var section = FindSectionByHeaderText(locator, scope) ??
                          Find(By.Id(locator), scope).FirstDisplayedOrDefault(IsSection);

            return BuildElement(section, "Failed to find section: " + locator);
        }

        public Element FindId(string id)
        {
            return BuildElement(Find(By.Id(id)).FirstDisplayedOrDefault(), "Failed to find id: " + id);
        }

        public Element FindIFrame(string locator)
        {
            var frame = Find(By.TagName("iframe")).FirstOrDefault(e => e.GetAttribute("id") == locator ||
                                                                       e.GetAttribute("title") == locator ||
                                                                       FindFrameByContents(e, locator));
            return BuildElement(frame, "Failed to find frame: " + locator);
        }

        public void Hover(Element element)
        {
            var sequenceBuilder = new OpenQA.Selenium.Interactions.DefaultActionSequenceBuilder(selenium);
            sequenceBuilder.MoveToElement((IWebElement) element.Native);
        }

        private bool FindFrameByContents(IWebElement e, string locator)
        {
            try
            {
                var frame = selenium.SwitchTo().Frame(e);
                return 
                    frame.Title == locator ||
                    frame.FindElements(By.XPath(string.Format(".//h1[text() = \"{0}\"]", locator))).Any();
            }
            finally
            {
                selenium.SwitchTo().DefaultContent();    
            }
            
        }

        private IWebElement FindSectionByHeaderText(string locator, ISearchContext scope)
        {
            return FindSectionByHeaderText(locator, scope, "section").FirstOrDefault() ??
                   FindSectionByHeaderText(locator, scope, "div").FirstOrDefault();
        }

        private IEnumerable<IWebElement> FindSectionByHeaderText(string locator, ISearchContext scope, string sectionTag)
        {
            string[] headerTags = { "h1", "h2", "h3", "h4", "h5", "h6" };
            var matchAnyHeaderWithText = string.Join(string.Format(" = \"{0}\" or ", locator), headerTags) +
                                         string.Format(" = \"{0}\"", locator);

            return Find(By.XPath(string.Format(".//{0}[{1}]", sectionTag, matchAnyHeaderWithText)), scope);
        }

        private bool IsSection(IWebElement e)
        {
            return e.TagName == "section" || e.TagName == "div";
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

        public Element FindButton(string locator)
        {
            try
            {
                var scope = Scope;
                return BuildElement(FindButtonByText(locator, scope) ??
                                    FindButtonByIdNameOrValue(locator, scope),
                                      "No such button: " + locator);
            }
            catch (NoSuchElementException e)
            {
                throw new MissingHtmlException(e.Message, e);
            }
        }

        private IWebElement FindButtonByIdNameOrValue(string locator, ISearchContext scope)
        {
            var xpathToFind = string.Format(".//*[@id = \"{0}\" or @name = \"{0}\" or @value = \"{0}\"]", locator);
            return Find(By.XPath(xpathToFind), scope).FirstOrDefault(IsButton);
        }

        private IWebElement FindButtonByText(string locator, ISearchContext scope)
        {
            return
                Find(By.TagName("button"), scope).FirstOrDefault(e => TextMatches(e, locator)) ??
                Find(By.ClassName("button"), scope).FirstOrDefault(e => TextMatches(e, locator));
        }

        private bool TextMatches(IWebElement e, string locator)
        {
            return e.Text.Trim() == locator.Trim();
        }

        public Element FindLink(string locator)
        {
            try
            {
                return BuildElement(Find(By.LinkText(locator)).FirstOrDefault(), "No such link: " + locator);
            }
            catch (NoSuchElementException e)
            {
                throw new MissingHtmlException(e.Message, e);
            }
        }

        public Element FindField(string locator)
        {
            var scope = Scope;
            var field = (FindFieldByIdOrName(locator, scope) ?? 
                         FindFieldFromLabel(locator, scope) ?? 
                         FindFieldByPlaceholder(locator, scope) ??
                         FindRadioButtonFromValue(locator, scope));

            return BuildElement(field, "No such field: " + locator);
        }

        private IWebElement FindRadioButtonFromValue(string locator, ISearchContext scope)
        {
            return Find(By.XPath(".//input[@type = 'radio']"),scope).FirstOrDefault(e => e.Value == locator);
        }

        private IWebElement FindFieldFromLabel(string locator, ISearchContext scope)
        {
            var label = FindLabelByText(locator, scope);
            if (label == null)
                return null;

            var id = label.GetAttribute("for");

            var field = id != null 
                ? FindFieldById(id, scope) 
                : label.FindElements(By.XPath("*")).FirstDisplayedOrDefault(IsField);

            return field;

        }

        private IWebElement FindLabelByText(string locator, ISearchContext scope)
        {
            return
                Find(By.XPath(string.Format(".//label[text() = \"{0}\"]", locator)),scope).FirstOrDefault() ??
                Find(By.XPath(string.Format(".//label[contains(text(),\"{0}\")]", locator)), scope).FirstOrDefault();
        }

        private IWebElement FindFieldByPlaceholder(string placeholder, ISearchContext scope)
        {
            return Find(By.XPath(string.Format(".//input[@placeholder = \"{0}\"]", placeholder)), scope).FirstOrDefault(IsField);
        }

        private IWebElement FindFieldByIdOrName(string locator, ISearchContext scope)
        {
            var xpathToFind = string.Format(".//*[@id = \"{0}\" or @name = \"{0}\"]", locator);
            return Find(By.XPath(xpathToFind), scope).FirstOrDefault(IsField);
        }

        private IWebElement FindFieldById(string id, ISearchContext scope)
        {
            return Find(By.Id(id), scope).FirstOrDefault(IsField);
        }


        public void Click(Element element)
        {
            SeleniumElement(element).Click();
        }

        public void Visit(string url)
        {
            selenium.Navigate().GoToUrl(url);
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
                      .FirstOrDefault(e => e.Text == option || e.Value == option);

            if (optionToSelect == null)
                throw new MissingHtmlException("No such option: " + option);

            optionToSelect.Select();
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
            return findScope != null
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

        public void AcceptModalDialog()
        {
            selenium.SwitchTo().Alert().Accept();
        }

        public void CancelModalDialog()
        {
            selenium.SwitchTo().Alert().Dismiss();
        }


        public Element FindCss(string cssSelector)
        {
            return BuildElement(Find(By.CssSelector(cssSelector)).FirstOrDefault(),
                             "No element found by css: " + cssSelector);
        }

        public Element FindXPath(string xpath)
        {
            return BuildElement(Find(By.XPath(xpath)).FirstOrDefault(),
                             "No element found by xpath: " + xpath);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return Find(By.CssSelector(cssSelector))
                       .Select(e => BuildElement(e))
                       .Cast<Element>();
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            return Find(By.XPath(xpath))
                       .Select(e => BuildElement(e))
                       .Cast<Element>();
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

        private bool IsButton(IWebElement e)
        {
            return e.TagName == "button" || IsInputButton(e);
        }

        private bool IsInputButton(IWebElement e)
        {
            return e.TagName == "input" && InputButtonTypes.Contains(e.GetAttribute("type"));
        }

        private bool IsField(IWebElement e)
        {
            return IsInputField(e) || e.TagName == "select" || e.TagName == "textarea";
        }

        private bool IsInputField(IWebElement e)
        {
            return e.TagName == "input" && FieldInputTypes.Contains(e.GetAttribute("type"));
        }

        private IEnumerable<IWebElement> Find(By by)
        {
            return Find(by, Scope);
        }

        private IEnumerable<IWebElement> Find(By by, ISearchContext scope)
        {
            return scope.FindElements(by).Where(IsDisplayed);
        }

        private bool IsDisplayed(IWebElement e)
        {
            return e.IsDisplayed();
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
            catch(WebDriverException){}
            catch(KeyNotFoundException){} // Chrome
            catch(InvalidOperationException) {}

        }
    }
}
