using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class OptionSelector
    {
        public void Select(Element element, string option)
        {
            var select = (IWebElement)element.Native;
            select.Click();

            var optionToSelect =
                select.FindElements(By.TagName("option")).FirstOrDefault(e => e.Text == option || e.GetAttribute("value") == option);

            if (optionToSelect == null)
                throw new MissingHtmlException("No such option: " + option);

            optionToSelect.Click();
        }
    }
}