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
		public bool Disposed { get; private set; }

		private RemoteWebDriver selenium;
		private Func<Element> findScope;
	    private bool findingScope;

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
	            return (ISearchContext) findScope().Native;
	        }
	        finally
	        {
	            findingScope = false;
	        }
	    }

	    public void ClearScope()
		{
			findScope = null;
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
				return BuildElement(FindButtonByText(locator) ??
								    FindInputButtonByValue(locator) ??
								    Find(By.Id(locator)).FirstDisplayedOrDefault(IsButton) ??
								    Find(By.Name(locator)).FirstDisplayedOrDefault(IsButton),
									  "No such button: " + locator);
			}
			catch (NoSuchElementException e)
			{
				throw new MissingHtmlException(e.Message, e);
			}
		}

		private IWebElement FindInputButtonByValue(string locator)
		{
			var inputButtonsCss = string.Format("input[type=button],input[type=submit]");

			return Find(By.CssSelector(inputButtonsCss)).FirstDisplayedOrDefault(e => e.Value == locator);
		}

		private IWebElement FindButtonByText(string locator)
		{
			return Find(By.TagName(string.Format("button"))).FirstDisplayedOrDefault(e => e.Text == locator);
		}

		public Element FindLink(string locator)
		{
			try
			{
				return BuildElement(Find(By.LinkText(locator)).FirstDisplayedOrDefault(), "No such link: " + locator);
			}
			catch (NoSuchElementException e)
			{
				throw new MissingHtmlException(e.Message, e);
			}
		}

		public Element FindField(string locator)
		{
			var field = (FindFieldById(locator) ??
						 FindFieldByName(locator)) ??
						 FindFieldFromLabel(locator) ??
						 FindFieldByPlaceholder(locator) ??
						 FindRadioButtonFromValue(locator);

			return BuildElement(field, "No such field: " + locator);
		}

		private IWebElement FindRadioButtonFromValue(string locator)
		{
			return Scope.FindElements(By.XPath(".//input[@type = 'radio']")).FirstDisplayedOrDefault(e => e.Value == locator);
		}

		private IWebElement FindLabelByText(string locator)
		{
			return
				Scope.FindElements(By.XPath(string.Format(".//label[text() = \"{0}\"]", locator))).FirstOrDefault() ??
				Scope.FindElements(By.XPath(string.Format(".//label[contains(text(),\"{0}\")]", locator))).FirstOrDefault();
		}

		private IWebElement FindFieldFromLabel(string locator)
		{
			var label = FindLabelByText(locator);
			if (label == null)
				return null;

			var id = label.GetAttribute("for");

			var field = id != null 
            	? FindFieldById(id) 
            	: label.FindElements(By.XPath("*")).FirstDisplayedOrDefault(IsField);

			return field;

		}

		private IWebElement FindFieldByPlaceholder(string placeholder)
		{
			return Scope.FindElements(By.XPath(string.Format(".//input[@placeholder = \"{0}\"]", placeholder)))
						   .FirstDisplayedOrDefault(IsField);
		}

		private IWebElement FindFieldById(string id)
		{
			return Scope.FindElements(By.Id(id)).FirstDisplayedOrDefault(IsField);
		}

		private IWebElement FindFieldByName(string name)
		{
			return Scope.FindElements(By.Name(name)).FirstDisplayedOrDefault(IsField);
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
			{
				throw new MissingHtmlException("No such option: " + option);
			}
			optionToSelect.Select();
		}

		public bool HasContent(string text)
		{
			return PageText().Contains(text);
		}

		public bool HasCss(string cssSelector)
		{
			return Find(By.CssSelector(cssSelector)).AnyDisplayed();
		}

		public bool HasXPath(string xpath)
		{
			return Find(By.XPath(xpath)).AnyDisplayed();
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
			return BuildElement(Find(By.CssSelector(cssSelector)).FirstDisplayedOrDefault(),
							 "No element found by css: " + cssSelector);
		}

		public Element FindXPath(string xpath)
		{
			return BuildElement(Find(By.XPath(xpath)).FirstDisplayedOrDefault(),
                             "No element found by xpath: " + xpath);
		}

		public IEnumerable<Element> FindAllCss(string cssSelector)
		{
			return Find(By.CssSelector(cssSelector))
					   .Where(e => e.Displayed())
					   .Select(e => BuildElement(e))
					   .Cast<Element>();
		}

		public IEnumerable<Element> FindAllXPath(string xpath)
		{
			return Find(By.XPath(xpath))
					   .Where(e => e.Displayed())
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

		private string PageText()
		{
			var pageText = selenium.FindElement(By.CssSelector("html body")).Text;

			pageText = NormalizeCRLFBetweenBrowserImplementations(pageText);

			return pageText;
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
			var inputButtonTypes = new[] {"button", "submit", "image"};
			return e.TagName == "input" && inputButtonTypes.Contains(e.GetAttribute("type"));
		}

		private bool IsField(IWebElement e)
		{
			return IsInputField(e) || e.TagName == "select" || e.TagName == "textarea";
		}

		private bool IsInputField(IWebElement e)
		{
			return e.TagName == "input" &&
				   (HasAttr(e, "type", "text") ||
					HasAttr(e, "type", "password") ||
					HasAttr(e, "type", "radio") ||
					HasAttr(e, "type", "checkbox"));
		}

		private bool HasAttr(IWebElement e, string attributeName, string value)
		{
			return e.GetAttribute(attributeName) == value;
		}

		private IEnumerable<IWebElement> Find(By by)
		{
			return Scope.FindElements(by);
		}

		public void Dispose()
		{
			if (selenium == null) 
				return;

			selenium.Close();
			selenium.Dispose();
			selenium = null;
			Disposed = true;
		}
	}
}