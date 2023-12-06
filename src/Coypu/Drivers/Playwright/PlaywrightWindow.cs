using System;
using OpenQA.Selenium;
using Microsoft.Playwright;
using System.Linq;

namespace Coypu.Drivers.Playwright
{
    internal class PlaywrightWindow : Element
    {
        private readonly IPage _page;

        public PlaywrightWindow(IPage page)
        {
            _page = page;
        }

        public string Id => throw new NotSupportedException();

        public string Text => ((IPage) Native).InnerTextAsync("xpath=/html/body").Sync();

        public string InnerHTML => ((IPage) Native).InnerHTMLAsync("xpath=./*").Sync().ToString();

        public string Title => _page.TitleAsync().Sync();

        public bool Disabled => throw new NotSupportedException();

        public string OuterHTML => ((IPage) Native).EvalOnSelectorAsync("html", "h => h.outerHTML").Sync().ToString();

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
