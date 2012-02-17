using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    internal class WatiNElement : Element
    {
        internal WatiNElement(WatiN.Core.Element watinElement)
        {
            Native = watinElement;
        }

        private T GetNativeWatiNElement<T>()
            where T : WatiN.Core.Element
        {
            return Native as T;
        }

        private WatiN.Core.Element NativeWatiNElement
        {
            get { return GetNativeWatiNElement<WatiN.Core.Element>(); }
        }

        public override string Id
        {
            get { return NativeWatiNElement.Id; }
        }

        public override string Text
        {
            get
            {
                var text = NativeWatiNElement.Text ?? NativeWatiNElement.OuterText;
                return text != null ? text.Trim() : null;
            }
        }

        public override string Value
        {
            get
            {
                var textField = GetNativeWatiNElement<TextField>();
                return textField != null ? textField.Value : this["value"];
            }
        }

        public override string Name
        {
            get { return NativeWatiNElement.Name; }
        }

        public override string SelectedOption
        {
            get 
            { 
                var selectList = GetNativeWatiNElement<SelectList>();
                return selectList != null ? selectList.SelectedOption.Text : string.Empty;
            }
        }

        public override bool Selected
        {
            get
            {
                var checkbox = GetNativeWatiNElement<CheckBox>();
                if (checkbox != null)
                    return checkbox.Checked;

                var radioButton = GetNativeWatiNElement<RadioButton>();
                if (radioButton != null)
                    return radioButton.Checked;

                return false;
            }
        }

        public override string this[string attributeName]
        {
            get { return NativeWatiNElement.GetAttributeValue(attributeName); }
        }
    }
}