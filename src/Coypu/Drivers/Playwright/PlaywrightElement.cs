using System.Linq;
using System.Runtime.Serialization;
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

        public string Value {
            get {
                var inputTags = new[] { "input", "textarea", "select" };
                if (inputTags.Contains(TagName.ToLower()))
                  return Async.WaitForResult(_native.InputValueAsync());

                return this["value"];
            }
        }

        public string Name => GetAttribute("name");

        public string TagName => Async.WaitForResult(_native.EvaluateAsync("e => e.tagName"))?.GetString();

        public virtual string OuterHTML => Async.WaitForResult(_native.EvaluateAsync("el => el.outerHTML")).ToString();

        public virtual string InnerHTML => Async.WaitForResult(_native.InnerHTMLAsync());

        public string Title => GetAttribute("title");

        public bool Disabled => !Async.WaitForResult<bool>(_native.IsEnabledAsync());

        public string SelectedOption
        {
            get
            {
                return Async.WaitForResult(_native.EvaluateAsync("sel => sel.options[sel.options.selectedIndex].innerText")).ToString();
            }
        }

        public bool Selected {
          get {
            return Async.WaitForResult(_native.IsCheckedAsync());
          }
        }

        public virtual object Native => _native;

        public string this[string attributeName] => GetAttribute(attributeName);
    }
}
