using System;

namespace Nappybara.API
{
    public class RobustDriver : Driver
    {
        private readonly Driver driver;
        private readonly Robustness robustness;

        public RobustDriver(Driver driver, Robustness robustness)
        {
            this.driver = driver;
            this.robustness = robustness;
        }

        public Node FindButton(string locator)
        {
            return robustness.Robustly(() => driver.FindButton(locator));
        }

    	public Node FindLink(string locator)
    	{
			return robustness.Robustly(() => driver.FindLink(locator));
    	}

    	public void Click(Node node)
        {
            robustness.Robustly(() => driver.Click(node));
        }

    	public void Visit(string url)
    	{
			driver.Visit(url);
    	}
    }
}