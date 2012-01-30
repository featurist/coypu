using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_considering_invisible_elements : BrowserInteractionTests
    {
        [Test]
        public void It_sets_the_driver_scope_to_consider_invisible_elements()
        {
            Assert.That(session.DriverScope.ConsiderInvisibleElements, Is.False);

            session.ConsideringInvisibleElements();

            Assert.That(session.DriverScope.ConsiderInvisibleElements, Is.True);

            session.ConsideringOnlyVisibleElements();

            Assert.That(session.DriverScope.ConsiderInvisibleElements, Is.False);
        }
    }
}