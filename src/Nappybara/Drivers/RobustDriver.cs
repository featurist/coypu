using Nappybara.Robustness;

namespace Nappybara.Drivers
{
	public class RobustDriver : Driver
	{
		private readonly Driver driver;
		private readonly RobustWrapper robustWrapper;

		public RobustDriver(Driver driver, RobustWrapper robustWrapper)
		{
			this.driver = driver;
			this.robustWrapper = robustWrapper;
		}

		public Node FindButton(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindButton(locator));
		}

		public Node FindLink(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindLink(locator));
		}

		public void Click(Node node)
		{
			robustWrapper.Robustly(() => driver.Click(node));
		}

		public void Visit(string url)
		{
			driver.Visit(url);
		}
	}
}