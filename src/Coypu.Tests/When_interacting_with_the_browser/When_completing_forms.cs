using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_completing_forms : BrowserInteractionTests 
    {
        [Test]
        public void When_filling_in_a_text_field_It_finds_field_and_sets_value_robustly()
        {
            var element = new StubElement();
            driver.StubField("Some field locator", element, browserSession);

            browserSession.FillIn("Some field locator").With("some value for the field");

            Assert.That(driver.SetFields.Keys, Has.No.Member(element));

            RunQueryAndCheckTiming();

            Assert.That(driver.SetFields.Keys, Has.Member(element));
            Assert.That(driver.SetFields[element].Value, Is.EqualTo("some value for the field"));
        }

        [Test]
        public void When_filling_in_a_field_It_clicks_to_ensure_focus_event()
        {
            var element = new StubElement();
            driver.StubField("Some field locator", element, browserSession);

            browserSession.FillIn("Some field locator").With("some value for the field");

            Assert.That(driver.ClickedElements, Is.Empty);

            RunQueryAndCheckTiming();

            Assert.That(driver.ClickedElements, Has.Member(element));
        }

        [Test]
        public void When_filling_in_file_field_It_doesnt_click() {
            var element = new StubElement();
            element.StubAttribute("type", "file");
            driver.StubField("Some field locator", element, browserSession);

            browserSession.FillIn("Some field locator").With("some value for the field");

            RunQueryAndCheckTiming();

            Assert.That(driver.ClickedElements, Has.No.Member(element));
        }

        [Test]
        public void When_selecting_an_option_It_finds_field_and_selects_option_robustly()
        {
            var element = new StubElement();
            driver.StubField("Some select field locator", element, browserSession);

            browserSession.Select("some option to select").From("Some select field locator");

            Assert.That(driver.SelectedOptions, Has.No.Member(element));

            RunQueryAndCheckTiming();

            Assert.That(driver.SelectedOptions.Keys, Has.Member(element));
            Assert.That(driver.SelectedOptions[element], Is.EqualTo("some option to select"));
        }

        [Test]
        public void When_checking_a_checkbox_It_find_fields_and_checks_robustly()
        {
            var element = new StubElement();
            driver.StubField("Some checkbox locator", element, browserSession);

            browserSession.Check("Some checkbox locator");

            Assert.That(driver.CheckedElements, Has.No.Member(element));

            RunQueryAndCheckTiming();

            Assert.That(driver.CheckedElements, Has.Member(element));
        }

        [Test]
        public void When_unchecking_a_checkbox_It_finds_field_and_unchecks_robustly()
        {
            var element = new StubElement();
            driver.StubField("Some checkbox locator", element, browserSession);

            browserSession.Uncheck("Some checkbox locator");

            Assert.That(driver.UncheckedElements, Has.No.Member(element));

            RunQueryAndCheckTiming();

            Assert.That(driver.UncheckedElements, Has.Member(element));
        }

        [Test]
        public void When_choosing_a_radio_button_It_finds_field_and_chooses_robustly()
        {
            var element = new StubElement();
            driver.StubField("Some radio locator", element, browserSession);

            browserSession.Choose("Some radio locator");

            Assert.That(driver.ChosenElements, Has.No.Member(element));

            RunQueryAndCheckTiming();

            Assert.That(driver.ChosenElements, Has.Member(element));
        }

        [Test]
        public void It_makes_robust_call_to_find_then_sends_keys_to_element_via_underlying_driver()
        {
            var element = new StubElement();
            driver.StubCss("something.to click", element, browserSession);
            spyRobustWrapper.AlwaysReturnFromRobustly(element);

            var elementScope = browserSession.FindCss("something.to click");

            Assert.That(driver.FindCssRequests, Is.Empty, "Finder not called robustly");

            elementScope.SendKeys("some keys to press");

            RunQueryAndCheckTiming();

            Assert.That(driver.FindCssRequests.Any(), Is.False, "Scope finder was not deferred");

            Assert.That(driver.SentKeys.Count, Is.EqualTo(1));
            Assert.That(driver.SentKeys[element], Is.EqualTo("some keys to press"));
        }
    }
}