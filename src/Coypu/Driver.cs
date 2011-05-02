using System;
using System.Collections.Generic;

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
		void Select(Node node, string option);
		object Native { get; }
		bool HasContent(string text);
		bool HasCss(string cssSelector);
		bool HasXPath(string xpath);
		bool HasDialog(string withText);
		Node FindCss(string cssSelector);
		Node FindXPath(string xpath);
		IEnumerable<Node> FindAllCss(string cssSelector);
		IEnumerable<Node> FindAllXPath(string xpath);
		void Check(Node field);
		void Uncheck(Node field);
		void Choose(Node field);
		bool Disposed { get; }
		void AcceptModalDialog();
		void CancelModalDialog();
	}
}