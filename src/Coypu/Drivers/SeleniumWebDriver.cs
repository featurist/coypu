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
					throw new BrowserNotSupportedException(Configuration.Browser,this);
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

			if (found == null)
				throw new MissingHtmlException("Could not find button: " + locator);

			return BuildNode(found);
		}

		public Node FindLink(string locator)
		{
			try
			{
				return BuildNode(selenium.FindElementByLinkText(locator));
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
			return BuildNode(textField);
		}

		private IWebElement FindLabelByText(string locator)
		{
			return selenium.FindElements(By.TagName("label")).FirstOrDefault(e => e.Text == locator);
		}

		private IWebElement FindTextInputFromLabel(IWebElement label)
		{
			return FindTextFieldById(label.GetAttribute("for")) ??
			       label.FindElements(By.TagName("input")).FirstOrDefault(IsTextField);
		}

		private IWebElement FindTextFieldById(string id)
		{
			return selenium.FindElementsById(id).FirstOrDefault(IsTextField);
		}

		private IWebElement FindTextFieldByName(string name)
		{
			return selenium.FindElementsByName(name).FirstOrDefault(IsTextField);
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

		private bool IsTextField(IWebElement e)
		{
			return e.TagName == "input" && (HasAttr(e, "type", "text"));
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
		}
	}
}