using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_inspecting_the_page : When_inspecting
	{
		[Test]
		public void HasContent_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, Session.HasContent, Driver.StubHasContent);
		}

		[Test]
		public void HasContent_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, Session.HasContent, Driver.StubHasContent);
		}

		[Test]
		public void HasNoContent_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, Session.HasNoContent, Driver.StubHasContent);
		}

		[Test]
		public void HasNoContent_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, Session.HasNoContent, Driver.StubHasContent);
		}

		[Test]
		public void HasCss_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, Session.HasCss, Driver.StubHasCss);
		}

		[Test]
		public void HasCss_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, Session.HasCss, Driver.StubHasCss);
		}

		[Test]
		public void HasNoCss_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, Session.HasNoCss, Driver.StubHasCss);
		}

		[Test]
		public void HasNoCss_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, Session.HasNoCss, Driver.StubHasCss);
		}

		[Test]
		public void HasXPath_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, Session.HasXPath, Driver.StubHasXPath);
		}

		[Test]
		public void HasXPath_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, Session.HasXPath, Driver.StubHasXPath);
		}

		[Test]
		public void HasNoXPath_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, Session.HasNoXPath, Driver.StubHasXPath);
		}

		[Test]
		public void HasNoXPath_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, Session.HasNoXPath, Driver.StubHasXPath);
		}
	}
}