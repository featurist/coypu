using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_completing_forms
	{
		protected FakeDriver Driver;
		protected SpyRobustWrapper SpyRobustWrapper;
		protected Session Session;

		[SetUp]
		public void SetUp()
		{
			Driver = new FakeDriver();
			SpyRobustWrapper = new SpyRobustWrapper();
			Session = new Session(Driver, SpyRobustWrapper);
		}

		[Test]
		public void When_filling_in_a_text_field_It_should_find_field_and_set_value_robustly()
		{
			var node = new StubNode();
			Driver.StubField("Some field locator", node);

			Session.FillIn("Some field locator").With("some value for the field");

			Assert.That(Driver.SetFields, Has.No.Member(node));

			SpyRobustWrapper.DeferredActions.Single()();

			Assert.That(Driver.SetFields.ContainsKey(node));
			Assert.That(Driver.SetFields[node], Is.EqualTo("some value for the field"));
		}

		[Test]
		public void When_selecting_an_option_It_should_find_field_and_select_option_robustly()
		{
			var node = new StubNode();
			Driver.StubField("Some select field locator", node);

			Session.Select("some option to select").From("Some select field locator");

			Assert.That(Driver.SetFields, Has.No.Member(node));

			SpyRobustWrapper.DeferredActions.Single()();

			Assert.That(Driver.SelectedOptions.ContainsKey(node));
			Assert.That(Driver.SelectedOptions[node], Is.EqualTo("some option to select"));
		}
	}
}