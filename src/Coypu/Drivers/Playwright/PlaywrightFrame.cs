using Microsoft.Playwright;

namespace Coypu.Drivers.Playwright
{
    internal class PlaywrightFrame : Element
    {

        private readonly IFrame _frame;
        private readonly IElementHandle _frameElement;
        private string _id;

        public PlaywrightFrame(IElementHandle frameElement)
        {
            _frameElement = frameElement;
            _frame = frameElement.ContentFrameAsync().Sync();
        }

        public string this[string attributeName] => GetAttribute(attributeName);

        private string GetAttribute(string attributeName)
        {
            return _frameElement.GetAttributeAsync(attributeName).Sync();
        }

        public string Text => FindBody().Text;

        public string OuterHTML => FindBody().OuterHTML;

        public string InnerHTML => FindBody().InnerHTML;

        public object Native
        {
            get
            {
                return _frame;
            }
        }

    public string Id {
      get {
        return this["id"];
      }
    }

    public string Value => throw new System.NotImplementedException();

    public string Name => _frame.Name;

    public string SelectedOption => throw new System.NotImplementedException();

    public bool Selected => throw new System.NotImplementedException();

    public string Title => _frame.TitleAsync().Sync();

    public bool Disabled => throw new System.NotImplementedException();

    private PlaywrightElement FindBody()
        {
            return new PlaywrightElement(_frame.QuerySelectorAsync("body").Sync());
        }
    }
}
