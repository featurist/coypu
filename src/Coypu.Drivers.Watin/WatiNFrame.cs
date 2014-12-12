using System;
using System.Linq;
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

        public string InnerHTML
        {
            get
            {
                return Frame.XPath("./*").First().InnerHtml;
            }
        }

        public string Title
        {
            get { return Frame.Title; }
        }

        public bool Disabled
        {
            get { throw new NotSupportedException(); }
        }

        public string OuterHTML
        {
            get
            {
                return Frame.XPath("./*").First().OuterHtml;
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