using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_completing_forms
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
		public void When_filling_in_a_text_field_It_should_find_field_and_set_value_robustly()
		{
			var node = new StubNode();
			driver.StubField("Some field locator", node);

			session.FillIn("Some field locator").With("some value for the field");

			Assert.That(driver.SetFields, Has.No.Member(node));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.SetFields.Keys, Has.Member(node));
			Assert.That(driver.SetFields[node], Is.EqualTo("some value for the field"));
		}

		[Test]
		public void When_filling_in_a_text_field_It_should_click_to_ensure_focus()
		{
			var node = new StubNode();
			driver.StubField("Some field locator", node);

			session.FillIn("Some field locator").With("some value for the field");

			Assert.That(driver.ClickedNodes,Is.Empty);
			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

		[Test]
		public void When_selecting_an_option_It_should_find_field_and_select_option_robustly()
		{
			var node = new StubNode();
			driver.StubField("Some select field locator", node);

			session.Select("some option to select").From("Some select field locator");

			Assert.That(driver.SelectedOptions, Has.No.Member(node));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.SelectedOptions.Keys, Has.Member(node));
			Assert.That(driver.SelectedOptions[node], Is.EqualTo("some option to select"));
		}

		[Test]
		public void When_selecting_an_option_It_should_click_to_ensure_focus()
		{
			var node = new StubNode();
			driver.StubField("Some select field locator", node);

			session.Select("some option to select").From("Some select field locator");

			Assert.That(driver.ClickedNodes, Has.No.Member(node));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedNodes, Has.Member(node));
		}

        [Test]
        public void When_checking_a_checkbox_It_should_find_field_and_check_robustly()
        {
            var node = new StubNode();
            driver.StubField("Some checkbox locator", node);

            session.Check("Some checkbox locator");

            Assert.That(driver.CheckedNodes, Has.No.Member(node));

            spyRobustWrapper.DeferredActions.Single()();

            Assert.That(driver.CheckedNodes, Has.Member(node));
        }

        [Test]
        public void When_unchecking_a_checkbox_It_should_find_field_and_uncheck_robustly()
        {
            var node = new StubNode();
            driver.StubField("Some checkbox locator", node);

            session.Uncheck("Some checkbox locator");

            Assert.That(driver.UncheckedNodes, Has.No.Member(node));

            spyRobustWrapper.DeferredActions.Single()();

            Assert.That(driver.UncheckedNodes, Has.Member(node));
        }
	}
}