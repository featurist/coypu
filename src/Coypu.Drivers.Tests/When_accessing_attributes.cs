using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_accessing_attributes : DriverSpecs
    {
        [Test]
        public void Exposes_element_attributes()
        {
            var formWithAttributesToTest = Id("attributeTestForm", Root, DefaultOptions);
            formWithAttributesToTest["id"].should_be("attributeTestForm");
            formWithAttributesToTest["method"].should_be("post");
            formWithAttributesToTest["action"].should_be("http://somesite.com/action.htm");
            formWithAttributesToTest["target"].should_be("_parent");
        }
    }
}