using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_visiting
	{
		protected FakeDriver Driver;
		protected SpyRobustWrapper SpyRobustWrapper;
		protected Session Session;

		[Test]
		public void Visit_should_pass_message_to_the_driver()
		{
			Session.Visit("http://visit.me");

			Assert.That(Driver.Visits.Single(), Is.EqualTo("http://visit.me"));
		}

		[SetUp]
		public void SetUp()
		{
			Driver = new FakeDriver();
			SpyRobustWrapper = new SpyRobustWrapper();
			Session = new Session(Driver, SpyRobustWrapper);
		}
	}
}