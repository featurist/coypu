using System;

namespace Coypu
{
	public interface Driver : IDisposable
	{
		Node FindButton(string locator);
		Node FindLink(string locator);
		Node FindField(string locator);
		void Click(Node node);
		void Visit(string url);
		void Set(Node node, string value);
		object Native { get; }
	}
}