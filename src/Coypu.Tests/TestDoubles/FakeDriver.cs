using System.Collections.Generic;

namespace Coypu.UnitTests.TestDoubles
{
	public class FakeDriver : Driver
	{
		private readonly IList<Node> clickedNodes = new List<Node>();
		private readonly Dictionary<string, Node> buttons = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> links = new Dictionary<string, Node>();
		private readonly Dictionary<string, Node> textFields = new Dictionary<string, Node>();
		private readonly IList<string> visits = new List<string>();
		private readonly IList<string> findButtonRequests = new List<string>();
		private readonly IList<string> findLinkRequests = new List<string>();
		private readonly IList<string> findTextFieldRequests = new List<string>();
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

		public IEnumerable<string> Visits
		{
			get { return visits; }
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

		public Node FindTextField(string locator)
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

		public void StubTextField(string locator, Node node)
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
	}
}