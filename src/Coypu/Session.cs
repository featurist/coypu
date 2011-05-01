using System;
using System.Collections.Generic;
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

		public object Native
		{
			get { return driver.Native; }
		}

		public Session(Driver driver, RobustWrapper robustWrapper)
		{
			this.robustWrapper = robustWrapper;
			this.driver = driver;
		}

		public void Dispose()
		{
			if (WasDisposed) return;
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

		public Node FindField(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindField(locator));
		}

		public FillInWith FillIn(string locator)
		{
			return new FillInWith(locator, driver, robustWrapper);
		}

		public SelectFrom Select(string option)
		{
			return new SelectFrom(option,driver,robustWrapper);
		}

		public bool HasContent(string cssSelector)
		{
			return robustWrapper.WaitFor(() => driver.HasContent(cssSelector), true);
		}

		public bool HasNoContent(string cssSelector)
		{
			return robustWrapper.WaitFor(() => driver.HasContent(cssSelector), false);
		}

		public bool HasCss(string cssSelector)
		{
			return robustWrapper.WaitFor(() => driver.HasCss(cssSelector), true);
		}

		public bool HasNoCss(string cssSelector)
		{
			return robustWrapper.WaitFor(() => driver.HasCss(cssSelector), false);
		}

		public bool HasXPath(string xpath)
		{
			return robustWrapper.WaitFor(() => driver.HasXPath(xpath), true);
		}

		public bool HasNoXPath(string xpath)
		{
			return robustWrapper.WaitFor(() => driver.HasXPath(xpath), false);
		}

		public Node FindCss(string cssSelector)
		{
			return robustWrapper.Robustly(() => driver.FindCss(cssSelector));
		}

		public Node FindXPath(string xpath)
		{
			return robustWrapper.Robustly(() => driver.FindXPath(xpath));
		}

		public IEnumerable<Node> FindAllCss(string cssSelector)
		{
			return robustWrapper.Robustly(() => driver.FindAllCss(cssSelector));
		}

		public IEnumerable<Node> FindAllXPath(string xpath)
		{
			return robustWrapper.Robustly(() => driver.FindAllXPath(xpath));
		}

		public void Check(string locator)
		{
			robustWrapper.Robustly(() => driver.Check(driver.FindField(locator)));
		}

		public void Uncheck(string locator)
		{
			robustWrapper.Robustly(() => driver.Uncheck(driver.FindField(locator)));
		}

		public void Choose(string locator)
		{
			robustWrapper.Robustly(() => driver.Choose(driver.FindField(locator)));
		}

		public bool HasDialog(string withText)
		{
			return robustWrapper.WaitFor(() => driver.HasDialog(withText),true);
		}

		public bool HasNoDialog(string withText)
		{
			return robustWrapper.WaitFor(() => driver.HasDialog(withText), false);
		}
	}
}