using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Coypu.Drivers.Playwright
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
            return Async.WaitForResult(_native.GetAttributeAsync(attributeName));
        }

        public string Id => GetAttribute("id");

        public virtual string Text => Async.WaitForResult(_native.InnerTextAsync());

        public string Value => Async.WaitForResult(_native.InputValueAsync());

        public string Name => GetAttribute("name");

        public virtual string OuterHTML => Async.WaitForResult(_native.EvaluateAsync("el => el.outerHTML")).ToString();

        public virtual string InnerHTML => Async.WaitForResult(_native.InnerHTMLAsync());

        public string Title => GetAttribute("title");

        public bool Disabled => !Async.WaitForResult<bool>(_native.IsEnabledAsync());

        public string SelectedOption
        {
            get
            {
                return Async.WaitForResult(_native.EvalOnSelectorAsync("option", "sel => sel.options[sel.options.selectedIndex].innerText")).ToString();
            }
        }

        public bool Selected {
          get {
            var tagName = Async.WaitForResult(_native.EvaluateAsync("e => e.tagName")).ToString();
            if (tagName.ToLower() == "select") {
              return Async.WaitForResult(_native.InputValueAsync()) == SelectedOption;
            } else {
              return Async.WaitForResult(_native.IsCheckedAsync());
            }
          }
        }

        public virtual object Native => _native;

        public string this[string attributeName] => GetAttribute(attributeName);
    }
}
