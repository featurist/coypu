using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_insepcting_elements : BrowserInteractionTests
    {
        [Test]
        public void It_finds_element_robustly_and_returns_id()
        {
            var stubElement = new StubElement {Id = "actual-id"};
            driver.StubId("some-element",stubElement);
            
            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Id, Is.EqualTo("actual-id"));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_text()
        {
            var stubElement = new StubElement { Text = "actual-text" };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Text, Is.EqualTo("actual-text"));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_value()
        {
            var stubElement = new StubElement { Value = "actual-value" };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Value, Is.EqualTo("actual-value"));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_name()
        {
            var stubElement = new StubElement { Name = "actual-name" };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Name, Is.EqualTo("actual-name"));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_selected_option()
        {
            var stubElement = new StubElement { SelectedOption = "actual-selected-option" };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").SelectedOption, Is.EqualTo("actual-selected-option"));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_selected_positive()
        {
            var stubElement = new StubElement { Selected = true };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Selected, Is.EqualTo(true));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_selected_negative()
        {
            var stubElement = new StubElement { Selected = false };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Selected, Is.EqualTo(false));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }

        [Test]
        public void It_finds_element_robustly_and_returns_native()
        {
            var native = new object();
            var stubElement = new StubElement { Native = native };
            driver.StubId("some-element", stubElement);

            spyRobustWrapper.AlwaysReturnFromRobustly(stubElement);

            Assert.That(session.FindId("some-element").Native, Is.SameAs(native));
            Assert.That(spyRobustWrapper.DeferredFinders.Single().Find(), Is.SameAs(stubElement));
        }
    }
}