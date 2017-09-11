using Shouldly;
using NUnit.Framework;
namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons : DriverSpecs
    {
        [Test]
        public void Finds_a_particular_button_by_its_text()
        {
            Button("first button").Id.ShouldBe("firstButtonId");
            Button("second button").Id.ShouldBe("secondButtonId");
        }

        [Test]
        public void Finds_a_particular_button_by_its_id()
        {
            Button("firstButtonId").Text.ShouldBe("first button");
            Button("thirdButtonId").Text.ShouldBe("third button");
        }

        [Test]
        public void Finds_a_particular_button_by_its_name()
        {
            Button("secondButtonName").Text.ShouldBe("second button");
            Button("thirdButtonName").Text.ShouldBe("third button");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_value()
        {
            Button("first input button").Id.ShouldBe("firstInputButtonId");
            Button("second input button").Id.ShouldBe("secondInputButtonId");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_id()
        {
            Button("firstInputButtonId").Value.ShouldBe("first input button");
            Button("thirdInputButtonId").Value.ShouldBe("third input button");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_name()
        {
            Button("secondInputButtonId").Value.ShouldBe("second input button");
            Button("thirdInputButtonName").Value.ShouldBe("third input button");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_value()
        {
            Button("first submit button").Id.ShouldBe("firstSubmitButtonId");
            Button("second submit button").Id.ShouldBe("secondSubmitButtonId");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_id()
        {
            Button("firstSubmitButtonId").Value.ShouldBe("first submit button");
            Button("thirdSubmitButtonId").Value.ShouldBe("third submit button");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_name()
        {
            Button("secondSubmitButtonName").Value.ShouldBe("second submit button");
            Button("thirdSubmitButtonName").Value.ShouldBe("third submit button");
        }

        [Test]
        public void Finds_image_buttons()
        {
            Button("firstImageButtonId").Value.ShouldBe("first image button");
            Button("secondImageButtonId").Value.ShouldBe("second image button");
        }

        [Test]
        public void Does_not_find_text_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstTextInputId"));
        }

        [Test]
        public void Does_not_find_email_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstEmailInputId"));
        }

        [Test]
        public void Does_not_find_tel_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstTelInputId"));
        }

        [Test]
        public void Does_not_find_url_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstUrlInputId"));
        }

        [Test]
        public void Does_not_find_hidden_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstHiddenInputId"));
        }

        [Test]
        public void Does_not_find_invisible_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => Button("firstInvisibleInputId"));
        }

        [Test]
        public void Finds_img_elements_with_role_button_by_alt_text()
        {
            Assert.That(Button("I'm an image with the role of button").Id, Is.EqualTo("roleImageButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_role_button_by_text()
        {
            Assert.That(Button("I'm a span with the role of button").Id, Is.EqualTo("roleSpanButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_class_button_by_text()
        {
            Assert.That(Button("I'm a span with the class of button").Id, Is.EqualTo("classButtonSpanButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_class_btn_by_text()
        {
            Assert.That(Button("I'm a span with the class of btn").Id, Is.EqualTo("classBtnSpanButtonId"));
        }

        [Test]
        public void Finds_an_image_button_with_both_types_of_quote_in_my_value()
        {
            var button = Button("I'm an image button with \"both\" types of quote in my value");
            Assert.That(button.Id, Is.EqualTo("buttonWithBothQuotesId"));
        }
    }
}