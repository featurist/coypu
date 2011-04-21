namespace Coypu.UnitTests.TestDoubles
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

		public Node FindTextField(string locator)
		{
			return null;
		}

		public void Click(Node node)
		{
		}

		public void Visit(string url)
		{
		}
	}
}