using System;
using System.Collections.Generic;

namespace Coypu.Tests.TestDoubles
{
	public class FakeDriver : Driver
	{
		private readonly IList<Node> clickedNodes = new List<Node>();
		private readonly Dictionary<string, Node> buttons = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> links = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> textFields = new Dictionary<string, Node>();
		private readonly IList<string> hasContentQueries = new List<string>();
		private readonly IList<string> hasCssQueries = new List<string>();
		private readonly IList<string> hasXPathQueries = new List<string>();
		private readonly IList<string> visits = new List<string>();
		private readonly IList<string> findButtonRequests = new List<string>();
		private readonly IList<string> findLinkRequests = new List<string>();
		private readonly IList<string> findTextFieldRequests = new List<string>();
		private readonly IDictionary<Node, string> setFields = new Dictionary<Node, string>();
		private readonly IDictionary<Node, string> selectedOptions = new Dictionary<Node, string>();
		private readonly IDictionary<string, bool> stubbedHasContentResults = new Dictionary<string, bool>();
		private readonly IDictionary<string, bool> stubbedHasCssResults = new Dictionary<string, bool>();
		private readonly IDictionary<string, bool> stubbedHasXPathResults = new Dictionary<string, bool>();
		private bool disposed;

		public IEnumerable<Node> ClickedNodes
		{
			get { return clickedNodes; }
		}

		public IList<string> FindButtonRequests
		{
			get { return findButtonRequests; }
		}

		public IList<string> FindLinkRequests
		{
			get { return findLinkRequests; }
		}

		public IDictionary<Node, string> SetFields
		{
			get { return setFields; }
		}

		public IDictionary<Node,string> SelectedOptions
		{
			get { return selectedOptions; }
		}

		public IEnumerable<string> Visits
		{
			get { return visits; }
		}

		public IEnumerable<string> HasContentQueries
		{
			get { return hasContentQueries; }
		}

		public IEnumerable<string> HasCssQueries
		{
			get { return hasCssQueries; }
		}

		public IEnumerable<string> HasXPathQueries
		{
			get { return hasXPathQueries; }
		}
        
		public Node FindButton(string locator)
		{
			findButtonRequests.Add(locator);
			return buttons[locator];
		}

		public Node FindLink(string locator)
		{
			findLinkRequests.Add(locator);
			return links[locator];
		}

		public Node FindField(string locator)
		{
			findTextFieldRequests.Add(locator);
			return textFields[locator];
		}

		public void Click(Node node)
		{
			clickedNodes.Add(node);
		}

		public void Visit(string url)
		{
			visits.Add(url);
		}

		public void StubButton(string locator, Node node)
		{
			buttons[locator] = node;
		}

		public void StubLink(string locator, Node node)
		{
			links[locator] = node;
		}

		public void StubField(string locator, Node node)
		{
			textFields[locator] = node;
		}

		public void Dispose()
		{
			disposed = true;
		}

		public bool Disposed()
		{
			return disposed;
		}

		public void Set(Node node, string value)
		{
			setFields.Add(node, value);
		}

		public void Select(Node node, string option)
		{
			selectedOptions.Add(node, option);
		}

		public object Native
		{
			get { return "Native driver on fake driver"; }
		}

		public bool HasContent(string text)
		{
			hasContentQueries.Add(text);
			return stubbedHasContentResults[text];
		}

		public void StubHasContent(string text, bool result)
		{
			stubbedHasContentResults.Add(text, result);
		}

		public bool HasCss(string cssSelector)
		{
			hasCssQueries.Add(cssSelector);
			return stubbedHasCssResults[cssSelector];
		}

		public void StubHasCss(string cssSelector, bool result)
		{
			stubbedHasCssResults.Add(cssSelector, result);
		}

		public bool HasXPath(string xpath)
		{
			hasXPathQueries.Add(xpath);
			return stubbedHasXPathResults[xpath];
		}

		public Node FindCss(string cssSelector)
		{
			throw new NotImplementedException();
		}

		public Node FindXPath(string xpath)
		{
			throw new NotImplementedException();
		}

		public void StubHasXPath(string xpath, bool result)
		{
			stubbedHasXPathResults.Add(xpath, result);
		}
	}
}