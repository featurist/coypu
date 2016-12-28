using System;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_insepcting_elements : BrowserInteractionTests
    {
        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_id()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-id", (element, value) => element.Id = value, (element => element.Id));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_text()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-text", (element, value) => element.Text = value, (element => element.Text));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_value()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-value", (element, value) => element.Value = value, (element => element.Value));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_attributes()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("http://some.href", (element, value) => element.StubAttribute("href",value), (element => element["href"]));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_name()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-name", (element, value) => element.Name = value, (element => element.Name));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_selected_option()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-selected-option", (element, value) => element.SelectedOption = value, (element => element.SelectedOption));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_selected_positive()
        {
            Is_Equal_Synchronises_FindElement_and_returns_property_value(true, (element, value) => element.Selected = value, (element => element.Selected));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_disabled_negative()
        {
            Is_Equal_Synchronises_FindElement_and_returns_property_value(false, (element, value) => element.Disabled = value, (element => element.Disabled));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_disabled_positive()
        {
            Is_Equal_Synchronises_FindElement_and_returns_property_value(true, (element, value) => element.Disabled = value, (element => element.Disabled));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_selected_negative()
        {
            Is_Equal_Synchronises_FindElement_and_returns_property_value(false, (element, value) => element.Selected = value, (element => element.Selected));
        }
        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_native()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value(new object(), (element,value) => element.Native = value, (element => element.Native));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_innerHTML()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-innerHTML", (element, value) => element.InnerHTML = value, (element => element.InnerHTML));
        }

        [Fact]
        public void It_synchronises_both_find_element_and_attribute_getter_for_outerHTML()
        {
            Is_Same_Synchronises_FindElement_and_returns_property_value("actual-outerHTML", (element,value) => element.OuterHTML = value, (element => element.OuterHTML));
        }

        private void Is_Equal_Synchronises_FindElement_and_returns_property_value<T>(T testValue, Action<StubElement, T> stubProperty, Func<ElementScope, T> getProperty)
        {
            var stubElement = new StubElement();

            stubProperty(stubElement, testValue);

            driver.StubId("some-element", stubElement, browserSession, sessionConfiguration);

            SpyTimingStrategy.ReturnOnceThenExecuteImmediately(stubElement);

            if (typeof(T) == typeof(bool))
            {
                Assert.Equal(testValue, getProperty(browserSession.FindId("some-element")));
            }
            else
            {
                Assert.Equal(testValue, getProperty(browserSession.FindId("some-element")));
            }

            var queryResult = RunQueryAndCheckTiming();

            Assert.Same(stubElement, queryResult);
        }
        private void Is_Same_Synchronises_FindElement_and_returns_property_value<T>(T testValue, Action<StubElement, T> stubProperty, Func<ElementScope, T> getProperty)
        {
            var stubElement = new StubElement();

            stubProperty(stubElement, testValue);

            driver.StubId("some-element", stubElement, browserSession, sessionConfiguration);

            SpyTimingStrategy.ReturnOnceThenExecuteImmediately(stubElement);

            if (typeof(T) == typeof(bool))
            {
                Assert.Same(testValue, getProperty(browserSession.FindId("some-element")));
            }
            else
            {
                Assert.Same(testValue, getProperty(browserSession.FindId("some-element")));
            }

            var queryResult = RunQueryAndCheckTiming();

            Assert.Same(stubElement, queryResult);
        }
    }
}
