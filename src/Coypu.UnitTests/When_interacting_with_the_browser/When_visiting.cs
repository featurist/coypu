using System.Linq;
using NUnit.Framework;

namespace Coypu.UnitTests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_visiting : APITests
	{
		[Test]
		public void Visit_should_pass_message_to_the_driver()
		{
			new Session(Driver, null).Visit("http://visit.me");

			Assert.That(Driver.Visits.Single(), Is.EqualTo("http://visit.me"));
		}
	}
}