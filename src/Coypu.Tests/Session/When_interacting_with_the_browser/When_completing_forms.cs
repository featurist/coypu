using System.Linq;
using NUnit.Framework;

namespace Coypu.Tests.Session.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_completing_forms : APITests
	{
		[Test]
		public void When_filling_in_a_text_field_It_should_find_field_and_set_value_robustly()
		{
			var node = new TestNode();
			Driver.StubTextField("Some field locator", node);

			Session.FillIn("Some field locator", "some value for the field");

			Assert.That(Driver.FilledInFields, Has.No.Member(node));
			
			SpyRobustWrapper.DeferredActions.Single()();

			Assert.That(Driver.FilledInFields.ContainsKey(node));
			Assert.That(Driver.FilledInFields[node], Is.EqualTo("some value for the field"));
		}
	}
}