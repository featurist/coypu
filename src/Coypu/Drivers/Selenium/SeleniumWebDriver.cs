using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
	public class SeleniumWebDriver : Driver
	{
		private readonly RemoteWebDriver selenium;

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

		private Node BuildNode(IWebElement element, string failureMessage)
		{
			if (element == null)
				throw new MissingHtmlException(failureMessage);

			return new SeleniumNode(element);
		}

		public Node FindButton(string locator)
		{
			try
			{
				return BuildNode(FindButtonByText(locator) ??
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

		public Node FindLink(string locator)
		{
			try
			{
				return BuildNode(selenium.FindElement(By.LinkText(locator)), "No such link: " + locator);
			}
			catch (NoSuchElementException e)
			{
				throw new MissingHtmlException(e.Message, e);
			}
		}

		public Node FindField(string locator)
		{
			IWebElement field;
			var label = FindLabelByText(locator);
			if (label != null)
			{
				field = FindFieldFromLabel(label);
			}
			else
			{
				field = FindFieldByPlaceholder(locator) ??
				        FindFieldById(locator) ??
				        FindFieldByName(locator);
			}
			return BuildNode(field, "No such field: " + locator);
		}

		private IWebElement FindLabelByText(string locator)
		{
			var labels = selenium.FindElements(By.TagName("label"));

			return labels.FirstOrDefault(e => e.Text == locator) ??
			       labels.FirstOrDefault(e => e.Text.StartsWith(locator));
		}

		private IWebElement FindFieldFromLabel(IWebElement label)
		{
			return FindFieldById(label.GetAttribute("for")) ??
			       label.FindElements(By.XPath("*")).FirstDisplayedOrDefault(IsField);
		}

		private IWebElement FindFieldByPlaceholder(string placeholder)
		{
			return selenium.FindElements(By.XPath("//input[@placeholder]"))
				.Where(IsField)
				.FirstDisplayedOrDefault(e => e.GetAttribute("placeholder") == placeholder);
		}

		private IWebElement FindFieldById(string id)
		{
			return selenium.FindElementsById(id).FirstDisplayedOrDefault(IsField);
		}

		private IWebElement FindFieldByName(string name)
		{
			return selenium.FindElementsByName(name).FirstDisplayedOrDefault(IsField);
		}

		public void Click(Node node)
		{
			SeleniumElement(node).Click();
		}

		public void Visit(string url)
		{
			selenium.Navigate().GoToUrl(url);
		}

		public void Set(Node node, string value)
		{
			var seleniumElement = SeleniumElement(node);

			seleniumElement.Clear();
			seleniumElement.SendKeys(value);
		}

		public void Select(Node node, string option)
		{
			var select = SeleniumElement(node);
			var optionToSelect = select.FindElements(By.TagName("option"))
									   .FirstOrDefault(e => e.Text == option || e.Value == option);
			if (optionToSelect == null)
			{
				throw new MissingHtmlException("No such option: " + option);
			}
			optionToSelect.Select();
		}

		public object Native
		{
			get { return selenium; }
		}

		public bool HasContent(string text)
		{
			return PageText().Contains(text);
		}

		public bool HasCss(string cssSelector)
		{
			throw new NotImplementedException();
		}

		public bool HasXPath(string xpath)
		{
			throw new NotImplementedException();
		}

		private string PageText()
		{
			var pageText = selenium.FindElement(By.CssSelector("html body")).Text;

			if (selenium is ChromeDriver) // Which adds extra whitespace around CRLF
				pageText = StripWhitespaceAroundCRLFs(pageText);

			return pageText;
		}

		private string StripWhitespaceAroundCRLFs(string pageText)
		{
			return Regex.Replace(pageText, @"\s*\r\n\s*", "\r\n");
		}

		private IWebElement SeleniumElement(Node node)
		{
			return ((IWebElement) node.Native);
		}

		private bool IsButton(IWebElement e)
		{
			return e.TagName == "button" || IsInputButton(e);
		}

		private bool IsInputButton(IWebElement e)
		{
			var inputButtonTypes = new []{"button","submit","image"};
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
			return selenium.FindElements(by);
		}

		public void Dispose()
		{
			selenium.Close();
			selenium.Dispose();
		}
	}
}