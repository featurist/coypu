using System;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons : DriverSpecs
    {
        internal override void Specs()
        {
            it["finds a particular button by its text"] = () =>
            {
                driver.FindButton("first button", Root).Id.should_be("firstButtonId");
                driver.FindButton("second button", Root).Id.should_be("secondButtonId");
            };

            it["finds a particular button by its id"] = () =>
            {
                driver.FindButton("firstButtonId", Root).Text.should_be("first button");
                driver.FindButton("thirdButtonId", Root).Text.should_be("third button");
            };

            it["finds a particular button by its name"] = () =>
            {
                driver.FindButton("secondButtonName", Root).Text.should_be("second button");
                driver.FindButton("thirdButtonName", Root).Text.should_be("third button");
            };

            it["finds a particular input button by its value"] = () =>
            {
                driver.FindButton("first input button", Root).Id.should_be("firstInputButtonId");
                driver.FindButton("second input button", Root).Id.should_be("secondInputButtonId");
            };

            it["finds a particular input button by its id"] = () =>
            {
                driver.FindButton("firstInputButtonId", Root).Value.should_be("first input button");
                driver.FindButton("thirdInputButtonId", Root).Value.should_be("third input button");
            };
            
            it["finds a particular input button by id ends with"] = () =>
            {
                driver.FindButton("rdInputButtonId", Root).Value.should_be("third input button");
            };

            it["finds a particular input button by its name"] = () =>
            {
                driver.FindButton("secondInputButtonId", Root).Value.should_be("second input button");
                driver.FindButton("thirdInputButtonName", Root).Value.should_be("third input button");
            };

            it["finds a particular submit button by its value"] = () =>
            {
                driver.FindButton("first submit button", Root).Id.should_be("firstSubmitButtonId");
                driver.FindButton("second submit button", Root).Id.should_be("secondSubmitButtonId");
            };

            it["finds a particular submit button by its id"] = () =>
            {
                driver.FindButton("firstSubmitButtonId", Root).Value.should_be("first submit button");
                driver.FindButton("thirdSubmitButtonId", Root).Value.should_be("third submit button");
            };

            it["finds a particular submit button by its name"] = () =>
            {
                driver.FindButton("secondSubmitButtonName", Root).Value.should_be("second submit button");
                driver.FindButton("thirdSubmitButtonName", Root).Value.should_be("third submit button");
            };

            it["finds image buttons"] = () =>
            {
                driver.FindButton("firstImageButtonId", Root).Value.should_be("first image button");
                driver.FindButton("secondImageButtonId", Root).Value.should_be("second image button");
            };

            it["does not find text inputs"] = () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindButton("firstTextInputId", Root));
            };

            it["does not find hidden inputs"] = () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindButton("firstHiddenInputId", Root));
            };

            it["does not find invisible inputs"] = () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindButton("firstInvisibleInputId", Root));
            };

            it["finds img elements with role='button' by alt text"] = () =>
            {
                Assert.That(driver.FindButton("I'm an image with the role of button", Root).Id, Is.EqualTo("roleImageButtonId"));
            };

            it["finds any elements with role='button' by text"] = () =>
            {
                Assert.That(driver.FindButton("I'm a span with the role of button", Root).Id, Is.EqualTo("roleSpanButtonId"));
            };

            it["finds an image button with both types of quote in my value"] = () =>
            {
                var button = driver.FindButton("I'm an image button with \"both\" types of quote in my value", Root);
                Assert.That(button.Id, Is.EqualTo("buttonWithBothQuotesId"));
            };
        }
    }
}