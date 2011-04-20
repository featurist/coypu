using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Coypu.Drivers
{
	public class SeleniumWebDriver : Driver, IDisposable
	{
		private readonly FirefoxDriver selenium;

		public SeleniumWebDriver()
		{
			selenium = new FirefoxDriver();
		}

		private Node BuildNode(IWebElement element)
		{
			return new Node(null)
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
			throw new NotImplementedException();
		}

		public void Visit(string url)
		{
			selenium.Navigate().GoToUrl(url);
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
}