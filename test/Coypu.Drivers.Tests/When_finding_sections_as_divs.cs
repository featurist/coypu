using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_sections_as_divs : DriverSpecs
    {
        [Fact]
        public void Finds_by_h1_text()
        {
            Section("Div Section One h1").Id.ShouldBe("divSectionOne");
            Section("Div Section Two h1").Id.ShouldBe("divSectionTwo");
        }

        [Fact]
        public void Finds_by_h2_text()
        {
            Section("Div Section One h2").Id.ShouldBe("divSectionOne");
            Section("Div Section Two h2").Id.ShouldBe("divSectionTwo");
        }

        [Fact]
        public void Finds_by_h3_text()
        {
            Section("Div Section One h3").Id.ShouldBe("divSectionOne");
            Section("Div Section Two h3").Id.ShouldBe("divSectionTwo");
        }

        [Fact]
        public void Finds_by_h6_text()
        {
            Section("Div Section One h6").Id.ShouldBe("divSectionOne");
            Section("Div Section Two h6").Id.ShouldBe("divSectionTwo");
        }


        [Fact]
        public void Finds_by_h2_text_within_child_link()
        {
            Section("Div Section One h2 with link").Id.ShouldBe("divSectionOneWithLink");
            Section("Div Section Two h2 with link").Id.ShouldBe("divSectionTwoWithLink");
        }


        [Fact]
        public void Finds_by_div_by_id()
        {
            Section("divSectionOne").Native.ShouldBe(Section("Div Section One h1").Native);
            Section("divSectionTwo").Native.ShouldBe(Section("Div Section Two h1").Native);
        }
    }
}