using System;

namespace Nappybara.API
{
    public class Session
    {
        private readonly Driver driver;

        public Session(Driver driver)
        {
            this.driver = driver;
        }

        public void ClickButton(string locator)
        {
        	driver.FindButton(locator).Click();
        }

    	public void ClickLink(string locator)
    	{
    		driver.FindLink(locator).Click();
    	}
    }
}