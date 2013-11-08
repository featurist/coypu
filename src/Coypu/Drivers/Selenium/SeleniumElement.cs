using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumElement : ElementFound
    {
        protected readonly IWebElement native;
        protected readonly IWebDriver selenium;

        public SeleniumElement(IWebElement seleniumElement, IWebDriver selenium)
        {
            native = seleniumElement;
            this.selenium = selenium;
        }

        public string Id
        {
            get { return native.GetAttribute("id"); }
        }

        public virtual string Text
        {
            get
            {
                return native.Text;
            }
        }

        public string Value
        {
            get { return native.GetAttribute("value"); }
        }

        public string Name
        {
            get { return native.GetAttribute("name"); }
        }

        public virtual string OuterHTML
        {
            get
            {
                return native.GetAttribute("outerHTML");
            }
        }

        public virtual string InnerHTML
        {
            get
            {
                return native.GetAttribute("innerHTML");
            }
        }

        public string SelectedOption
        {
            get
            {
                return native.FindElements(By.TagName("option"))
                    .Where(e => e.Selected)
                    .Select(e => e.Text)
                    .FirstOrDefault();
            }
        }

        public bool Selected
        {
            get { return native.Selected; }
        }

        public virtual object Native
        {
            get
            {
                return native;
            }
        }

        public bool Stale(Options options)
        {
            try
            {
                native.FindElement(By.XPath("."));
                return !options.ConsiderInvisibleElements && !native.Displayed;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
            catch (NoSuchWindowException)
            {
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        }

        public string this[string attributeName]
        {
            get { return native.GetAttribute(attributeName); }
        }
    }
}