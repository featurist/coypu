using System.IO;
using NUnit.Framework;

namespace Nappybara.API.UnitTests
{
	[TestFixture]
	public abstract class RealDriverImplementationTests
	{
		private const string INTERACTION_TESTS_PAGE = @"html\InteractionTestsPage.htm";
		protected abstract Driver _driver { get; }

		[SetUp]
		public void SetUp()
		{
			_driver.Visit(new FileInfo(INTERACTION_TESTS_PAGE).FullName);
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_text()
		{
			Assert.That(_driver.FindButton("I am the first button").Id, Is.EqualTo("firstButtonId"));
			Assert.That(_driver.FindButton("I am the second button").Id, Is.EqualTo("secondButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_id_if_not_by_text()
		{
			Assert.That(_driver.FindButton("firstButtonId").Text, Is.EqualTo("I am the first button"));
			Assert.That(_driver.FindButton("thirdButtonId").Text, Is.EqualTo("I am the third button"));
		}

	}
}