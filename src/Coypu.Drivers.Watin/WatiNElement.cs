using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    internal class WatiNElement : Element
    {
        internal WatiNElement(object watinElement)
        {
            Native = watinElement;
        }

        public WatiN.Core.Element NativeWatiNElement
        {
            get { return (WatiN.Core.Element) Native; }
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
                var textField = NativeWatiNElement as TextField;
                
                return textField != null 
                    ? textField.Value 
                    : this["value"];
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
                var selectList = NativeWatiNElement as SelectList;
                
                return selectList != null 
                    ? selectList.SelectedOption.Text 
                    : string.Empty;
            }
        }

        public override bool Selected
        {
            get
            {
                var checkbox = NativeWatiNElement as CheckBox;
                if (checkbox != null)
                    return checkbox.Checked;

                var radioButton = NativeWatiNElement as RadioButton;
                if (radioButton  != null)
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