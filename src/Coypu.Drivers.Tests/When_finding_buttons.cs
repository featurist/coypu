using Coypu.Finders;
using NSpec;
using NUnit.Framework;
namespace Coypu.Drivers.Tests
{
    // TODO: These now all call through Options.Find(query) to Driver.FindXPath
    // TODO: Move them to text XPath.Button directly against an XML DOM
    // TODO: Unit tests for ClickButton should then test interactino with Options.Find not the Driver
    // TODO: Options.Find will then be tested against the Driver for correct Match Strategy implementation
    // TODO: But all finders through Options.Find(query) except for Windows/Frames using XPath or css where appropriate
    public class When_finding_buttons : DriverSpecs
    {
        [Test]
        public void Finds_a_particular_button_by_its_text()
        {
            new ButtonFinder(Driver, "first button", Root).Find().Id.should_be("firstButtonId");
            new ButtonFinder(Driver, "second button", Root).Find().Id.should_be("secondButtonId");
        }

        [Test]
        public void Finds_a_particular_button_by_its_id()
        {
            new ButtonFinder(Driver,"firstButtonId", Root).Find().Text.should_be("first button");
            new ButtonFinder(Driver,"thirdButtonId", Root).Find().Text.should_be("third button");
        }

        [Test]
        public void Finds_a_particular_button_by_its_name()
        {
            new ButtonFinder(Driver,"secondButtonName", Root).Find().Text.should_be("second button");
            new ButtonFinder(Driver,"thirdButtonName", Root).Find().Text.should_be("third button");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_value()
        {
            new ButtonFinder(Driver,"first input button", Root).Find().Id.should_be("firstInputButtonId");
            new ButtonFinder(Driver,"second input button", Root).Find().Id.should_be("secondInputButtonId");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_id()
        {
            new ButtonFinder(Driver,"firstInputButtonId", Root).Find().Value.should_be("first input button");
            new ButtonFinder(Driver,"thirdInputButtonId", Root).Find().Value.should_be("third input button");
        }

        [Test]
        public void Finds_a_particular_input_button_by_its_name()
        {
            new ButtonFinder(Driver,"secondInputButtonId", Root).Find().Value.should_be("second input button");
            new ButtonFinder(Driver,"thirdInputButtonName", Root).Find().Value.should_be("third input button");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_value()
        {
            new ButtonFinder(Driver,"first submit button", Root).Find().Id.should_be("firstSubmitButtonId");
            new ButtonFinder(Driver,"second submit button", Root).Find().Id.should_be("secondSubmitButtonId");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_id()
        {
            new ButtonFinder(Driver,"firstSubmitButtonId", Root).Find().Value.should_be("first submit button");
            new ButtonFinder(Driver,"thirdSubmitButtonId", Root).Find().Value.should_be("third submit button");
        }

        [Test]
        public void Finds_a_particular_submit_button_by_its_name()
        {
            new ButtonFinder(Driver,"secondSubmitButtonName", Root).Find().Value.should_be("second submit button");
            new ButtonFinder(Driver,"thirdSubmitButtonName", Root).Find().Value.should_be("third submit button");
        }

        [Test]
        public void Finds_image_buttons()
        {
            new ButtonFinder(Driver,"firstImageButtonId", Root).Find().Value.should_be("first image button");
            new ButtonFinder(Driver,"secondImageButtonId", Root).Find().Value.should_be("second image button");
        }

        [Test]
        public void Does_not_find_text_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver, "firstTextInputId", Root).Find());
        }

        [Test]
        public void Does_not_find_email_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver, "firstEmailInputId", Root).Find());
        }

        [Test]
        public void Does_not_find_tel_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver, "firstTelInputId", Root).Find());
        }

        [Test]
        public void Does_not_find_url_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver,"firstUrlInputId", Root).Find());
        }

        [Test]
        public void Does_not_find_hidden_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver, "firstHiddenInputId", Root).Find());
        }

        [Test]
        public void Does_not_find_invisible_inputs()
        {
            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver, "firstInvisibleInputId", Root).Find());
        }

        [Test]
        public void Finds_img_elements_with_role_button_by_alt_text()
        {
            Assert.That(new ButtonFinder(Driver,"I'm an image with the role of button", Root).Find().Id, Is.EqualTo("roleImageButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_role_button_by_text()
        {
            Assert.That(new ButtonFinder(Driver,"I'm a span with the role of button", Root).Find().Id, Is.EqualTo("roleSpanButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_class_button_by_text()
        {
            Assert.That(new ButtonFinder(Driver,"I'm a span with the class of button", Root).Find().Id, Is.EqualTo("classButtonSpanButtonId"));
        }

        [Test]
        public void Finds_any_elements_with_class_btn_by_text()
        {
            Assert.That(new ButtonFinder(Driver,"I'm a span with the class of btn", Root).Find().Id, Is.EqualTo("classBtnSpanButtonId"));
        }

        [Test]
        public void Finds_an_image_button_with_both_types_of_quote_in_my_value()
        {
            var button = new ButtonFinder(Driver,"I'm an image button with \"both\" types of quote in my value", Root).Find();
            Assert.That(button.Id, Is.EqualTo("buttonWithBothQuotesId"));
        }
    }
}