namespace Nappybara.API
{
	public class Node
	{
		private readonly Driver driver;

		public Node(Driver driver)
		{
			this.driver = driver;
		}

		public string Id { get; private set; }
		public string Text { get; private set; }

		public void Click()
		{
			driver.Click(this);
		}
	}
}