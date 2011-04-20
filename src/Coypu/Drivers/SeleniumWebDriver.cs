using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers
{
	public class SeleniumWebDriver : Driver
	{
		private readonly RemoteWebDriver selenium;

		public SeleniumWebDriver(Browser browser)
		{
			selenium = NewRemoteWebDriver(browser);
		}

		private RemoteWebDriver NewRemoteWebDriver(Browser browser)
		{
			switch (browser)
			{
			
				case (Browser.Firefox):
					return new FirefoxDriver();
				case (Browser.InternetExplorer):
					return new InternetExplorerDriver();
				case (Browser.Chrome):
					return new ChromeDriver();
				default:
					throw new BrowserNotSupportedException(browser,this);
			}
		}

		private Node BuildNode(IWebElement element)
		{
			return new Node
			       	{
			       		UnderlyingNode = element,
			       		Text = element.Text,
			       		Id = element.GetAttribute("id"),
			       		Value = element.GetAttribute("value"),
			       	};
		}

		public Node FindButton(string locator)
		{
			var found =
				find(By.TagName(string.Format("button"))).FirstOrDefault(e => e.Text == locator) ??
				find(By.CssSelector(string.Format("input[type=button],input[type=submit]"))).FirstOrDefault(e => e.Value == locator) ??
				find(By.Id(locator)).FirstOrDefault(IsInputButton) ??
				find(By.Name(locator)).FirstOrDefault(IsInputButton);

			return BuildNode(found);
		}

		public Node FindLink(string locator)
		{
			throw new NotImplementedException();
		}

		public void Click(Node node)
		{
			SeleniumElement(node).Click();
		}

		public void Visit(string url)
		{
			selenium.Navigate().GoToUrl(url);
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
			selenium.Dispose();
		}
	}

	internal class BrowserNotSupportedException : Exception
	{
		public BrowserNotSupportedException(Browser browser, Driver driver) 
			: base(string.Format("{0} is not supported by {1}", browser, driver.GetType().Name))
		{
		}
	}
}