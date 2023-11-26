using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Coypu.Drivers.MicrosoftPlaywright
{
    internal class PlaywrightElement : Element
    {
        // ReSharper disable once InconsistentNaming
        protected readonly IElementHandle _native;
        protected readonly IPlaywright _playwright;

        public PlaywrightElement(IElementHandle PlaywrightElement,
                               IPlaywright Playwright)
        {
            _native = PlaywrightElement;
            _playwright = Playwright;
        }

        private string GetAttribute(string attributeName)
        {
            Task<string> task =  _native.GetAttributeAsync(attributeName);
            task.Wait();
            return task.Result;
        }

        public string Id => GetAttribute("id");

        public virtual string Text()
        {
          Task<string> task = _native.InnerTextAsync();
          task.Wait();
          return task.Result;
        }

        public string Value => GetAttribute("value");

        public string Name => GetAttribute("name");

        public virtual string OuterHTML => GetAttribute("outerHTML");

        public virtual string InnerHTML => GetAttribute("innerHTML");

        public string Title => GetAttribute("title");

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

        public string this[string attributeName] => GetAttribute(attributeName);
    }
}
