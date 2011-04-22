using System;
using System.Collections.Generic;
using System.Linq;
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

		private Node BuildNode(IWebElement element, string description)
		{
			try
			{
				return new SeleniumNode(element);
			}
			catch (NullReferenceException)
			{
				throw new MissingHtmlException("Failed to " + description);
			}
		}

		public Node FindButton(string locator)
		{
			try
			{
				return BuildNode(FindButtonByText(locator) ??
                                 FindInputButtonByValue(locator) ??
                                 Find(By.Id(locator)).FirstVisibleOrDefault(IsInputButton) ??
                                 Find(By.Name(locator)).FirstVisibleOrDefault(IsInputButton),
								 "find button: " + locator);
			}
			catch (NoSuchElementException e)
			{
				throw new MissingHtmlException(e.Message, e);
			}
		}

		private IWebElement FindInputButtonByValue(string locator)
		{
			var inputButtonsCss = string.Format("input[type=button],input[type=submit]");

			return Find(By.CssSelector(inputButtonsCss)).FirstVisibleOrDefault(e => e.Value == locator);
		}

		private IWebElement FindButtonByText(string locator)
		{
			return Find(By.TagName(string.Format("button"))).FirstVisibleOrDefault(e => e.Text == locator);
		}

		public Node FindLink(string locator)
		{
			try
			{
				return BuildNode(selenium.FindElement(By.LinkText(locator)), "find link: " + locator);
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
			return BuildNode(field, "find text field: " + locator);
		}

		private IWebElement FindLabelByText(string locator)
		{
			var labels = selenium.FindElements(By.TagName("label"));

			return labels.FirstVisibleOrDefault(e => e.Text == locator) ??
				   labels.FirstVisibleOrDefault(e => e.Text.StartsWith(locator));
		}

		private IWebElement FindFieldFromLabel(IWebElement label)
		{
			return FindFieldById(label.GetAttribute("for")) ??
			       label.FindElements(By.XPath("*")).FirstVisibleOrDefault(IsField);
		}

		private IWebElement FindFieldByPlaceholder(string placeholder)
		{
			return selenium.FindElements(By.XPath("//input[@placeholder]"))
					.Where(IsField)
					.FirstVisibleOrDefault(e => e.GetAttribute("placeholder") == placeholder);
		}

		private IWebElement FindFieldById(string id)
		{
			return selenium.FindElementsById(id).FirstVisibleOrDefault(IsField);
		}

		private IWebElement FindFieldByName(string name)
		{
			return selenium.FindElementsByName(name).FirstVisibleOrDefault(IsField);
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
			
			if (!SetSelectedOption(seleniumElement, value))
			{
				seleniumElement.Clear();
				seleniumElement.SendKeys(value);
			}
			node.Update();
		}

		private bool SetSelectedOption(IWebElement seleniumElement, string value)
		{
			var optionToSelect = seleniumElement.FindElements(By.TagName("option"))
				.FirstOrDefault(e => e.Text == value || e.Value == value);
			if (optionToSelect != null)
			{
				optionToSelect.Select();
				return true;
			}
			return false;
		}

		public object Native
		{
			get { return selenium; }
		}

		private IWebElement SeleniumElement(Node node)
		{
			return ((IWebElement) node.Native);
		}

		private bool IsInputButton(IWebElement e)
		{
			return e.TagName == "button" ||
			       (e.TagName == "input" && (HasAttr(e, "type", "button") || HasAttr(e, "type", "submit")));
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