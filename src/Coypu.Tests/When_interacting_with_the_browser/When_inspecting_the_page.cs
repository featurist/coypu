using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_inspecting_the_page
	{
		private FakeDriver driver;
		private SpyRobustWrapper spyRobustWrapper;
		private Session session;

		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			spyRobustWrapper = new SpyRobustWrapper();
			session = new Session(driver, spyRobustWrapper);
		}

		[Test]
		public void HasContent_should_return_result_from_wrapper()
		{
			const string textToFind = "#some css.selector";

			spyRobustWrapper.AlwaysReturnFromWaitFor(true, true);
			Assert.That(session.HasContent(textToFind), Is.True);

			spyRobustWrapper.AlwaysReturnFromWaitFor(true, false);
			Assert.That(session.HasContent(textToFind), Is.False);
		}

		[Test]
		public void HasContent_should_wait_for_robustly()
		{
			const string textToFind = "#some css.selector";
			driver.StubHasContent(textToFind, true);
			spyRobustWrapper.AlwaysReturnFromWaitFor(true, true);

			session.HasContent(textToFind);

			Assert.That(driver.HasContentQueries, Is.Empty);
			spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(driver.HasContentQueries.Single(), Is.EqualTo(textToFind));
		}

		[Test]
		public void HasNoContent_should_return_result_from_wrapper()
		{
			const string textToFind = "#some css.selector";

			spyRobustWrapper.AlwaysReturnFromWaitFor(false, true);
			Assert.That(session.HasNoContent(textToFind), Is.True);

			spyRobustWrapper.AlwaysReturnFromWaitFor(false, false);
			Assert.That(session.HasNoContent(textToFind), Is.False);
		}

		[Test]
		public void HasNoContent_should_wait_for_robustly()
		{
			const string textToFind = "#some css.selector";
			driver.StubHasContent(textToFind, false);
			spyRobustWrapper.AlwaysReturnFromWaitFor(false, false);

			session.HasNoContent(textToFind);

			Assert.That(driver.HasContentQueries, Is.Empty);
			spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(driver.HasContentQueries.Single(), Is.EqualTo(textToFind));
		}

		[Test]
		public void HasCss_should_return_result_from_wrapper()
		{
			const string cssSelector = "#some css.selector";

			spyRobustWrapper.AlwaysReturnFromWaitFor(true, true);
			Assert.That(session.HasCss(cssSelector), Is.True);

			spyRobustWrapper.AlwaysReturnFromWaitFor(true, false);
			Assert.That(session.HasCss(cssSelector), Is.False);
		}

		[Test]
		public void HasCss_should_wait_for_robustly()
		{
			const string cssSelector = "#some css.selector";
			driver.StubHasCss(cssSelector, true);
			spyRobustWrapper.AlwaysReturnFromWaitFor(true, true);

			session.HasCss(cssSelector);

			Assert.That(driver.HasCssQueries, Is.Empty);
			spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(driver.HasCssQueries.Single(), Is.EqualTo(cssSelector));
		}

		[Test]
		public void HasNoCss_should_return_result_from_wrapper()
		{
			const string cssSelector = "#some css.selector";

			spyRobustWrapper.AlwaysReturnFromWaitFor(false, true);
			Assert.That(session.HasNoCss(cssSelector), Is.True);

			spyRobustWrapper.AlwaysReturnFromWaitFor(false, false);
			Assert.That(session.HasNoCss(cssSelector), Is.False);
		}

		[Test]
		public void HasNoCss_should_wait_for_robustly()
		{
			const string cssSelector = "#some css.selector";
			driver.StubHasCss(cssSelector, false);
			spyRobustWrapper.AlwaysReturnFromWaitFor(false, false);

			session.HasNoCss(cssSelector);

			Assert.That(driver.HasCssQueries, Is.Empty);
			spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(driver.HasCssQueries.Single(), Is.EqualTo(cssSelector));
		}

		[Test]
		public void HasXPath_should_return_result_from_wrapper()
		{
			const string xpath = "//some/xpath[@selector]";

			spyRobustWrapper.AlwaysReturnFromWaitFor(true, true);
			Assert.That(session.HasXPath(xpath), Is.True);

			spyRobustWrapper.AlwaysReturnFromWaitFor(true, false);
			Assert.That(session.HasXPath(xpath), Is.False);
		}

		[Test]
		public void HasXPath_should_wait_for_robustly()
		{
			const string xpath = "//some/xpath[@selector]";
			driver.StubHasXPath(xpath, true);
			spyRobustWrapper.AlwaysReturnFromWaitFor(true, true);

			session.HasXPath(xpath);

			Assert.That(driver.HasXPathQueries, Is.Empty);
			spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(driver.HasXPathQueries.Single(), Is.EqualTo(xpath));
		}

		[Test]
		public void HasNoXPath_should_return_result_from_wrapper()
		{
			const string xpath = "//some/xpath[@selector]";

			spyRobustWrapper.AlwaysReturnFromWaitFor(false, true);
			Assert.That(session.HasNoXPath(xpath), Is.True);

			spyRobustWrapper.AlwaysReturnFromWaitFor(false, false);
			Assert.That(session.HasNoXPath(xpath), Is.False);
		}

		[Test]
		public void HasNoXPath_should_wait_for_robustly()
		{
			const string xpath = "//some/xpath[@selector]";
			driver.StubHasXPath(xpath, false);
			spyRobustWrapper.AlwaysReturnFromWaitFor(false, false);

			session.HasNoXPath(xpath);

			Assert.That(driver.HasXPathQueries, Is.Empty);
			spyRobustWrapper.DeferredWaitForQueries.Single()();
			Assert.That(driver.HasXPathQueries.Single(), Is.EqualTo(xpath));
		}
	}
}