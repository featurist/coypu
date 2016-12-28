using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Drivers;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
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
            var stubSelect = new StubElement { Id = Guid.NewGuid().ToString() };
            var stubOption = new StubElement { Id = Guid.NewGuid().ToString() };
            var selectXpath = new Html(sessionConfiguration.Browser.UppercaseTagNames).Select(fieldLocator, options ?? sessionConfiguration);
            var optionXpath = new Html(sessionConfiguration.Browser.UppercaseTagNames).Option(optionLocator, options ?? sessionConfiguration);
            driver.StubAllXPath(selectXpath, new[] { stubSelect }, browserSession, options ?? sessionConfiguration);
            driver.StubAllXPath(optionXpath, new[] { stubOption }, new SnapshotElementScope(stubSelect, browserSession, options), options ?? sessionConfiguration);

            return stubOption;
        }
        
        [Fact]
        public void When_filling_in_a_text_field_It_finds_field_and_sets_value_robustly()
        {
            var element = StubField("Some field locator");

            browserSession.FillIn("Some field locator").With("some value for the field");

            Assert.DoesNotContain(element, driver.SetFields.Keys);

            RunQueryAndCheckTiming();

            var setField = driver.SetFields.Keys.Cast<SynchronisedElementScope>().Single().Now();
            Assert.Same(element, setField);
            Assert.Equal("some value for the field", driver.SetFields.Values.Single().Value);
        }

        [Fact]
        public void When_filling_in_a_field_It_clicks_to_ensure_focus_event()
        {
            var element = StubField("Some field locator");

            browserSession.FillIn("Some field locator").With("some value for the field");

            Assert.Empty(driver.ClickedElements);

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.ClickedElements);
        }

        [Fact]
        public void When_filling_in_file_field_It_doesnt_click() {
            var element = StubField("Some field locator");
            element.StubAttribute("type", "file");

            browserSession.FillIn("Some field locator").With("some value for the field");

            RunQueryAndCheckTiming();

            Assert.Empty(driver.ClickedElements);
        }

        [Fact]
        public void When_selecting_an_option_It_finds_field_and_clicks_the_option_synchronised()
        {
            var stubbedOption = StubOption("Some select field locator", "Some option to select");

            browserSession.Select("Some option to select").From("Some select field locator");

            Assert.DoesNotContain(stubbedOption, driver.ClickedElements);

            RunQueryAndCheckTiming();

            var selectedOption = driver.ClickedElements.Single();
            Assert.Same(stubbedOption, selectedOption);
        }

        [Fact]
        public void When_checking_a_checkbox_It_find_fields_and_checks_robustly()
        {
            var element = StubField("Some checkbox locator");

            browserSession.Check("Some checkbox locator");

            Assert.Empty(driver.CheckedElements);

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.CheckedElements);
        }

        private void AssertSingleElementEquals(StubElement element, IEnumerable<Element> elements)
        {
            Assert.Equal(element, elements.Cast<SynchronisedElementScope>().Single().Now());
        }

        [Fact]
        public void When_unchecking_a_checkbox_It_finds_field_and_unchecks_robustly()
        {
            var element = StubField("Some checkbox locator");

            browserSession.Uncheck("Some checkbox locator");

            Assert.DoesNotContain(element, driver.UncheckedElements);

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.UncheckedElements);
        }

        [Fact]
        public void When_choosing_a_radio_button_It_finds_field_and_chooses_robustly()
        {
            var element = StubField("Some radio locator");

            browserSession.Choose("Some radio locator");

            Assert.DoesNotContain(element, driver.ChosenElements);

            RunQueryAndCheckTiming();

            AssertSingleElementEquals(element, driver.ChosenElements);
        }

        [Fact]
        public void It_sends_keys_to_element_via_underlying_driver()
        {
            var element = new StubElement();
            driver.StubId("something", element, browserSession, sessionConfiguration);
            SpyTimingStrategy.AlwaysReturnFromSynchronise(element);

            browserSession.FindId("something")
                          .SendKeys("some keys to press");

            Assert.Equal(0, driver.SentKeys.Count);

            RunQueryAndCheckTiming();

            Assert.Equal(1, driver.SentKeys.Count);
            Assert.Equal("some keys to press", driver.SentKeys[element]);
        }
    }
}