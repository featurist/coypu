using System;

namespace Coypu
{
	public interface Driver : IDisposable
	{
		Node FindButton(string locator);
		Node FindLink(string locator);
		void Click(Node node);
		void Visit(string url);
	}
}