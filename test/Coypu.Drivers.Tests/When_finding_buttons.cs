using NSpec;
using Xunit;
namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons : DriverSpecs
    {
        [Fact]
        public void Finds_a_particular_button_by_its_text()
        {
            Button("first button").Id.should_be("firstButtonId");
            Button("second button").Id.should_be("secondButtonId");
        }

        [Fact]
        public void Finds_a_particular_button_by_its_id()
        {
            Button("firstButtonId").Text.should_be("first button");
            Button("thirdButtonId").Text.should_be("third button");
        }

        [Fact]
        public void Finds_a_particular_button_by_its_name()
        {
            Button("secondButtonName").Text.should_be("second button");
            Button("thirdButtonName").Text.should_be("third button");
        }

        [Fact]
        public void Finds_a_particular_input_button_by_its_value()
        {
            Button("first input button").Id.should_be("firstInputButtonId");
            Button("second input button").Id.should_be("secondInputButtonId");
        }

        [Fact]
        public void Finds_a_particular_input_button_by_its_id()
        {
            Button("firstInputButtonId").Value.should_be("first input button");
            Button("thirdInputButtonId").Value.should_be("third input button");
        }

        [Fact]
        public void Finds_a_particular_input_button_by_its_name()
        {
            Button("secondInputButtonId").Value.should_be("second input button");
            Button("thirdInputButtonName").Value.should_be("third input button");
        }

        [Fact]
        public void Finds_a_particular_submit_button_by_its_value()
        {
            Button("first submit button").Id.should_be("firstSubmitButtonId");
            Button("second submit button").Id.should_be("secondSubmitButtonId");
        }

        [Fact]
        public void Finds_a_particular_submit_button_by_its_id()
        {
            Button("firstSubmitButtonId").Value.should_be("first submit button");
            Button("thirdSubmitButtonId").Value.should_be("third submit button");
        }

        [Fact]
        public void Finds_a_particular_submit_button_by_its_name()
        {
            Button("secondSubmitButtonName").Value.should_be("second submit button");
            Button("thirdSubmitButtonName").Value.should_be("third submit button");
        }

        [Fact]
        public void Finds_image_buttons()
        {
            Button("firstImageButtonId").Value.should_be("first image button");
            Button("secondImageButtonId").Value.should_be("second image button");
        }

        [Fact]
        public void Does_not_find_text_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstTextInputId"));
        }

        [Fact]
        public void Does_not_find_email_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstEmailInputId"));
        }

        [Fact]
        public void Does_not_find_tel_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstTelInputId"));
        }

        [Fact]
        public void Does_not_find_url_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstUrlInputId"));
        }

        [Fact]
        public void Does_not_find_hidden_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstHiddenInputId"));
        }

        [Fact]
        public void Does_not_find_invisible_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstInvisibleInputId"));
        }

        [Fact]
        public void Finds_img_elements_with_role_button_by_alt_text()
        {
            Assert.That(Button("I'm an image with the role of button").Id, Is.EqualTo("roleImageButtonId"));
        }

        [Fact]
        public void Finds_any_elements_with_role_button_by_text()
        {
            Assert.That(Button("I'm a span with the role of button").Id, Is.EqualTo("roleSpanButtonId"));
        }

        [Fact]
        public void Finds_any_elements_with_class_button_by_text()
        {
            Assert.That(Button("I'm a span with the class of button").Id, Is.EqualTo("classButtonSpanButtonId"));
        }

        [Fact]
        public void Finds_any_elements_with_class_btn_by_text()
        {
            Assert.That(Button("I'm a span with the class of btn").Id, Is.EqualTo("classBtnSpanButtonId"));
        }

        [Fact]
        public void Finds_an_image_button_with_both_types_of_quote_in_my_value()
        {
            var button = Button("I'm an image button with \"both\" types of quote in my value");
            Assert.That(button.Id, Is.EqualTo("buttonWithBothQuotesId"));
        }
    }
}