using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Coypu.Drivers.Selenium
{
    internal class OptionSelector
    {
        public void Select(Element element, string option)
        {
            var select = new SelectElement((IWebElement)element.Native);
            try
            {
                select.SelectByText(option);
            }
            catch (NoSuchElementException)
            {
                select.SelectByValue(option);
            }
        }
    }
}