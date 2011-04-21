using Coypu.Tests.TestDoubles;
using Coypu.UnitTests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.Session
{
	[TestFixture]
	public class When_managing_sessions
	{
		[SetUp]
		public void SetUp()
		{
			Configuration.WebDriver = typeof(FakeDriver);
		}

		[TearDown]
		public void TearDown()
		{
			Browser.EndSession();
		}

		[Test]
		public void A_session_is_always_available()
		{
			Assert.That(Browser.Session, Is.TypeOf(typeof (Coypu.Session)));

			Browser.EndSession();

			Assert.That(Browser.Session, Is.TypeOf(typeof (Coypu.Session)));
		}

		[Test]
		public void After_end_a_new_session_is_available()
		{
			var firstSession = Browser.Session;
			Assert.That(firstSession, Is.TypeOf(typeof (Coypu.Session)));

			Browser.EndSession();

			Assert.That(Browser.Session, Is.Not.SameAs(firstSession));
		}

		[Test]
		public void After_disposing_the_session_a_new_session_is_available()
		{
			Coypu.Session firstSession;
			using (var session = Browser.Session)
			{
				firstSession = session;
			}
			Assert.That(Browser.Session, Is.Not.SameAs(firstSession));
		}

		[Test]
		public void A_session_gets_its_driver_from_config()
		{
			Configuration.WebDriver = typeof(FakeDriver);
			Assert.That(Browser.Session.Driver, Is.TypeOf(typeof(FakeDriver)));

			Browser.EndSession();

			Configuration.WebDriver = typeof(StubDriver);
			Assert.That(Browser.Session.Driver, Is.TypeOf(typeof(StubDriver)));
		}
	}
}