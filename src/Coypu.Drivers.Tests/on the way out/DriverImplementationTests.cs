//using System.IO;
//using System.Linq;
//using NSpec;
//using NUnit.Framework;
//
//namespace Coypu.Drivers.Tests
//{
//	public abstract class DriverImplementationTests : nspec
//	{
//		private const string INTERACTION_TESTS_PAGE = @"..\..\html\InteractionTestsPage.htm";
//		private Driver driver;
//
//		[TestFixtureSetUp]
//		public void FixtureSetUp()
//		{
//
//		}
//
//		public void NewSession()
//		{
//			driver.Dispose();
//			driver = GetDriver();
//		}
//
//		[SetUp]
//		public void SetUp()
//		{
//			driver.Visit(new FileInfo(INTERACTION_TESTS_PAGE).FullName);
//		}
//
//		[TestFixtureTearDown]
//		public void Dispose()
//		{
//			driver.Dispose();
//		}
//
//		protected abstract Driver GetDriver();
//
//
//
//		[Test]
//		public void FindField_should_find_field_by_label_text_by_for_attribute()
//		{
//			driver.FindField("text input field linked by for").Id.should_be("forLabeledTextInputFieldId");
//			driver.FindField("password field linked by for").Id.should_be("forLabeledPasswordFieldId");
//			driver.FindField("select field linked by for").Id.should_be("forLabeledSelectFieldId");
//			driver.FindField("checkbox field linked by for").Id.should_be("forLabeledCheckboxFieldId");
//			driver.FindField("radio field linked by for").Id.should_be("forLabeledRadioFieldId");
//			driver.FindField("textarea field linked by for").Id.should_be("forLabeledTextareaFieldId");
//		}
//
//		[Test]
//		public void FindField_should_find_field_by_container_label()
//		{
//			driver.FindField("text input field in a label container").Id.should_be("containerLabeledTextInputFieldId");
//			driver.FindField("password field in a label container").Id.should_be("containerLabeledPasswordFieldId");
//			driver.FindField("checkbox field in a label container").Id.should_be("containerLabeledCheckboxFieldId");
//			driver.FindField("radio field in a label container").Id.should_be("containerLabeledRadioFieldId");
//			driver.FindField("select field in a label container").Id.should_be("containerLabeledSelectFieldId");
//			driver.FindField("textarea field in a label container").Id.should_be("containerLabeledTextareaFieldId");
//		}
//
//		[Test]
//		public void FindField_should_find_text_field_by_placeholder()
//		{
//			driver.FindField("text input field with a placeholder").Id.should_be("textInputFieldWithPlaceholder");
//			driver.FindField("textarea field with a placeholder").Id.should_be("textareaFieldWithPlaceholder");
//		}
//
//		[Test]
//		public void FindField_should_find_field_by_id()
//		{
//			driver.FindField("containerLabeledTextInputFieldId").Value.should_be("text input field two val");
//			driver.FindField("containerLabeledTextareaFieldId").Value.should_be("textarea field two val");
//			driver.FindField("containerLabeledSelectFieldId").Name.should_be("containerLabeledSelectFieldName");
//			driver.FindField("containerLabeledCheckboxFieldId").Value.should_be("checkbox field two val");
//			driver.FindField("containerLabeledRadioFieldId").Value.should_be("radio field two val");
//			driver.FindField("containerLabeledPasswordFieldId").Name.should_be("containerLabeledPasswordFieldName");
//		}
//
//		[Test]
//		public void FindField_should_find_field_by_name()
//		{
//			driver.FindField("containerLabeledTextInputFieldName").Value.should_be("text input field two val");
//			driver.FindField("containerLabeledTextareaFieldName").Value.should_be("textarea field two val");
//			driver.FindField("containerLabeledSelectFieldName").Id.should_be("containerLabeledSelectFieldId");
//			driver.FindField("containerLabeledCheckboxFieldName").Value.should_be("checkbox field two val");
//			driver.FindField("containerLabeledRadioFieldName").Value.should_be("radio field two val");
//			driver.FindField("containerLabeledPasswordFieldName").Id.should_be("containerLabeledPasswordFieldId");
//		}
//
//		[Test]
//		public void FindField_should_find_radio_button_by_value()
//		{
//			driver.FindField("radio field one val").Name.should_be("forLabeledRadioFieldName");
//			driver.FindField("radio field two val").Name.should_be("containerLabeledRadioFieldName");
//		}
//
//		[Test]
//		public void Set_should_set_value_of_text_input_field()
//		{
//			var textField = driver.FindField("containerLabeledTextInputFieldName");
//			driver.Set(textField, "New text input value");
//
//			textField.Value.should_be("New text input value");
//
//			var findAgain = driver.FindField("containerLabeledTextInputFieldName");
//			findAgain.Value.should_be("New text input value");
//		}
//
//		[Test]
//		public void Set_should_set_value_of_textarea_field()
//		{
//			var textField = driver.FindField("containerLabeledTextareaFieldName");
//			driver.Set(textField, "New textarea value");
//
//			textField.Value.should_be("New textarea value");
//
//			var findAgain = driver.FindField("containerLabeledTextareaFieldName");
//			findAgain.Value.should_be("New textarea value");
//		}
//
//		[Test]
//		public void Set_should_select_option_by_text_or_value()
//		{
//			var textField = driver.FindField("containerLabeledSelectFieldId");
//			textField.Value.should_be("select2value1");
//
//			driver.Select(textField, "select two option two");
//
//			var findAgain = driver.FindField("containerLabeledSelectFieldId");
//			findAgain.Value.should_be("select2value2");
//
//			driver.Select(textField, "select2value1");
//
//			var andAgain = driver.FindField("containerLabeledSelectFieldId");
//			andAgain.Value.should_be("select2value1");
//		}
//
//		[Test]
//		public void Set_should_fire_change_event_when_selecting_an_option()
//		{
//			var textField = driver.FindField("containerLabeledSelectFieldId");
//			textField.Name.should_be("containerLabeledSelectFieldName");
//
//			driver.Select(textField, "select two option two");
//
//			driver.FindField("containerLabeledSelectFieldId").Name.should_be("containerLabeledSelectFieldName - changed");
//		}
//
//		[Test]
//		public void Selected_is_text_of_selected_option()
//		{
//			var textField = driver.FindField("containerLabeledSelectFieldId");
//			textField.SelectedOption.should_be("select two option one");
//
//			driver.Select(textField, "select2value2");
//
//			textField = driver.FindField("containerLabeledSelectFieldId");
//			textField.SelectedOption.should_be("select two option two");
//		}
//
//		[Test]
//		public void Check_should_check_an_unchecked_checkbox()
//		{
//			var checkbox = driver.FindField("uncheckedBox");
//			checkbox.Selected.should_be_false();
//
//			driver.Check(checkbox);
//
//			var findAgain = driver.FindField("uncheckedBox");
//			findAgain.Selected.should_be_true();
//		}
//
//		[Test]
//		public void Check_should_leave_a_checked_checkbox_checked()
//		{
//			var checkbox = driver.FindField("checkedBox");
//			checkbox.Selected.should_be_true();
//
//			driver.Check(checkbox);
//
//			var findAgain = driver.FindField("checkedBox");
//			findAgain.Selected.should_be_true();
//		}
//
//		[Test]
//		public void Uncheck_should_uncheck_a_checked_checkbox()
//		{
//			var checkbox = driver.FindField("checkedBox");
//			checkbox.Selected.should_be_true();
//
//			driver.Uncheck(checkbox);
//
//			var findAgain = driver.FindField("checkedBox");
//			findAgain.Selected.should_be_false();
//		}
//
//		[Test]
//		public void Uncheck_should_leave_an_unchecked_checkbox_unchecked()
//		{
//			var checkbox = driver.FindField("uncheckedBox");
//			checkbox.Selected.should_be_false();
//
//			driver.Uncheck(checkbox);
//
//			var findAgain = driver.FindField("uncheckedBox");
//			findAgain.Selected.should_be_false();
//		}
//
//
//		[Test]
//		public void Check_should_fire_onclick_event()
//		{
//			var checkbox = driver.FindField("uncheckedBox");
//			checkbox.Value.should_be("unchecked");
//
//			driver.Check(checkbox);
//
//			driver.FindField("uncheckedBox").Value.should_be("unchecked - clicked");
//		}
//
//		[Test]
//		public void Uncheck_should_fire_onclick_event()
//		{
//			var checkbox = driver.FindField("checkedBox");
//			checkbox.Value.should_be("checked");
//
//			driver.Uncheck(checkbox);
//
//			driver.FindField("checkedBox").Value.should_be("checked - clicked");
//		}
//
//		[Test]
//		public void Choose_should_choose_radio_button_from_list()
//		{
//			var radioButton1 = driver.FindField("chooseRadio1");
//			radioButton1.Selected.should_be_false();
//
			//Choose 1
//			driver.Choose(radioButton1);
//
//			var radioButton2 = driver.FindField("chooseRadio2");
//			radioButton2.Selected.should_be_false();
//
			//Choose 2
//			driver.Choose(radioButton2);
//
			//New choice is now selected
//			radioButton2 = driver.FindField("chooseRadio2");
//			radioButton2.Selected.should_be_true();
//
			//Originally selected is no longer selected
//			radioButton1 = driver.FindField("chooseRadio1");
//			radioButton1.Selected.should_be_false();
//		}
//
//		[Test]
//		public void Choose_should_fire_onclick_event()
//		{
//			var radio = driver.FindField("chooseRadio2");
//			radio.Value.should_be("Radio buttons - 2nd value");
//
//			driver.Choose(radio);
//
//			driver.FindField("chooseRadio2").Value.should_be("Radio buttons - 2nd value - clicked");
//		}
//
//		[Test]
//		public void HasContent_doesnt_find_missing_text()
//		{
//			driver.HasContent("Some missing text").should_be_false();
//		}
//
//		[Test]
//		public void HasContent_finds_text_with_parts_marked_up_variously()
//		{
//			driver.HasContent("Some text with parts marked up variously").should_be_true();
//		}
//
//		[Test]
//		public void HasContent_finds_text_in_a_table_row()
//		{
//			driver.HasContent("Some text in a table row").should_be_true();
//		}
//
//		[Test]
//		public void HasContent_finds_text_in_a_list()
//		{
//			driver.HasContent("Some text in a list").should_be_true();
//		}
//
//		[Test]
//		public void HasContent_finds_text_split_over_multiple_lines_in_source()
//		{
//			driver.HasContent("Some text split over multiple lines in source").should_be_true();
//		}
//
//		[Test]
//		public void HasContent_finds_text_displayed_over_multiple_lines_in_source()
//		{
//			driver.HasContent("Some text displayed over\r\nmultiple lines").should_be_true();
//			driver.HasContent("Some text displayed over\r\ntwo paragraphs").should_be_true();
//		}
//
//		[Test]
//		public void HasContent_does_not_find_single_line_text_displayed_over_multiple_lines_in_source()
//		{
//			driver.HasContent("Some text displayed over multiple lines").should_be_false();
//		}
//
		// FindCss / FindXPath are probably impractical to test thoroughly without mocking out the driver in question.
		// Think there's more value in just throwing some examples at it to get some evidence of correct usage.
//		[Test]
//		public void FindCss_finds_present_examples()
//		{
//			var shouldFind = "#inspectingContent p.css-test span";
//			driver.FindCss(shouldFind).Text.should_be("This");
//
//			shouldFind = "ul#cssTest li:nth-child(3)";
//			driver.FindCss(shouldFind).Text.should_be("Me! Pick me!");
//		}
//
//		[Test]
//		public void HasCss_finds_present_examples()
//		{
//			var shouldFind = "#inspectingContent p.css-test span";
//			Assert.That(driver.HasCss(shouldFind), "Expected to find something at: " + shouldFind);
//
//			shouldFind = "ul#cssTest li:nth-child(3)";
//			Assert.That(driver.HasCss(shouldFind), "Expected to find something at: " + shouldFind);
//		}
//
//		[Test]
//		public void FindCss_does_not_find_missing_examples()
//		{
//			const string shouldNotFind = "#inspectingContent p.css-missing-test";
//			Assert.Throws<MissingHtmlException>(() => driver.FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void HasCss_does_not_find_missing_examples()
//		{
//			const string shouldNotFind = "#inspectingContent p.css-missing-test";
//			Assert.That(driver.HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void FindCss_only_finds_visible_elements()
//		{
//			const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
//			Assert.Throws<MissingHtmlException>(() => driver.FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void HasCss_only_finds_visible_elements()
//		{
//			const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
//			Assert.That(driver.HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
//		}
//
//
//		[Test]
//		public void FindAllCss_returns_empty_if_no_matches()
//		{
//			const string shouldNotFind = "#inspectingContent p.css-missing-test";
//			Assert.That(driver.FindAllCss(shouldNotFind), Is.Empty);
//		}
//
//		[Test]
//		public void FindCss_returns_all_matches_by_css()
//		{
//			const string shouldNotFind = "#inspectingContent ul#cssTest li";
//			var all = driver.FindAllCss(shouldNotFind);
//			all.Count().should_be(3);
//			all.ElementAt(1).Text.should_be("two");
//			all.ElementAt(2).Text.should_be("Me! Pick me!");
//		}
//
//		[Test]
//		public void FindXPath_returns_empty_if_no_matches()
//		{
//			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
//			driver.FindAllXPath(shouldNotFind).should_be_empty();
//		}
//
//		[Test]
//		public void FindXPath_returns_all_matches_by_xpath()
//		{
//			const string shouldNotFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
//			var all = driver.FindAllXPath(shouldNotFind);
//			all.Count().should_be(3);
//			all.ElementAt(1).Text.should_be("two");
//			all.ElementAt(2).Text.should_be("Me! Pick me!");
//		}
//
//		[Test]
//		public void FindXPath_finds_present_examples()
//		{
//			var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
//			driver.FindXPath(shouldFind).Text.should_be("This");
//
//			shouldFind = "//ul[@id='cssTest']/li[3]";
//			driver.FindXPath(shouldFind).Text.should_be("Me! Pick me!");
//		}
//
//		[Test]
//		public void HasXPath_finds_present_examples()
//		{
//			var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
//			Assert.That(driver.HasXPath(shouldFind), "Expected to find something at: " + shouldFind);
//
//			shouldFind = "//ul[@id='cssTest']/li[3]";
//			Assert.That(driver.HasXPath(shouldFind), "Expected to find something at: " + shouldFind);
//		}
//
//		[Test]
//		public void FindXPath_does_not_find_missing_examples()
//		{
//			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
//			Assert.Throws<MissingHtmlException>(() => driver.FindXPath(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void HasXPath_does_not_find_missing_examples()
//		{
//			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
//			Assert.That(driver.HasXPath(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void FindXPath_only_finds_visible_elements()
//		{
//			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
//			Assert.Throws<MissingHtmlException>(() => driver.FindXPath(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void HasXPath_only_finds_visible_elements()
//		{
//			const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
//			Assert.That(driver.HasXPath(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
//		}
//
//		[Test]
//		public void HasDialog_finds_exact_text_in_alert()
//		{
//			driver.Click(driver.FindLink("Trigger an alert"));
//			driver.HasDialog("You have triggered an alert and this is the text.");
//			driver.HasDialog("You have triggered AN alert and this is the text.").should_be_false();
//			NewSession();
//		}
//
//		[Test]
//		public void HasDialog_finds_exact_text_in_confirm()
//		{
//			driver.Click(driver.FindLink("Trigger a confirm"));
//			driver.HasDialog("You have triggered a confirm and this is the text.");
//			driver.HasDialog("You have triggered a different confirm and this is the different text.").should_be_false();
//			NewSession();
//		}
//
//	}
//}
