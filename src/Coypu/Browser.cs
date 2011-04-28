using Coypu.Robustness;

namespace Coypu
{
	public static class Browser
	{
		private static Session session;
		public static Session Session
		{
			get
			{
				if (session == null || session.WasDisposed)
				{
					StartNewSession();
				}
				return session;
			}
		}

		private static void StartNewSession()
		{
			session = new Session(NewWebDriver(), new WaitAndRetryRobustWrapper());
		}

		private static Driver NewWebDriver()
		{
			return Configuration.RegisterDriver();

		}

		public static void EndSession()
		{
			session.Dispose();
		}

		public static void ClickButton(string locator)
		{
			session.ClickButton(locator);
		}
	}
}