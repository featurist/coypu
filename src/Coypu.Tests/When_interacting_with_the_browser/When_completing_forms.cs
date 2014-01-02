using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Drivers;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_completing_forms : BrowserInteractionTests 
    {
        private StubElement StubField(string locator, Options options = null)
        {
            var stubField = new StubElement{Id = Guid.NewGuid().ToString()};
            var fieldXpath = new Html(sessionConfiguration.Browser.UppercaseTagNames).Field(locator, options ?? sessionConfiguration);
            driver.StubAllXPath(fieldXpath, new []{stubField}, browserSession, options ?? sessionConfiguration);

            return stubField;
        }

        private StubElement StubOption(string fieldLocator, string optionLocator, Options options = null)
        {
            var stubField = new StubElement { Id = Guid.NewGuid().ToString() };
            var fieldXpath = new Html(sessionConfiguration.Browser.UppercaseTagNames).SelectOption(fieldLocator, optionLocator, options ?? sessionConfiguration);
            driver.StubAllXPath(fieldXpath, new[] { stubField }, browserSession, options ?? sessionConfiguration);

            return stubField;
        }
        
        [Test]
        public void When_filling_in_a_text_field_It_finds_field_and_sets_value_robustly()
        {
            var element = StubField("Some field locator");

            browserSession.FillIn("Some field locator").With("some value for the field");

            Assert.That(driver.SetFields.Keys, Has.No.Member(element));

            RunQueryAndCheckTiming();

            var setField = driver.SetFields.Keys.Cast<SynchronisedElementScope>().Single().Now();
            Assert.That(setField, Is.SameAs(element));
            Assert.That(driver.SetFields.Values.Single().Value, Is.EqualTo("some value for the field"));
        }

        [Test]
        public void When_filling_in_a_field_It_clicks_to_ensure_focus_event()
        {
            var element = StubField("Some field locator");

            browserSession.FillIn("Some field locator").With("some value for the field");

            Assert.That(driver.ClickedElements, Is.Empty);

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.ClickedElements);
        }

        [Test]
        public void When_filling_in_file_field_It_doesnt_click() {
            var element = StubField("Some field locator");
            element.StubAttribute("type", "file");

            browserSession.FillIn("Some field locator").With("some value for the field");

            RunQueryAndCheckTiming();

            Assert.That(driver.ClickedElements, Is.Empty);
        }

        [Test]
        public void When_selecting_an_option_It_finds_field_and_clicks_the_option_synchronised()
        {
            var stubbedOption = StubOption("Some select field locator", "Some option to select");

            browserSession.Select("Some option to select").From("Some select field locator");

            Assert.That(driver.ClickedElements, Has.No.Member(stubbedOption));

            RunQueryAndCheckTiming();

            var selectedOption = driver.ClickedElements.Single();
            Assert.That(selectedOption, Is.SameAs(stubbedOption));
        }

        [Test]
        public void When_checking_a_checkbox_It_find_fields_and_checks_robustly()
        {
            var element = StubField("Some checkbox locator");

            browserSession.Check("Some checkbox locator");

            Assert.That(driver.CheckedElements, Is.Empty);

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.CheckedElements);
        }

        private void AssertSingleElementEquals(StubElement element, IEnumerable<Element> elements)
        {
            Assert.That(elements.Cast<SynchronisedElementScope>().Single().Now(), Is.EqualTo(element));
        }

        [Test]
        public void When_unchecking_a_checkbox_It_finds_field_and_unchecks_robustly()
        {
            var element = StubField("Some checkbox locator");

            browserSession.Uncheck("Some checkbox locator");

            Assert.That(driver.UncheckedElements, Has.No.Member(element));

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.UncheckedElements);
        }

        [Test]
        public void When_choosing_a_radio_button_It_finds_field_and_chooses_robustly()
        {
            var element = StubField("Some radio locator");

            browserSession.Choose("Some radio locator");

            Assert.That(driver.ChosenElements, Has.No.Member(element));

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.ChosenElements);
        }

        [Test]
        public void It_sends_keys_to_element_via_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromRobustly(element);

            browserSession.FindId("something")
                          .SendKeys("some keys to press");

            Assert.That(driver.SentKeys.Count, Is.EqualTo(0));

            RunQueryAndCheckTiming();

            Assert.That(driver.SentKeys.Count, Is.EqualTo(1));
            Assert.That(driver.SentKeys[element], Is.EqualTo("some keys to press"));
        }
    }
}