using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_content_within_scope : DriverSpecs
    {
        private DriverScope scope1;
        private DriverScope scope2;

        [SetUp]
        public void SetUpScope()
        {
            scope1 = new DriverScope(new SessionConfiguration(), new IdFinder(Driver, "scope1", Root), Driver,null,null,null);
            scope2 = new DriverScope(new SessionConfiguration(), new IdFinder(Driver, "scope2", Root), Driver,null,null,null);
        }

        [Test]
        public void Finds_content_within_scope()
        {
            Driver.HasContent("Scope 1", scope1).should_be_true();
            Driver.HasContent("Scope 2", scope2).should_be_true();
        }

        [Test]
        public void Does_not_find_content_outside_scope()
        {
            Driver.HasContent("Scope 2", scope1).should_be_false();
            Driver.HasContent("Scope 1", scope2).should_be_false();
        }
    }
}