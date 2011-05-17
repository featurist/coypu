using System;
using System.Collections.Generic;
using System.Threading;
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
            robustWrapper.Robustly(
                () =>
                {
                    var findLink = driver.FindButton(locator);
                    Thread.Sleep(Configuration.WaitBeforeClick);
                    driver.Click(findLink);
                }
            );
		}

		public void ClickLink(string locator)
		{
			robustWrapper.Robustly(
                () =>
                {
                    var findLink = driver.FindLink(locator);
                    Thread.Sleep(Configuration.WaitBeforeClick);
                    driver.Click(findLink);
                }
		    );
		}

		public void Visit(string virtualPath)
		{
			driver.Visit(GetFullyQualifiedUrl(virtualPath));
		}

		private string GetFullyQualifiedUrl(string virtualPath)
		{
			if (Uri.IsWellFormedUriString(virtualPath, UriKind.Absolute))
				return virtualPath;

			virtualPath = virtualPath.TrimStart('/');
			var scheme = Configuration.SSL ? "https" : "http";

			return Configuration.Port == 80
                	? string.Format("{0}://{1}/{2}", scheme, Configuration.AppHost, virtualPath)
                	: string.Format("{0}://{1}:{2}/{3}", scheme, Configuration.AppHost, Configuration.Port, virtualPath);
		}

		public void Click(Element element)
		{
			robustWrapper.Robustly(() => driver.Click(element));
		}

		public Element FindButton(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindButton(locator));
		}

		public Element FindLink(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindLink(locator));
		}

		public Element FindField(string locator)
		{
			return robustWrapper.Robustly(() => driver.FindField(locator));
		}

		public FillInWith FillIn(string locator)
		{
			return new FillInWith(locator, driver, robustWrapper);
		}

		public SelectFrom Select(string option)
		{
			return new SelectFrom(option, driver, robustWrapper);
		}

		public bool HasContent(string cssSelector)
		{
			return robustWrapper.Query(() => driver.HasContent(cssSelector), true);
		}

		public bool HasNoContent(string cssSelector)
		{
			return robustWrapper.Query(() => driver.HasContent(cssSelector), false);
		}

		public bool HasCss(string cssSelector)
		{
			return robustWrapper.Query(() => driver.HasCss(cssSelector), true);
		}

		public bool HasNoCss(string cssSelector)
		{
			return robustWrapper.Query(() => driver.HasCss(cssSelector), false);
		}

		public bool HasXPath(string xpath)
		{
			return robustWrapper.Query(() => driver.HasXPath(xpath), true);
		}

		public bool HasNoXPath(string xpath)
		{
			return robustWrapper.Query(() => driver.HasXPath(xpath), false);
		}

		public Element FindCss(string cssSelector)
		{
			return robustWrapper.Robustly(() => driver.FindCss(cssSelector));
		}

		public Element FindXPath(string xpath)
		{
			return robustWrapper.Robustly(() => driver.FindXPath(xpath));
		}

		public IEnumerable<Element> FindAllCss(string cssSelector)
		{
			return driver.FindAllCss(cssSelector);
		}

		public IEnumerable<Element> FindAllXPath(string xpath)
		{
			return driver.FindAllXPath(xpath);
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
			return robustWrapper.Query(() => driver.HasDialog(withText), true);
		}

		public bool HasNoDialog(string withText)
		{
			return robustWrapper.Query(() => driver.HasDialog(withText), false);
		}

		public void AcceptModalDialog()
		{
			robustWrapper.Robustly(() => driver.AcceptModalDialog());
		}

		public void CancelModalDialog()
		{
			robustWrapper.Robustly(() => driver.CancelModalDialog());
		}

		public void WithIndividualTimeout(TimeSpan individualTimeout, Action action)
		{
			var defaultTimeout = Configuration.Timeout;
			Configuration.Timeout = individualTimeout;
			try
			{
				action();
			}
			finally
			{
				Configuration.Timeout = defaultTimeout;
			}
		}

		public void Within(Func<Element> findScope, Action doThis)
		{
			try
			{
				driver.SetScope(findScope);
				doThis();
			}
			finally
			{
				driver.ClearScope();
			}
		}

		public void ClickButton(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
		{
			robustWrapper.TryUntil(() => ClickButton(locator), until, waitBetweenRetries);
		}

		public void ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
		{
			robustWrapper.TryUntil(() => ClickLink(locator), until, waitBetweenRetries);
		}

		public string ExecuteScript(string javascript)
		{
			return driver.ExecuteScript(javascript);
		}
	}
}