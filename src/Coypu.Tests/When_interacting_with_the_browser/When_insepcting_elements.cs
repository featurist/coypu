using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_insepcting_elements : BrowserInteractionTests
    {
        [Test]
        public void It_finds_element_and_returns_id()
        {
            driver.StubId("some-element",new StubElement{Id = "actual-id"});
            
            Assert.That(session.FindId("some-element").Id, Is.EqualTo("actual-id"));
        }

        [Test]
        public void It_finds_element_and_returns_text()
        {
            Assert.Fail("todo");
        }

        [Test]
        public void It_finds_element_and_returns_value()
        {
            Assert.Fail("todo");
        }

        [Test]
        public void It_finds_element_and_returns_name()
        {
            Assert.Fail("todo");
        }

        [Test]
        public void It_finds_element_and_returns_selected_option()
        {
            Assert.Fail("todo");
        }

        [Test]
        public void It_finds_element_and_returns_selected()
        {
            Assert.Fail("todo");
        }

        [Test]
        public void It_finds_element_and_returns_native()
        {
            Assert.Fail("todo");
        }
    }
}