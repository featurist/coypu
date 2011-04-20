using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Nappybara.Drivers
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
                       };
        }

        public Node FindButton(string locator)
        {
            var found =
                selenium.FindElements(By.XPath(string.Format("//button[text() = '{0}']", locator))).FirstOrDefault() ??
                selenium.FindElements(By.Id(locator)).FirstOrDefault(e => e.TagName == "button");

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

        public void Dispose()
        {
            selenium.Dispose();
        }
    }
}