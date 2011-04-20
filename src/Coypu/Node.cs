namespace Coypu
{
	public class Node
	{
		private readonly Driver driver;

		public Node(Driver driver)
		{
			this.driver = driver;
		}

		public string Id { get; set; }
		public string Text { get; set; }
		public string Value { get; set; }
		public object UnderlyingNode { get; set; }

		public void Click()
		{
			driver.Click(this);
		}
	}
}