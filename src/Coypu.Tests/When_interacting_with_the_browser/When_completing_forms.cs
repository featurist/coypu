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
		public void When_filling_in_a_text_field_It_finds_field_and_sets_value_robustly()
		{
			var element = new StubElement();
			driver.StubField("Some field locator", element);

			session.FillIn("Some field locator").With("some value for the field");

			Assert.That(driver.SetFields, Has.No.Member(element));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.SetFields.Keys, Has.Member(element));
			Assert.That(driver.SetFields[element], Is.EqualTo("some value for the field"));
		}

		[Test]
		public void When_filling_in_a_text_field_It_clicks_to_ensure_focus()
		{
			var element = new StubElement();
			driver.StubField("Some field locator", element);

			session.FillIn("Some field locator").With("some value for the field");

			Assert.That(driver.ClickedElements,Is.Empty);
			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ClickedElements, Has.Member(element));
		}

		[Test]
		public void When_selecting_an_option_It_finds_field_and_selects_option_robustly()
		{
			var element = new StubElement();
			driver.StubField("Some select field locator", element);

			session.Select("some option to select").From("Some select field locator");

			Assert.That(driver.SelectedOptions, Has.No.Member(element));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.SelectedOptions.Keys, Has.Member(element));
			Assert.That(driver.SelectedOptions[element], Is.EqualTo("some option to select"));
		}

		[Test]
		public void When_checking_a_checkbox_It_find_fields_and_checks_robustly()
		{
			var element = new StubElement();
			driver.StubField("Some checkbox locator", element);

			session.Check("Some checkbox locator");

			Assert.That(driver.CheckedElements, Has.No.Member(element));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.CheckedElements, Has.Member(element));
		}

		[Test]
		public void When_unchecking_a_checkbox_It_finds_field_and_unchecks_robustly()
		{
			var element = new StubElement();
			driver.StubField("Some checkbox locator", element);

			session.Uncheck("Some checkbox locator");

			Assert.That(driver.UncheckedElements, Has.No.Member(element));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.UncheckedElements, Has.Member(element));
		}

		[Test]
		public void When_choosing_a_radio_button_It_finds_field_and_chooses_robustly()
		{
			var element = new StubElement();
			driver.StubField("Some radio locator", element);

			session.Choose("Some radio locator");

			Assert.That(driver.ChosenElements, Has.No.Member(element));

			spyRobustWrapper.DeferredActions.Single()();

			Assert.That(driver.ChosenElements, Has.Member(element));
		}
	}
}