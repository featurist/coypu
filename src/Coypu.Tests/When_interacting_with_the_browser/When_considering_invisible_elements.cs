using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_considering_invisible_elements : BrowserInteractionTests
    {
        [Test]
        public void It_sets_the_driver_to_consider_invisible_elements_before_executing_action() {
            var actualConsiderInvisibleElements = false;
            Assert.That(driver.ConsiderInvisibleElements, Is.False);
            session.ConsideringInvisibleElements(() =>
            {
                actualConsiderInvisibleElements = driver.ConsiderInvisibleElements;
            });

            Assert.That(actualConsiderInvisibleElements, Is.True);
        }

        [Test]
        public void It_clears_the_scope_after_executing_action() {
            session.ConsideringInvisibleElements(() => { });

            Assert.That(driver.ConsiderInvisibleElements, Is.False);
        }

        [Test]
        public void It_clears_the_scope_after_exception_in_action() {

            Assert.Throws<ExplicitlyThrownTestException>(
                () => session.ConsideringInvisibleElements(
                    () => { throw new ExplicitlyThrownTestException("Exception from action within scope"); }));

            Assert.That(driver.ConsiderInvisibleElements, Is.False);
        }

        [Test]
        public void It_sets_the_scope_before_executing_function() 
        {
            bool actualConsiderInvisibleElements = false;
            session.ConsideringInvisibleElements(() =>
            {
                actualConsiderInvisibleElements = driver.ConsiderInvisibleElements;
                return new object();
            });

            Assert.That(actualConsiderInvisibleElements, Is.True);
        }

        [Test]
        public void It_clears_the_scope_after_executing_function() 
        {
            session.ConsideringInvisibleElements(() => new object());

            Assert.That(driver.ConsiderInvisibleElements, Is.False);
        }

        [Test]
        public void It_clears_the_scope_after_exception_in_function() 
        {
            Assert.Throws<ExplicitlyThrownTestException>(
                () => session.ConsideringInvisibleElements<object>(
                    () => { throw new ExplicitlyThrownTestException("Exception from action within scope"); }));

            Assert.That(driver.ConsiderInvisibleElements, Is.False);
        }

        [Test]
        public void It_returns_the_result_of_the_function() 
        {
            var expectedResult = new object();
            var actualResult = session.ConsideringInvisibleElements(() => expectedResult);

            Assert.That(expectedResult, Is.SameAs(actualResult));
        }
    }
}