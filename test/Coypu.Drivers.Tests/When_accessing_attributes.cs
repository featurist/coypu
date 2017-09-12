using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_accessing_attributes : DriverSpecs
    {
        [Fact]
        public void Exposes_element_attributes()
        {
            var formWithAttributesToTest = Id("attributeTestForm", Root, DefaultOptions);
            formWithAttributesToTest["id"].ShouldBe("attributeTestForm");
            formWithAttributesToTest["method"].ShouldBe("post");
            formWithAttributesToTest["action"].ShouldBe("http://somesite.com/action.htm");
            formWithAttributesToTest["target"].ShouldBe("_parent");
        }
    }
}