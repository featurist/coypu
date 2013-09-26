using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    internal class WatiNFrame : ElementFound
    {
        public WatiNFrame(Frame frame)
        {
            Native = frame;
        }

        private Frame Frame
        {
            get { return Native as Frame; }
        }

        public string Id
        {
            get { return Frame.Id; }
        }

        public string Text
        {
            get
            {
                var text = Frame.Text;
                return text != null ? text.Trim() : null;
            }
        }

        public string Value
        {
            get { return null; }
        }

        public string Name
        {
            get { return Frame.Name; }
        }

        public string SelectedOption
        {
            get { return string.Empty; }
        }

        public bool Selected
        {
            get { return false; }
        }

        public object Native { get; private set; }

        public bool Stale(Options options)
        {
            return !((Frame) Native).FrameElement.Exists;
        }

        public string this[string attributeName]
        {
            get { return Frame.GetAttributeValue(attributeName); }
        }
    }
}