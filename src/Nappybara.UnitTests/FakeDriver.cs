using System.Collections.Generic;

namespace Nappybara.UnitTests
{
	public class FakeDriver : Driver
	{
		private readonly Dictionary<string, Node> buttons = new Dictionary<string, Node>();
		private readonly IList<Node> clickedNodes = new List<Node>();
		private readonly Dictionary<string, Node> links = new Dictionary<string, Node>();
		private readonly IList<string> visits = new List<string>();

		public IEnumerable<Node> ClickedNodes
		{
			get { return clickedNodes; }
		}

		public IEnumerable<string> Visits
		{
			get { return visits; }
		}

		public Node FindButton(string locator)
		{
			return buttons[locator];
		}

		public Node FindLink(string locator)
		{
			return links[locator];
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
	}
}