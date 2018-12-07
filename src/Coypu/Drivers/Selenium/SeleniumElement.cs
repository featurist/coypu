using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumElement : Element
    {
        // ReSharper disable once InconsistentNaming
        protected readonly IWebElement _native;
        protected readonly IWebDriver Selenium;

        public SeleniumElement(IWebElement seleniumElement,
                               IWebDriver selenium)
        {
            _native = seleniumElement;
            Selenium = selenium;
        }

        public string Id => _native.GetAttribute("id");

        public virtual string Text => _native.Text;

        public string Value => _native.GetAttribute("value");

        public string Name => _native.GetAttribute("name");

        public virtual string OuterHTML => _native.GetAttribute("outerHTML");

        public virtual string InnerHTML => _native.GetAttribute("innerHTML");

        public string Title => _native.GetAttribute("title");

        public bool Disabled => !_native.Enabled;

        public string SelectedOption
        {
            get
            {
                return _native.FindElements(By.TagName("option"))
                             .Where(e => e.Selected)
                             .Select(e => e.Text)
                             .FirstOrDefault();
            }
        }

        public bool Selected => _native.Selected;

        public virtual object Native => _native;

        public string this[string attributeName] => _native.GetAttribute(attributeName);
    }
}