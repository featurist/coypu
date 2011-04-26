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

		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			session = new Session(driver, null);
		}

		[Test]
		public void Visit_should_pass_message_to_the_driver()
		{
			session.Visit("http://visit.me");

			Assert.That(driver.Visits.Single(), Is.EqualTo("http://visit.me"));
		}
	}
}