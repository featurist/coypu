using System;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_insepcting_elements : BrowserInteractionTests
    {
        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_id()
        {
            Synchronises_FindElement_and_returns_property_value("actual-id", (element, value) => element.Id = value, (element => element.Id));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_text()
        {
            Synchronises_FindElement_and_returns_property_value("actual-text", (element, value) => element.Text = value, (element => element.Text));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_value()
        {
            Synchronises_FindElement_and_returns_property_value("actual-value", (element, value) => element.Value = value, (element => element.Value));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_attributes()
        {
            Synchronises_FindElement_and_returns_property_value("http://some.href", (element, value) => element.StubAttribute("href",value), (element => element["href"]));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_name()
        {
            Synchronises_FindElement_and_returns_property_value("actual-name", (element, value) => element.Name = value, (element => element.Name));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_selected_option()
        {
            Synchronises_FindElement_and_returns_property_value("actual-selected-option", (element, value) => element.SelectedOption = value, (element => element.SelectedOption));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_selected_positive()
        {
            Synchronises_FindElement_and_returns_property_value(true, (element, value) => element.Selected = value, (element => element.Selected), Is.EqualTo);
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_disabled_negative()
        {
            Synchronises_FindElement_and_returns_property_value(false, (element, value) => element.Disabled = value, (element => element.Disabled), Is.EqualTo);
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_disabled_positive()
        {
            Synchronises_FindElement_and_returns_property_value(true, (element, value) => element.Disabled = value, (element => element.Disabled), Is.EqualTo);
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_selected_negative()
        {
            Synchronises_FindElement_and_returns_property_value(false, (element, value) => element.Selected = value, (element => element.Selected), Is.EqualTo);
        }
        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_native()
        {
            Synchronises_FindElement_and_returns_property_value(new object(), (element,value) => element.Native = value, (element => element.Native));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_innerHTML()
        {
            Synchronises_FindElement_and_returns_property_value("actual-innerHTML", (element, value) => element.InnerHTML = value, (element => element.InnerHTML));
        }

        [Test]
        public void It_synchronises_both_find_element_and_attribute_getter_for_outerHTML()
        {
            Synchronises_FindElement_and_returns_property_value("actual-outerHTML", (element,value) => element.OuterHTML = value, (element => element.OuterHTML));
        }

        private void Synchronises_FindElement_and_returns_property_value<T>(T testValue, Action<StubElement, T> stubProperty, Func<ElementScope, T> getProperty, Func<object,Constraint> constraint = null)
        {
            constraint = constraint ?? Is.SameAs;
            var stubElement = new StubElement();

            stubProperty(stubElement, testValue);

            driver.StubId("some-element", stubElement, browserSession, sessionConfiguration);

            SpyTimingStrategy.ReturnOnceThenExecuteImmediately(stubElement);

            if (typeof (T) == typeof (bool))
            {
                Assert.That(getProperty(browserSession.FindId("some-element")), constraint(testValue));
            }
            else
            {
                Assert.That(getProperty(browserSession.FindId("some-element")), constraint(testValue));
            }

            var queryResult = RunQueryAndCheckTiming();

            Assert.That(queryResult, Is.SameAs(stubElement));
        }
    }
}
