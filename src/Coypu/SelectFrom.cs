using Coypu.Robustness;

namespace Coypu
{
	public class SelectFrom
	{
		private readonly string option;
		private readonly Driver driver;
		private readonly RobustWrapper robustWrapper;

		public SelectFrom(string option, Driver driver, RobustWrapper robustWrapper)
		{
			this.option = option;
			this.driver = driver;
			this.robustWrapper = robustWrapper;
		}

		public void From(string locator)
		{
			robustWrapper.Robustly(
				() => {
						driver.Click(driver.FindField(locator));
						  driver.Select(driver.FindField(locator), option);
				});
		}
	}
}