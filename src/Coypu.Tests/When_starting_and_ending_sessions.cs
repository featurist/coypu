using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests
{
	[TestFixture]
	public class When_starting_and_ending_sessions
	{
		[SetUp]
		public void SetUp()
		{
			Configuration.RegisterDriver = () => new FakeDriver();
		}

		[TearDown]
		public void TearDown()
		{
			Browser.EndSession();
		}

		[Test]
		public void A_session_is_always_available()
		{
			Assert.That(Browser.Session, Is.TypeOf(typeof (Session)));

			Browser.EndSession();

			Assert.That(Browser.Session, Is.TypeOf(typeof (Session)));
		}

		[Test]
		public void After_end_a_new_session_is_available()
		{
			var firstSession = Browser.Session;
			Assert.That(firstSession, Is.TypeOf(typeof (Session)));

			Browser.EndSession();

			Assert.That(Browser.Session, Is.Not.SameAs(firstSession));
		}

		[Test]
		public void After_disposing_the_session_a_new_session_is_available()
		{
			Session firstSession;
			using (var session = Browser.Session)
			{
				firstSession = session;
			}
			Assert.That(Browser.Session, Is.Not.SameAs(firstSession));
		}

		[Test]
		public void A_session_gets_its_driver_from_config()
		{
			Configuration.RegisterDriver = () => new FakeDriver();
			Assert.That(Browser.Session.Driver, Is.TypeOf(typeof (FakeDriver)));

			Browser.EndSession();

			Configuration.RegisterDriver = () => new StubDriver();
			Assert.That(Browser.Session.Driver, Is.TypeOf(typeof (StubDriver)));
		}

		[Test]
		public void Session_exposes_native_driver_if_you_really_need_it()
		{
			Configuration.RegisterDriver = () => new FakeDriver();
			Assert.That(Browser.Session.Native, Is.EqualTo("Native driver on fake driver"));

			Browser.EndSession();

			Configuration.RegisterDriver = () => new StubDriver();
			Assert.That(Browser.Session.Native, Is.EqualTo("Native driver on stub driver"));
		}

	}
}