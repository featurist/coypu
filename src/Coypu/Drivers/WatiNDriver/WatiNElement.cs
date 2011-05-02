using System;

namespace Coypu.Drivers.WatiNDriver
{
	public class WatiNElement : Element
	{
		public WatiNElement(object watinElement)
		{
			Native = watinElement;
		}

		public override string Id
		{
			get { throw new NotImplementedException(); }
		}

		public override string Text
		{
			get { throw new NotImplementedException(); }
		}

		public override string Value
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override string SelectedOption
		{
			get { throw new NotImplementedException(); }
		}

		public override bool Selected
		{
			get { throw new NotImplementedException(); }
		}

		public override string this[string attributeName]
		{
			get { throw new NotImplementedException(); }
		}
	}
}