using Coypu.Robustness;

namespace Coypu
{
	public class FillInWith
	{
		private readonly string locator;
		private readonly Driver driver;
		private readonly RobustWrapper robustWrapper;

		public FillInWith(string locator, Driver driver, RobustWrapper robustWrapper)
		{
			this.locator = locator;
			this.driver = driver;
			this.robustWrapper = robustWrapper;
		}

		public void With(string value)
		{
			robustWrapper.Robustly(() => driver.Set(driver.FindField(locator), value));

		}
	}
}