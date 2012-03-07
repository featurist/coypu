using System;
using WatiN.Core;

namespace Coypu.Drivers.Watin
{
    internal class WatiNElement : ElementFound
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

        public string Id
        {
            get { return NativeWatiNElement.Id; }
        }

        public string Text
        {
            get
            {
                var text = NativeWatiNElement.Text ?? NativeWatiNElement.OuterText;
                return text != null ? text.Trim() : null;
            }
        }

        public string Value
        {
            get
            {
                var textField = GetNativeWatiNElement<TextField>();
                return textField != null ? textField.Value : this["value"];
            }
        }

        public string Name
        {
            get { return NativeWatiNElement.Name; }
        }

        public string SelectedOption
        {
            get 
            { 
                var selectList = GetNativeWatiNElement<SelectList>();
                return selectList != null ? selectList.SelectedOption.Text : string.Empty;
            }
        }

        public bool Selected
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

        public object Native { get; private set; }


        public bool Stale
        {
            get
            {
                return !NativeWatiNElement.Exists;
            }
        }

        public string this[string attributeName]
        {
            get { return NativeWatiNElement.GetAttributeValue(attributeName); }
        }
    }
}