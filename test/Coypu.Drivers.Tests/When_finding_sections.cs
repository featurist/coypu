using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_sections : DriverSpecs
    {
        [Fact]
        public void Finds_by_h1_text()
        {
            Section("Section One h1").Id.should_be("sectionOne");
            Section("Section Two h1").Id.should_be("sectionTwo");
        }

        [Fact]
        public void Finds_by_h2_text()
        {
            Section("Section One h2").Id.should_be("sectionOne");
            Section("Section Two h2").Id.should_be("sectionTwo");
        }

        [Fact]
        public void Finds_by_h3_text()
        {
            Section("Section One h3").Id.should_be("sectionOne");
            Section("Section Two h3").Id.should_be("sectionTwo");
        }

        [Fact]
        public void Finds_by_h6_text()
        {
            Section("Section One h6").Id.should_be("sectionOne");
            Section("Section Two h6").Id.should_be("sectionTwo");
        }

        [Fact]
        public void Finds_section_by_id()
        {
            Section("sectionOne").Id.should_be("sectionOne");
            Section("sectionTwo").Id.should_be("sectionTwo");
        }


        [Fact]
        public void Only_finds_div_and_section()
        {
            Assert.Throws<MissingHtmlException>(() => Section("scope1TextInputFieldId"));
            Assert.Throws<MissingHtmlException>(() => Section("fieldsetScope2"));
        }
    }
}