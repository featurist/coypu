using System;
using System.Collections.Generic;
using WatiN.Core;

namespace Coypu.Drivers.WatiNDriver
{
	public class WatiNDriver : Driver
	{
		private readonly WatiN.Core.Browser watin;

		public WatiNDriver()
		{
			watin = NewDriver();
		}

		private WatiN.Core.Browser NewDriver()
		{
			switch (Configuration.Browser)
			{
				case (Browser.InternetExplorer):
					return new IE();
				case (Browser.Firefox):
					return new FireFox();
				default:
					throw new BrowserNotSupportedException(Configuration.Browser, this);
			}
		}
		public Element FindButton(string locator)
		{
			throw new NotImplementedException();
		}

		public Element FindLink(string locator)
		{
			throw new NotImplementedException();
		}

		public Element FindField(string locator)
		{
			throw new NotImplementedException();
		}

		public void Click(Element element)
		{
			throw new NotImplementedException();
		}

		public void Visit(string url)
		{
			throw new NotImplementedException();
		}

		public void Set(Element element, string value)
		{
			throw new NotImplementedException();
		}

		public void Select(Element element, string option)
		{
			throw new NotImplementedException();
		}

		public object Native
		{
			get { throw new NotImplementedException(); }
		}

		public bool HasContent(string text)
		{
			throw new NotImplementedException();
		}

		public bool HasCss(string cssSelector)
		{
			throw new NotImplementedException();
		}

		public bool HasXPath(string xpath)
		{
			throw new NotImplementedException();
		}

		public bool HasDialog(string withText)
		{
			throw new NotImplementedException();
		}

		public Element FindCss(string cssSelector)
		{
			throw new NotImplementedException();
		}

		public Element FindXPath(string xpath)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Element> FindAllCss(string cssSelector)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Element> FindAllXPath(string xpath)
		{
			throw new NotImplementedException();
		}

		public void Check(Element field)
		{
			throw new NotImplementedException();
		}

		public void Uncheck(Element field)
		{
			throw new NotImplementedException();
		}

		public void Choose(Element field)
		{
			throw new NotImplementedException();
		}

		public bool Disposed { get; private set; }
		public void AcceptModalDialog()
		{
			throw new NotImplementedException();
		}

		public void CancelModalDialog()
		{
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
			watin.Close();
			watin.Dispose();
			Disposed = true;
		}
	}
}