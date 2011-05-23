using System;
using System.Collections.Generic;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Exceptions;

namespace Coypu.Drivers.Watin
{
	public class WatiNDriver : Driver
	{
	    public bool Disposed { get; private set; }

	    private WatiN.Core.Browser watinInstance;

	    private WatiN.Core.Browser Watin 
		{ 
			get 
			{ 
				return watinInstance ?? (watinInstance = NewDriver());
			}
		}

	    private WatiN.Core.Browser NewDriver()
		{
			switch (Configuration.Browser)
			{
				case (Browser.InternetExplorer):
					return new IE();
//				case (Browser.Firefox):
//					return new FireFox();
				default:
					throw new BrowserNotSupportedException(Configuration.Browser, this);
			}
		}

	    public void SetScope(Func<Element> find)
	    {
	        throw new NotSupportedException();
	    }

	    public void ClearScope()
		{
			throw new NotSupportedException();
		}

	    public string ExecuteScript(string javascript)
	    {
	        throw new NotSupportedException();
	    }

		public Element FindFieldset(string locator)
		{
			throw new NotImplementedException();
		}

		public Element FindSection(string locator)
		{
			throw new NotImplementedException();
		}

		public Element FindButton(string locator)
		{
			var button = Watin.Buttons.Filter(b => b.Text == locator).Cast<WatiN.Core.Element>().FirstDisplayedOrDefault() ??
						 Watin.Buttons.Filter(b => b.Id == locator).Cast<WatiN.Core.Element>().FirstDisplayedOrDefault();
						 Watin.Buttons.Filter(b => b.Name == locator).Cast<WatiN.Core.Element>().FirstDisplayedOrDefault();

			return BuildElement(button, "Failed to find button with text, id or name: " + locator);
		}

	    public Element BuildElement(WatiN.Core.Element element, string description)
		{
			if (element == null)
			{
				throw new MissingHtmlException(description);
			}
			return new WatiNElement(element);
		}

	    public Element FindLink(string locator)
		{
			var link = Watin.Links.Filter(l => l.Text.Trim() == locator.Trim()).Cast<WatiN.Core.Element>().FirstDisplayedOrDefault();

			return BuildElement(link, "Failed to find link with text: " + locator);
		}

	    public Element FindField(string locator)
		{
			var allFields = FindAllFields();
			var field = FindFieldByLabel(locator, allFields) ??
			            allFields.FirstDisplayedOrDefault(f => f.Id == locator) ??
			            allFields.FirstDisplayedOrDefault(f => f.Name == locator) ??
			            allFields.FirstDisplayedOrDefault(f => f.GetAttributeValue("placeholder") == locator) ??
			            GetRadioButtonWithValue(locator);

			return BuildElement(field, "Failed to find field with label, id, name or placeholder: " + locator);
		}

	    private WatiN.Core.Element GetRadioButtonWithValue(string value)
		{
			return Watin.RadioButtons.Cast<WatiN.Core.Element>().FirstDisplayedOrDefault(r => r.GetAttributeValue("value") == value);
		}

	    private IEnumerable<WatiN.Core.Element> FindAllFields()
		{
			var textFields = Watin.TextFields.Cast<WatiN.Core.Element>();
			var selects = Watin.SelectLists.Cast<WatiN.Core.Element>();
			var checkboxes = Watin.CheckBoxes.Cast<WatiN.Core.Element>();
			var radioButtons = Watin.RadioButtons.Cast<WatiN.Core.Element>();

			return textFields.Concat(selects.Concat(checkboxes.Concat(radioButtons)));
		}

	    private WatiN.Core.Element FindFieldByLabel(string locator, IEnumerable<WatiN.Core.Element> allFields)
		{
			var label = Watin.Labels.Filter(l => l.Text == locator).First();
			if (label != null)
			{
				return allFields.FirstDisplayedOrDefault(f => f.Id == label.For) ??
					   allFields.FirstDisplayedOrDefault(f => label.Children().Contains(f));
			}
			return null;
		}

	    public void Click(Element element)
		{
			WatiNElement(element).Click();
		}

	    private WatiN.Core.Element WatiNElement(Element element)
		{
			return ((WatiN.Core.Element) element.Native);
		}

	    public void Visit(string url)
		{
			Watin.GoTo(url);
		}

	    public void Set(Element element, string value)
		{
			((TextField) element.Native).Value = value;
		}

	    public void Select(Element element, string option)
		{
			try
			{
				((SelectList) element.Native).Select(option);
			}
			catch (WatiNException)
			{
				((SelectList) element.Native).SelectByValue(option);
			}
		}

	    public object Native
		{
			get { return Watin; }
		}

	    public bool HasContent(string text)
		{
			return Watin.ContainsText(text);
		}

	    public void Check(Element field)
		{
			((CheckBox)field.Native).Checked = true;
		}

	    public void Uncheck(Element field)
		{
			((CheckBox)field.Native).Checked = false;
		}

	    public void Choose(Element field)
		{
			((RadioButton)field.Native).Checked = true;
		}

	    public bool HasDialog(string withText)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

	    public void AcceptModalDialog()
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

	    public void CancelModalDialog()
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

	    public bool HasCss(string cssSelector)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

		public bool HasXPath(string xpath)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

		public Element FindCss(string cssSelector)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

		public Element FindXPath(string xpath)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

		public IEnumerable<Element> FindAllCss(string cssSelector)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

		public IEnumerable<Element> FindAllXPath(string xpath)
		{
			throw new NotSupportedException("Not yet implemented in WatiNDriver");
		}

		public void Dispose()
		{
            Watin.Close();
            Watin.Dispose();
			Disposed = true;
		}
	}
}