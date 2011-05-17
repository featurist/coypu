using System.Threading;

namespace Coypu
{
	public class Clicker
	{
		private readonly Driver driver;

		public Clicker(Driver driver)
		{
			this.driver = driver;
		}

		public void FindAndClickButton(string locator)
		{
			var findLink = driver.FindButton(locator);
			Thread.Sleep(Configuration.WaitBeforeClick);
			driver.Click(findLink);
		}

		public void FindAndClickLink(string locator)
		{
			var findLink = driver.FindLink(locator);
			Thread.Sleep(Configuration.WaitBeforeClick);
			driver.Click(findLink);
		}
	}
}