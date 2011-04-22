using System;
using System.Collections.Generic;
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
			return FindNode(() => FindButtonByText(locator) ??
		                          FindInputButtonByValue(locator) ??
		                          find(By.Id(locator)).FirstVisibleOrDefault(IsInputButton) ??
		                          find(By.Name(locator)).FirstVisibleOrDefault(IsInputButton),
								  "find button: " + locator);
		}

		private IWebElement FindInputButtonByValue(string locator)
		{
			var inputButtonsCss = string.Format("input[type=button],input[type=submit]");

			return find(By.CssSelector(inputButtonsCss)).FirstVisibleOrDefault(e => e.Value == locator);
		}

		private IWebElement FindButtonByText(string locator)
		{
			return find(By.TagName(string.Format("button"))).FirstVisibleOrDefault(e => e.Text == locator);
		}

		public Node FindLink(string locator)
		{
			return FindNode(() => selenium.FindElement(By.LinkText(locator)), "finding link: " + locator);
		}

		private Node FindNode(Func<IWebElement> findNode, string description)
		{
			try
			{
				return BuildNode(findNode(), description);
			}
			catch (NoSuchElementException e)
			{
				throw new MissingHtmlException(e.Message, e);
			}
		}

		public Node FindTextField(string locator)
		{
			IWebElement textField;
			var label = FindLabelByText(locator);
			if (label != null)
			{
				textField = FindTextInputFromLabel(label);
			}
			else
			{
				textField = FindTextFieldById(locator) ??
				            FindTextFieldByName(locator);
			}
			return BuildNode(textField, "find text field: " + locator);
		}

		private IWebElement FindLabelByText(string locator)
		{
			return selenium.FindElements(By.TagName("label")).FirstVisibleOrDefault(e => e.Text == locator);
		}

		private IWebElement FindTextInputFromLabel(IWebElement label)
		{
			return FindTextFieldById(label.GetAttribute("for")) ??
			       label.FindElements(By.TagName("input")).FirstVisibleOrDefault(IsTextField);
		}

		private IWebElement FindTextFieldById(string id)
		{
			return selenium.FindElementsById(id).FirstVisibleOrDefault(IsTextField);
		}

		private IWebElement FindTextFieldByName(string name)
		{
			return selenium.FindElementsByName(name).FirstVisibleOrDefault(IsTextField);
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
			node.Update();
		}

		private IWebElement SeleniumElement(Node node)
		{
			return ((IWebElement) node.UnderlyingNode);
		}

		private bool IsInputButton(IWebElement e)
		{
			return e.TagName == "button" ||
			       (e.TagName == "input" && (HasAttr(e, "type", "button") || HasAttr(e, "type", "submit")));
		}

		private bool IsTextField(IWebElement e)
		{
			return e.TagName == "input" && (HasAttr(e, "type", "text") || HasAttr(e, "type", "password"));
		}

		private bool HasAttr(IWebElement e, string attributeName, string value)
		{
			return e.GetAttribute(attributeName) == value;
		}

		private IEnumerable<IWebElement> find(By by)
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