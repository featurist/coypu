using System;
using System.IO;
using NUnit.Framework;

namespace Coypu.DriverImplementationTests
{
	public abstract class DriverImplementationTests
	{
		[SetUp]
		public void SetUp()
		{
			Driver.Visit(new FileInfo(INTERACTION_TESTS_PAGE).FullName);
		}

		[TestFixtureTearDown]
		public abstract void Dispose();

		private const string INTERACTION_TESTS_PAGE = @"..\..\html\InteractionTestsPage.htm";
		protected abstract Driver Driver { get; }

		// NEXT: Find link by text

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_text()
		{
			Assert.That(Driver.FindButton("I am the first button").Id, Is.EqualTo("firstButtonId"));
			Assert.That(Driver.FindButton("I am the second button").Id, Is.EqualTo("secondButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_id()
		{
			Assert.That(Driver.FindButton("firstButtonId").Text, Is.EqualTo("I am the first button"));
			Assert.That(Driver.FindButton("thirdButtonId").Text, Is.EqualTo("I am the third button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_name()
		{
			Assert.That(Driver.FindButton("secondButtonName").Text, Is.EqualTo("I am the second button"));
			Assert.That(Driver.FindButton("thirdButtonName").Text, Is.EqualTo("I am the third button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_value()
		{
			Assert.That(Driver.FindButton("I am the first input button").Id, Is.EqualTo("firstInputButtonId"));
			Assert.That(Driver.FindButton("I am the second input button").Id, Is.EqualTo("secondInputButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_id()
		{
			Assert.That(Driver.FindButton("firstInputButtonId").Value, Is.EqualTo("I am the first input button"));
			Assert.That(Driver.FindButton("thirdInputButtonId").Value, Is.EqualTo("I am the third input button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_name()
		{
			Assert.That(Driver.FindButton("secondInputButtonId").Value, Is.EqualTo("I am the second input button"));
			Assert.That(Driver.FindButton("thirdInputButtonName").Value, Is.EqualTo("I am the third input button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_value()
		{
			Assert.That(Driver.FindButton("I am the first submit button").Id, Is.EqualTo("firstSubmitButtonId"));
			Assert.That(Driver.FindButton("I am the second submit button").Id, Is.EqualTo("secondSubmitButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_id()
		{
			Assert.That(Driver.FindButton("firstSubmitButtonId").Value, Is.EqualTo("I am the first submit button"));
			Assert.That(Driver.FindButton("thirdSubmitButtonId").Value, Is.EqualTo("I am the third submit button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_name()
		{
			Assert.That(Driver.FindButton("secondSubmitButtonId").Value, Is.EqualTo("I am the second submit button"));
			Assert.That(Driver.FindButton("thirdSubmitButtonName").Value, Is.EqualTo("I am the third submit button"));
		}

		[Test]
		public void FindButton_should_not_find_text_inputs()
		{
			AssertElementNotFound(() => Driver.FindButton("firstTextInputId"));
		}

		[Test]
		public void FindButton_should_not_find_hidden_inputs()
		{
			AssertElementNotFound(() => Driver.FindButton("firstHiddenInputId"));
		}

		[Test]
		public void When_clicking_It_should_click_the_underlying_node()
		{
			var node = Driver.FindButton("clickMeTest");
			Assert.That(Driver.FindButton("clickMeTest").Text, Is.EqualTo("Click me"));
			Driver.Click(node);
			Assert.That(Driver.FindButton("clickMeTest").Text, Is.EqualTo("Click me - clicked"));
		}

		private void AssertElementNotFound(Func<Node> find)
		{
			var thrown = false;
			try
			{
				find();
			}
			catch (Exception)
			{
				thrown = true;
			}
			if (!thrown)
				Assert.Fail("Expected an element not found exception of some kind");
		}
	}
}