using System;

namespace Coypu.Tests.TestDoubles
{
	public class StubDriver : Driver
	{
		public void Dispose()
		{
		}

		public Node FindButton(string locator)
		{
			return null;
		}

		public Node FindLink(string locator)
		{
			return null;
		}

		public Node FindField(string locator)
		{
			return null;
		}

		public void Click(Node node)
		{
		}

		public void Visit(string url)
		{
		}

		public void Set(Node node, string value)
		{
		}

		public void Select(Node node, string option)
		{
		}

		public object Native
		{
			get { return "Native driver on stub driver"; }
		}

		public bool HasContent(string text)
		{
			return false;
		}

		public bool HasCss(string cssSelector)
		{
			return false;
		}

		public bool HasXPath(string xpath)
		{
			return false;
		}

		public Node FindCss(string cssSelector)
		{
			return null;
		}

		public Node FindXPath(string xpath)
		{
			return null;
		}
	}
}