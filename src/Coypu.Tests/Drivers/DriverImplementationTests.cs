using System.IO;
using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.Tests.Drivers
{
	public abstract class DriverImplementationTests
	{
		private Driver driver;
		
		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			driver = GetDriver();
		}

		[SetUp]
		public void SetUp()
		{
			driver.Visit(new FileInfo(INTERACTION_TESTS_PAGE).FullName);
		}

		[TestFixtureTearDown]
		public void Dispose()
		{
			driver.Dispose();
		}

		protected abstract Driver GetDriver();

		private const string INTERACTION_TESTS_PAGE = @"..\..\Drivers\html\InteractionTestsPage.htm";

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_text()
		{
			Assert.That(driver.FindButton("I am the first button").Id, Is.EqualTo("firstButtonId"));
			Assert.That(driver.FindButton("I am the second button").Id, Is.EqualTo("secondButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_id()
		{
			Assert.That(driver.FindButton("firstButtonId").Text, Is.EqualTo("I am the first button"));
			Assert.That(driver.FindButton("thirdButtonId").Text, Is.EqualTo("I am the third button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_name()
		{
			Assert.That(driver.FindButton("secondButtonName").Text, Is.EqualTo("I am the second button"));
			Assert.That(driver.FindButton("thirdButtonName").Text, Is.EqualTo("I am the third button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_value()
		{
			Assert.That(driver.FindButton("I am the first input button").Id, Is.EqualTo("firstInputButtonId"));
			Assert.That(driver.FindButton("I am the second input button").Id, Is.EqualTo("secondInputButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_id()
		{
			Assert.That(driver.FindButton("firstInputButtonId").Value, Is.EqualTo("I am the first input button"));
			Assert.That(driver.FindButton("thirdInputButtonId").Value, Is.EqualTo("I am the third input button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_name()
		{
			Assert.That(driver.FindButton("secondInputButtonId").Value, Is.EqualTo("I am the second input button"));
			Assert.That(driver.FindButton("thirdInputButtonName").Value, Is.EqualTo("I am the third input button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_value()
		{
			Assert.That(driver.FindButton("I am the first submit button").Id, Is.EqualTo("firstSubmitButtonId"));
			Assert.That(driver.FindButton("I am the second submit button").Id, Is.EqualTo("secondSubmitButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_id()
		{
			Assert.That(driver.FindButton("firstSubmitButtonId").Value, Is.EqualTo("I am the first submit button"));
			Assert.That(driver.FindButton("thirdSubmitButtonId").Value, Is.EqualTo("I am the third submit button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_name()
		{
			Assert.That(driver.FindButton("secondSubmitButtonId").Value, Is.EqualTo("I am the second submit button"));
			Assert.That(driver.FindButton("thirdSubmitButtonName").Value, Is.EqualTo("I am the third submit button"));
		}

		[Test]
		public void FindButton_should_not_find_text_inputs()
		{
			Assert.Throws<MissingHtmlException>(() => driver.FindButton("firstTextInputId"));
		}

		[Test]
		public void FindButton_should_not_find_hidden_inputs()
		{
			Assert.Throws<MissingHtmlException>(() => driver.FindButton("firstHiddenInputId"));
		}

		[Test]
		public void FindLink_should_find_link_by_text()
		{
			Assert.That(driver.FindLink("I am the first link").Id == "firstLinkId");
			Assert.That(driver.FindLink("I am the second link").Id == "secondLinkId");
		}

		[Test]
		public void FindLink_should_find_only_find_links()
		{
			Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am not a link"));
		}

		[Test]
		public void When_clicking_It_should_click_the_underlying_node()
		{
			var node = driver.FindButton("clickMeTest");
			Assert.That(driver.FindButton("clickMeTest").Text, Is.EqualTo("Click me"));
			driver.Click(node);
			Assert.That(driver.FindButton("clickMeTest").Text, Is.EqualTo("Click me - clicked"));
		}

		[Test]
		public void FindTextField_should_find_text_field_by_label_text_by_for_attribute()
		{
			Assert.That(driver.FindTextField("I am the text field linked by for").Id, Is.EqualTo("forLabeledTextFieldId"));
		}

		[Test]
		public void FindTextField_should_find_text_field_by_container_label()
		{
			Assert.That(driver.FindTextField("I am the text field in a label container").Id, Is.EqualTo("containerLabeledTextFieldId"));
		}

		[Test]
		public void FindTextField_should_find_text_field_by_id()
		{
			Assert.That(driver.FindTextField("containerLabeledTextFieldId").Value, Is.EqualTo("text field two val"));
		}

		[Test]
		public void FindTextField_should_find_text_field_by_name()
		{
			Assert.That(driver.FindTextField("containerLabeledTextFieldName").Value, Is.EqualTo("text field two val"));
		}

	}
}