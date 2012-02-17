using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{

    internal class SeleniumSearchContext : Element
    {
        private readonly ISearchContext searchContext;

        public SeleniumSearchContext(ISearchContext searchContext)
        {
            this.searchContext = searchContext;
        }

        public string Id
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Text
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Value
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public string SelectedOption
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Selected
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Native
        {
            get { return searchContext; }
        }

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }
    }

    internal class SeleniumElement : Element
    {
        private IWebElement NativeSeleniumElement
        {
            get { return (IWebElement) Native; }
        }

        public SeleniumElement(IWebElement seleniumElement)
        {
            Native = seleniumElement;
        }

        public string Id
        {
            get { return NativeSeleniumElement.GetAttribute("id"); }
        }

        public string Text
        {
            get { return NativeSeleniumElement.Text; }
        }

        public string Value
        {
            get { return NativeSeleniumElement.GetAttribute("value"); }
        }

        public string Name
        {
            get { return NativeSeleniumElement.GetAttribute("name"); }
        }

        public string SelectedOption
        {
            get
            {
                return NativeSeleniumElement.FindElements(By.TagName("option"))
                    .Where(e => e.Selected)
                    .Select(e => e.Text)
                    .FirstOrDefault();
            }
        }

        public bool Selected
        {
            get { return NativeSeleniumElement.Selected; }
        }

        public object Native { get; set; }

        public string this[string attributeName]
        {
            get { return NativeSeleniumElement.GetAttribute(attributeName); }
        }
    }
}