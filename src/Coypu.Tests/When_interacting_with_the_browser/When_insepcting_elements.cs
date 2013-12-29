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
        public void It_finds_element_robustly_and_returns_id()
        {
            Finds_element_robustly_and_returns_property_value("actual-id", (element, value) => element.Id = value, (element => element.Id));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_text()
        {
            Finds_element_robustly_and_returns_property_value("actual-text", (element, value) => element.Text = value, (element => element.Text));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_value()
        {
            Finds_element_robustly_and_returns_property_value("actual-value", (element, value) => element.Value = value, (element => element.Value));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_attributes()
        {
            Finds_element_robustly_and_returns_property_value("http://some.href", (element, value) => element.StubAttribute("href",value), (element => element["href"]));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_name()
        {
            Finds_element_robustly_and_returns_property_value("actual-name", (element, value) => element.Name = value, (element => element.Name));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_selected_option()
        {
            Finds_element_robustly_and_returns_property_value("actual-selected-option", (element, value) => element.SelectedOption = value, (element => element.SelectedOption));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_selected_positive()
        {
            Finds_element_robustly_and_returns_property_value(true, (element, value) => element.Selected = value, (element => element.Selected), Is.EqualTo);
        }

        [Test]
        public void It_finds_element_robustly_and_returns_selected_negative()
        {
            Finds_element_robustly_and_returns_property_value(false, (element, value) => element.Selected = value, (element => element.Selected), Is.EqualTo);
        }

        [Test]
        public void It_finds_element_robustly_and_returns_native()
        {
            Finds_element_robustly_and_returns_property_value(new object(), (element,value) => element.Native = value, (element => element.Native));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_innerHTML()
        {
            Finds_element_robustly_and_returns_property_value("actual-innerHTML", (element, value) => element.InnerHTML = value, (element => element.InnerHTML));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_outerHTML()
        {
            Finds_element_robustly_and_returns_property_value("actual-outerHTML", (element,value) => element.OuterHTML = value, (element => element.OuterHTML));
        }

        private void Finds_element_robustly_and_returns_property_value<T>(T testValue, Action<StubElement, T> stubProperty, Func<ElementScope, T> getProperty, Func<object,Constraint> constraint = null)
        {
            constraint = constraint ?? Is.SameAs;
            var stubElement = new StubElement();

            stubProperty(stubElement, testValue);

            driver.StubId("some-element", stubElement, browserSession, sessionConfiguration);

            SpyTimingStrategy.AlwaysReturnFromRobustly(stubElement);

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
