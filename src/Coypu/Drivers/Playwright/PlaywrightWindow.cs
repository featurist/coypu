using System;
using OpenQA.Selenium;
using Microsoft.Playwright;

namespace Coypu.Drivers.Playwright
{
    internal class PlaywrightWindow : Element
    {
        private readonly IBrowser _playwrightBrowser;
        private readonly IPage _page;

        public PlaywrightWindow(IBrowser playwrightBrowser,
                                IPage page)
        {
            _playwrightBrowser = playwrightBrowser;
            _page = page;
        }

        public string Id => throw new NotSupportedException();

        public string Text => ((ISearchContext) Native).FindElement(By.CssSelector("body"))
                                                       .Text;

        public string InnerHTML => ((ISearchContext) Native).FindElement(By.XPath("./*"))
                                                            .GetAttribute("innerHTML");

        public string Title => Async.WaitForResult(_page.TitleAsync());

        public bool Disabled => throw new NotSupportedException();

        public string OuterHTML => ((ISearchContext) Native).FindElement(By.XPath("./*"))
                                                            .GetAttribute("outerHTML");

        public string Value => throw new NotSupportedException();

        public string Name => throw new NotSupportedException();

        public string SelectedOption => throw new NotSupportedException();

        public bool Selected => throw new NotSupportedException();

        public object Native
        {
            get
            {
                return _page;
            }
        }

        public string this[string attributeName] => throw new NotSupportedException();
    }
}
