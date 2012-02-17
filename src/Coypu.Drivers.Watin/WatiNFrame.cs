using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    internal class WatiNFrame : Element
    {
        public WatiNFrame(Frame frame)
        {
            Native = frame;
        }

        private Frame Frame
        {
            get { return Native as Frame; }
        }

        public override string Id
        {
            get { return Frame.Id; }
        }

        public override string Text
        {
            get
            {
                var text = Frame.Text;
                return text != null ? text.Trim() : null;
            }
        }

        public override string Value
        {
            get { return null; }
        }

        public override string Name
        {
            get { return Frame.Name; }
        }

        public override string SelectedOption
        {
            get { return string.Empty; }
        }

        public override bool Selected
        {
            get { return false; }
        }

        public override string this[string attributeName]
        {
            get { return Frame.GetAttributeValue(attributeName); }
        }
    }
}