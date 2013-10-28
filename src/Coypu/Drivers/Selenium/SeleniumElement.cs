using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumElement : ElementFound
    {
        private readonly IWebElement native;
        private readonly IWebDriver webDriver;

        public SeleniumElement(IWebElement seleniumElement, IWebDriver webDriver)
        {
            native = seleniumElement;
            this.webDriver =  webDriver;
        }

        protected IWebElement NativeSeleniumElement
        {
            get { return native; }
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

        public string OuterHTML
        {
            get { return NativeSeleniumElement.GetAttribute("outerHTML"); }
        }

        public string InnerHTML
        {
            get { return NativeSeleniumElement.GetAttribute("innerHTML"); }
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

        public virtual object Native
        {
            get
            {
                if (new []{"iframe","frame" }.Contains(NativeSeleniumElement.TagName.ToLower()))
                {
                    webDriver.SwitchTo().Frame(NativeSeleniumElement);
                    return webDriver;
                }
                return NativeSeleniumElement;
            }
        }

        

        public bool Stale(Options options)
        {
            try
            {
                NativeSeleniumElement.FindElement(By.XPath("."));
                return !options.ConsiderInvisibleElements && !NativeSeleniumElement.Displayed;
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
            get { return NativeSeleniumElement.GetAttribute(attributeName); }
        }
    }
}