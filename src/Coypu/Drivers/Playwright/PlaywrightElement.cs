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

        public PlaywrightElement(IElementHandle PlaywrightElement)
        {
            _native = PlaywrightElement;
        }

        private string GetAttribute(string attributeName)
        {
            return _native.GetAttributeAsync(attributeName).Sync();
        }

        public string Id => GetAttribute("id");

        public virtual string Text => _native.InnerTextAsync().Sync();

        public string Value {
            get {
                var inputTags = new[] { "input", "textarea", "select" };
                if (inputTags.Contains(TagName.ToLower()))
                  return _native.InputValueAsync().Sync();

                return this["value"];
            }
        }

        public string Name => GetAttribute("name");

        public string TagName => _native.EvaluateAsync("e => e.tagName").Sync()?.GetString();

        public virtual string OuterHTML => _native.EvaluateAsync("el => el.outerHTML").Sync().ToString();

        public virtual string InnerHTML => _native.InnerHTMLAsync().Sync();

        public string Title => GetAttribute("title");

        public bool Disabled => !_native.IsEnabledAsync().Sync();

        public string SelectedOption
        {
            get
            {
                return _native.EvaluateAsync("sel => sel.options[sel.options.selectedIndex].innerText").Sync().ToString();
            }
        }

        public bool Selected {
          get {
            return _native.IsCheckedAsync().Sync();
          }
        }

        public virtual object Native => _native;

        public string this[string attributeName] => GetAttribute(attributeName);
    }
}
