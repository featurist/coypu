using System.IO;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	public abstract class DriverImplementationTests
	{
		private const string INTERACTION_TESTS_PAGE = @"..\..\html\InteractionTestsPage.htm";
		private Driver driver;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			driver = GetDriver();
		}

		public void NewSession()
		{
			driver.Dispose();
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

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_text()
		{
			Assert.That(driver.FindButton("first button").Id, Is.EqualTo("firstButtonId"));
			Assert.That(driver.FindButton("second button").Id, Is.EqualTo("secondButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_id()
		{
			Assert.That(driver.FindButton("firstButtonId").Text, Is.EqualTo("first button"));
			Assert.That(driver.FindButton("thirdButtonId").Text, Is.EqualTo("third button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_button_by_its_name()
		{
			Assert.That(driver.FindButton("secondButtonName").Text, Is.EqualTo("second button"));
			Assert.That(driver.FindButton("thirdButtonName").Text, Is.EqualTo("third button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_value()
		{
			Assert.That(driver.FindButton("first input button").Id, Is.EqualTo("firstInputButtonId"));
			Assert.That(driver.FindButton("second input button").Id, Is.EqualTo("secondInputButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_id()
		{
			Assert.That(driver.FindButton("firstInputButtonId").Value, Is.EqualTo("first input button"));
			Assert.That(driver.FindButton("thirdInputButtonId").Value, Is.EqualTo("third input button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_input_button_by_its_name()
		{
			Assert.That(driver.FindButton("secondInputButtonId").Value, Is.EqualTo("second input button"));
			Assert.That(driver.FindButton("thirdInputButtonName").Value, Is.EqualTo("third input button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_value()
		{
			Assert.That(driver.FindButton("first submit button").Id, Is.EqualTo("firstSubmitButtonId"));
			Assert.That(driver.FindButton("second submit button").Id, Is.EqualTo("secondSubmitButtonId"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_id()
		{
			Assert.That(driver.FindButton("firstSubmitButtonId").Value, Is.EqualTo("first submit button"));
			Assert.That(driver.FindButton("thirdSubmitButtonId").Value, Is.EqualTo("third submit button"));
		}

		[Test]
		public void FindButton_should_find_a_particular_submit_button_by_its_name()
		{
			Assert.That(driver.FindButton("secondSubmitButtonId").Value, Is.EqualTo("second submit button"));
			Assert.That(driver.FindButton("thirdSubmitButtonName").Value, Is.EqualTo("third submit button"));
		}

		[Test]
		public void FindButton_should_find_image_buttons()
		{
			Assert.That(driver.FindButton("firstImageButtonId").Value, Is.EqualTo("first image button"));
			Assert.That(driver.FindButton("secondImageButtonId").Value, Is.EqualTo("second image button"));
			
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
		public void FindButton_should_not_find_invisible_inputs()
		{
			Assert.Throws<MissingHtmlException>(() => driver.FindButton("firstInvisibleInputId"));
		}

		[Test]
		public void FindLink_should_find_link_by_text()
		{
			Assert.That(driver.FindLink("first link").Id == "firstLinkId");
			Assert.That(driver.FindLink("second link").Id == "secondLinkId");
		}

		[Test]
		public void FindLink_should_find_only_find_visible_links()
		{
			Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am an invisible link by display"));
			Assert.Throws<MissingHtmlException>(() => driver.FindLink("I am an invisible link by visibility"));
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
		public void FindField_should_find_field_by_label_text_by_for_attribute()
		{
			Assert.That(driver.FindField("text input field linked by for").Id, Is.EqualTo("forLabeledTextInputFieldId"));
			Assert.That(driver.FindField("password field linked by for").Id, Is.EqualTo("forLabeledPasswordFieldId"));
			Assert.That(driver.FindField("select field linked by for").Id, Is.EqualTo("forLabeledSelectFieldId"));
			Assert.That(driver.FindField("checkbox field linked by for").Id, Is.EqualTo("forLabeledCheckboxFieldId"));
			Assert.That(driver.FindField("radio field linked by for").Id, Is.EqualTo("forLabeledRadioFieldId"));
			Assert.That(driver.FindField("textarea field linked by for").Id, Is.EqualTo("forLabeledTextareaFieldId"));
		}

		[Test]
		public void FindField_should_find_field_by_container_label()
		{
			Assert.That(driver.FindField("text input field in a label container").Id, Is.EqualTo("containerLabeledTextInputFieldId"));
			Assert.That(driver.FindField("password field in a label container").Id, Is.EqualTo("containerLabeledPasswordFieldId"));
			Assert.That(driver.FindField("checkbox field in a label container").Id, Is.EqualTo("containerLabeledCheckboxFieldId"));
			Assert.That(driver.FindField("radio field in a label container").Id, Is.EqualTo("containerLabeledRadioFieldId"));
			Assert.That(driver.FindField("select field in a label container").Id, Is.EqualTo("containerLabeledSelectFieldId"));
			Assert.That(driver.FindField("textarea field in a label container").Id, Is.EqualTo("containerLabeledTextareaFieldId"));
		}

		[Test]
		public void FindField_should_find_text_field_by_placeholder()
		{
			Assert.That(driver.FindField("text input field with a placeholder").Id, Is.EqualTo("textInputFieldWithPlaceholder"));
			Assert.That(driver.FindField("textarea field with a placeholder").Id, Is.EqualTo("textareaFieldWithPlaceholder"));
		}

		[Test]
		public void FindField_should_find_field_by_id()
		{
			Assert.That(driver.FindField("containerLabeledTextInputFieldId").Value, Is.EqualTo("text input field two val"));
			Assert.That(driver.FindField("containerLabeledTextareaFieldId").Value, Is.EqualTo("textarea field two val"));
			Assert.That(driver.FindField("containerLabeledSelectFieldId").Name, Is.EqualTo("containerLabeledSelectFieldName"));
			Assert.That(driver.FindField("containerLabeledCheckboxFieldId").Value, Is.EqualTo("checkbox field two val"));
			Assert.That(driver.FindField("containerLabeledRadioFieldId").Value, Is.EqualTo("radio field two val"));
			Assert.That(driver.FindField("containerLabeledPasswordFieldId").Name, Is.EqualTo("containerLabeledPasswordFieldName"));
		}

		[Test]
		public void FindField_should_find_field_by_name()
		{
			Assert.That(driver.FindField("containerLabeledTextInputFieldName").Value, Is.EqualTo("text input field two val"));
			Assert.That(driver.FindField("containerLabeledTextareaFieldName").Value, Is.EqualTo("textarea field two val"));
			Assert.That(driver.FindField("containerLabeledSelectFieldName").Id, Is.EqualTo("containerLabeledSelectFieldId"));
			Assert.That(driver.FindField("containerLabeledCheckboxFieldName").Value, Is.EqualTo("checkbox field two val"));
			Assert.That(driver.FindField("containerLabeledRadioFieldName").Value, Is.EqualTo("radio field two val"));
			Assert.That(driver.FindField("containerLabeledPasswordFieldName").Id, Is.EqualTo("containerLabeledPasswordFieldId"));
		}

        [Test]
        public void FindField_should_find_radio_button_by_value()
        {
            Assert.That(driver.FindField("radio field one val").Name, Is.EqualTo("forLabeledRadioFieldName"));
            Assert.That(driver.FindField("radio field two val").Name, Is.EqualTo("containerLabeledRadioFieldName"));
        }

	    [Test]
		public void Set_should_set_value_of_text_input_field()
		{
			var textField = driver.FindField("containerLabeledTextInputFieldName");
			driver.Set(textField, "New text input value");

			Assert.That(textField.Value, Is.EqualTo("New text input value"));

			var findAgain = driver.FindField("containerLabeledTextInputFieldName");
			Assert.That(findAgain.Value, Is.EqualTo("New text input value"));
		}

		[Test]
		public void Set_should_set_value_of_textarea_field()
		{
			var textField = driver.FindField("containerLabeledTextareaFieldName");
			driver.Set(textField, "New textarea value");

			Assert.That(textField.Value, Is.EqualTo("New textarea value"));

			var findAgain = driver.FindField("containerLabeledTextareaFieldName");
			Assert.That(findAgain.Value, Is.EqualTo("New textarea value"));
		}

		[Test]
		public void Set_should_select_option_by_text_or_value()
		{
			var textField = driver.FindField("containerLabeledSelectFieldId");
			Assert.That(textField.Value, Is.EqualTo("select2value1"));

			driver.Select(textField, "select two option two");

			var findAgain = driver.FindField("containerLabeledSelectFieldId");
			Assert.That(findAgain.Value, Is.EqualTo("select2value2"));

			driver.Select(textField, "select2value1");

			var andAgain = driver.FindField("containerLabeledSelectFieldId");
			Assert.That(andAgain.Value, Is.EqualTo("select2value1"));
		}

		[Test]
		public void Set_should_fire_change_event_when_selecting_an_option()
		{
			var textField = driver.FindField("containerLabeledSelectFieldId");
			Assert.That(textField.Name, Is.EqualTo("containerLabeledSelectFieldName"));

			driver.Select(textField, "select two option two");

			Assert.That(driver.FindField("containerLabeledSelectFieldId").Name, Is.EqualTo("containerLabeledSelectFieldName - changed"));
		}

		[Test]
		public void Selected_is_text_of_selected_option()
		{
			var textField = driver.FindField("containerLabeledSelectFieldId");
			Assert.That(textField.SelectedOption, Is.EqualTo("select two option one"));

			driver.Select(textField, "select2value2");

			textField = driver.FindField("containerLabeledSelectFieldId");
			Assert.That(textField.SelectedOption, Is.EqualTo("select two option two"));
		}

        [Test]
		public void Check_should_check_an_unchecked_checkbox()
		{
            var checkbox = driver.FindField("uncheckedBox");
            Assert.That(checkbox.Selected, Is.False);

            driver.Check(checkbox);

            var findAgain = driver.FindField("uncheckedBox");
            Assert.That(findAgain.Selected, Is.True);
		}

	    [Test]
	    public void Check_should_leave_a_checked_checkbox_checked()
	    {
            var checkbox = driver.FindField("checkedBox");
            Assert.That(checkbox.Selected, Is.True);

	        driver.Check(checkbox);

            var findAgain = driver.FindField("checkedBox");
	        Assert.That(findAgain.Selected, Is.True);
	    }

	    [Test]
        public void Uncheck_should_uncheck_a_checked_checkbox()
		{
            var checkbox = driver.FindField("checkedBox");
            Assert.That(checkbox.Selected, Is.True);

            driver.Uncheck(checkbox);

            var findAgain = driver.FindField("checkedBox");
            Assert.That(findAgain.Selected, Is.False);
		}

	    [Test]
        public void Uncheck_should_leave_an_unchecked_checkbox_unchecked()
        {
            var checkbox = driver.FindField("uncheckedBox");
            Assert.That(checkbox.Selected, Is.False);

            driver.Uncheck(checkbox);

            var findAgain = driver.FindField("uncheckedBox");
            Assert.That(findAgain.Selected, Is.False);
        }


		[Test]
		public void Check_should_fire_onclick_event()
		{
			var checkbox = driver.FindField("uncheckedBox");
			Assert.That(checkbox.Value, Is.EqualTo("unchecked"));

			driver.Check(checkbox);

			Assert.That(driver.FindField("uncheckedBox").Value, Is.EqualTo("unchecked - clicked"));
		}

		[Test]
		public void Uncheck_should_fire_onclick_event()
		{
			var checkbox = driver.FindField("checkedBox");
			Assert.That(checkbox.Value, Is.EqualTo("checked"));

			driver.Uncheck(checkbox);

			Assert.That(driver.FindField("checkedBox").Value, Is.EqualTo("checked - clicked"));
		}

        [Test]
        public void Choose_should_choose_radio_button_from_list()
        {
            var radioButton1 = driver.FindField("chooseRadio1");
            Assert.That(radioButton1.Selected, Is.False);

            // Choose 1
            driver.Choose(radioButton1);

            var radioButton2 = driver.FindField("chooseRadio2");
            Assert.That(radioButton2.Selected, Is.False);

            // Choose 2
            driver.Choose(radioButton2);

            // New choice is now selected
            radioButton2 = driver.FindField("chooseRadio2");
            Assert.That(radioButton2.Selected, Is.True);

            // Originally selected is no longer selected
            radioButton1 = driver.FindField("chooseRadio1");
            Assert.That(radioButton1.Selected, Is.False);
        }

		[Test]
		public void Choose_should_fire_onclick_event()
		{
			var radio = driver.FindField("chooseRadio2");
			Assert.That(radio.Value, Is.EqualTo("Radio buttons - 2nd value"));

			driver.Choose(radio);

			Assert.That(driver.FindField("chooseRadio2").Value, Is.EqualTo("Radio buttons - 2nd value - clicked"));
		}

		[Test]
		public void HasContent_doesnt_find_missing_text()
		{
			AssertHasNoContent("Some missing text");
		}

		[Test]
		public void HasContent_finds_text_with_parts_marked_up_variously()
		{
			AssertHasContent("Some text with parts marked up variously");
		}

		[Test]
		public void HasContent_finds_text_in_a_table_row()
		{
			AssertHasContent("Some text in a table row");
		}

		[Test]
		public void HasContent_finds_text_in_a_list()
		{
			AssertHasContent("Some text in a list");
		}

		[Test]
		public void HasContent_finds_text_split_over_multiple_lines_in_source()
		{
			AssertHasContent("Some text split over multiple lines in source");
		}

		[Test]
		public void HasContent_finds_text_displayed_over_multiple_lines_in_source()
		{
		    AssertHasContent("Some text displayed over\r\nmultiple lines");
		    AssertHasContent("Some text displayed over\r\ntwo paragraphs");
		}

		[Test]
		public void HasContent_does_not_find_single_line_text_displayed_over_multiple_lines_in_source()
		{
			AssertHasNoContent("Some text displayed over multiple lines");
		}

		private void AssertHasContent(string expectedText)
		{
			Assert.That(driver.HasContent(expectedText), "Expected to find text:\r\n" + expectedText);
		}

		private void AssertHasNoContent(string unexpectedText)
		{
			Assert.That(driver.HasContent(unexpectedText), Is.False, "Expected not to find text:\r\n" + unexpectedText);
		}

		// FindCss / FindXPath are probably impractical to test thoroughly without mocking out the driver in question.
		// Think there's more value in just throwing some examples at it to get some evidence of correct usage.
		[Test]
		public void FindCss_finds_present_examples()
		{
			var shouldFind = "#inspectingContent p.css-test span";
			Assert.That(driver.FindCss(shouldFind).Text, Is.EqualTo("This"));

			shouldFind = "ul#cssTest li:nth-child(3)";
			Assert.That(driver.FindCss(shouldFind).Text, Is.EqualTo("Me! Pick me!"));
		}

		[Test]
		public void HasCss_finds_present_examples()
		{
			var shouldFind = "#inspectingContent p.css-test span";
			Assert.That(driver.HasCss(shouldFind), "Expected to find something at: " + shouldFind);

			shouldFind = "ul#cssTest li:nth-child(3)";
			Assert.That(driver.HasCss(shouldFind), "Expected to find something at: " + shouldFind);
		}

		[Test]
		public void FindCss_does_not_find_missing_examples()
		{
			const string shouldNotFind = "#inspectingContent p.css-missing-test";
			Assert.Throws<MissingHtmlException>(() => driver.FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void HasCss_does_not_find_missing_examples()
		{
			const string shouldNotFind = "#inspectingContent p.css-missing-test";
			Assert.That(driver.HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void FindCss_only_finds_visible_elements()
		{
			const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
			Assert.Throws<MissingHtmlException>(() => driver.FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void HasCss_only_finds_visible_elements()
		{
			const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
			Assert.That(driver.HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void FindXPath_finds_present_examples()
		{
			var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
			Assert.That(driver.FindXPath(shouldFind).Text, Is.EqualTo("This"));

			shouldFind = "//ul[@id='cssTest']/li[3]";
			Assert.That(driver.FindXPath(shouldFind).Text, Is.EqualTo("Me! Pick me!"));
		}

		[Test]
		public void HasXPath_finds_present_examples()
		{
			var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
			Assert.That(driver.HasXPath(shouldFind), "Expected to find something at: " + shouldFind);

			shouldFind = "//ul[@id='cssTest']/li[3]";
			Assert.That(driver.HasXPath(shouldFind), "Expected to find something at: " + shouldFind);
		}

		[Test]
		public void FindXPath_does_not_find_missing_examples()
		{
			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
			Assert.Throws<MissingHtmlException>(() => driver.FindXPath(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void HasXPath_does_not_find_missing_examples()
		{
			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
			Assert.That(driver.HasXPath(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void FindXPath_only_finds_visible_elements()
		{
			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
			Assert.Throws<MissingHtmlException>(() => driver.FindXPath(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void HasXPath_only_finds_visible_elements()
		{
			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
			Assert.That(driver.HasXPath(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
		}

		[Test]
		public void HasDialog_finds_exact_text_in_alert()
		{
			driver.Click(driver.FindLink("Trigger an alert"));
			Assert.That(driver.HasDialog("You have triggered an alert and this is the text."));
			Assert.That(driver.HasDialog("You have triggered AN alert and this is the text."), Is.False);
			NewSession();
		}

		[Test]
		public void HasDialog_finds_exact_text_in_confirm()
		{
			driver.Click(driver.FindLink("Trigger a confirm"));
			Assert.That(driver.HasDialog("You have triggered a confirm and this is the text."));
			Assert.That(driver.HasDialog("You have triggered a different confirm and this is the different text."), Is.False);
			NewSession();
		}

		
	}
}