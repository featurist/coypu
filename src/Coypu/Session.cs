using System;
using Coypu.Robustness;

namespace Coypu
{
	public class Session : IDisposable
	{
		private readonly Driver driver;
		private readonly RobustWrapper robustWrapper;
		public bool WasDisposed { get; private set; }

		public Driver Driver
		{
			get { return driver; }
		}

		public Session(Driver driver, RobustWrapper robustWrapper)
		{
			this.robustWrapper = robustWrapper;
			this.driver = driver;
		}

		public void Dispose()
		{
			driver.Dispose();
			WasDisposed = true;
		}

		public void ClickButton(string locator)
		{
			robustWrapper.Robustly(() => driver.Click(driver.FindButton(locator)));
		}

		public void ClickLink(string locator)
		{
			robustWrapper.Robustly(() => driver.Click(driver.FindLink(locator)));
		}

		public void Visit(string url)
		{
			driver.Visit(url);
		}

		public void Click(Node node)
		{
			robustWrapper.Robustly(() => driver.Click(node));
		}

		public Node FindButton(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindButton(locator));
		}

		public Node FindLink(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindLink(locator));
		}

		public Node FindTextField(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindTextField(locator));
		}
	}
}