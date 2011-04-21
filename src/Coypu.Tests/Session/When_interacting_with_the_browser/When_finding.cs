using NUnit.Framework;

namespace Coypu.Tests.Session.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_finding : APITests
	{
		[Test]
		public void FindButton_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(Session.FindButton, Driver.StubButton);
		}

		[Test]
		public void FindLink_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(Session.FindLink, Driver.StubLink);
		}

		[Test]
		public void FindTextField_should_make_robust_call_to_underlying_driver()
		{
			Should_find_robustly(Session.FindTextField, Driver.StubTextField);
		}
	}
}