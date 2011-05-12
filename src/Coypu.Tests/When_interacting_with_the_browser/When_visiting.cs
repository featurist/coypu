using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_visiting
	{
		private FakeDriver driver;
		private Session session;

		[Test]
		public void It_should_pass_message_directly_to_the_driver()
		{
			session.Visit("http://visit.me");

			Assert.That(driver.Visits.SingleOrDefault(), Is.Not.Null);
		}

		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			session = new Session(driver, null);
		}

		[TearDown]
		public void TearDown()
		{
			Configuration.AppHost = default(string);
			Configuration.Port = default(int);
			Configuration.SSL = default(bool);
		}

		[Test]
		public void It_should_form_url_from_host_port_and_virtual_path()
		{
			Configuration.AppHost = "im.theho.st";
			Configuration.Port = 81;

			session.Visit("/visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st:81/visit/me"));
		}

		[Test]
		public void It_should_default_to_localhost()
		{
			Configuration.Port = 81;

			session.Visit("/visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://localhost:81/visit/me"));
		}

		[Test]
		public void It_should_default_to_port_80()
		{
			Configuration.AppHost = "im.theho.st";

			session.Visit("/visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st/visit/me"));
		}

		[Test]
		public void It_should_handle_trailing_slashes_in_host()
		{
			Configuration.AppHost = "im.theho.st/";

			session.Visit("/visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st/visit/me"));
		}

		[Test]
		public void It_should_handle_missing_leading_slashes_in_virtual_path()
		{
			Configuration.AppHost = "im.theho.st";

			session.Visit("visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st/visit/me"));
		}

		[Test]
		public void It_should_handle_trailing_and_missing_leading_slashes_with_a_port()
		{
			Configuration.AppHost = "im.theho.st/";
			Configuration.Port = 123;

			session.Visit("visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://im.theho.st:123/visit/me"));
		}

		[Test]
		public void It_should_support_SSL()
		{
			Configuration.AppHost = "im.theho.st";
			Configuration.SSL = true;

			session.Visit("/visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("https://im.theho.st/visit/me"));
		}
		[Test]
		public void It_should_support_SSL_with_ports()
		{
			Configuration.AppHost = "im.theho.st";
			Configuration.Port = 321;
			Configuration.SSL = true;

			session.Visit("/visit/me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("https://im.theho.st:321/visit/me"));
		}
	}
}