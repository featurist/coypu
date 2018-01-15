using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_sections : DriverSpecs
    {
        [Test]
        public void Finds_by_h1_text()
        {
            Section("Section One h1").Id.ShouldBe("sectionOne");
            Section("Section Two h1").Id.ShouldBe("sectionTwo");
        }

        [Test]
        public void Finds_by_h2_text()
        {
            Section("Section One h2").Id.ShouldBe("sectionOne");
            Section("Section Two h2").Id.ShouldBe("sectionTwo");
        }

        [Test]
        public void Finds_by_h3_text()
        {
            Section("Section One h3").Id.ShouldBe("sectionOne");
            Section("Section Two h3").Id.ShouldBe("sectionTwo");
        }

        [Test]
        public void Finds_by_h6_text()
        {
            Section("Section One h6").Id.ShouldBe("sectionOne");
            Section("Section Two h6").Id.ShouldBe("sectionTwo");
        }

        [Test]
        public void Finds_section_by_id()
        {
            Section("sectionOne").Id.ShouldBe("sectionOne");
            Section("sectionTwo").Id.ShouldBe("sectionTwo");
        }


        [Test]
        public void Only_finds_div_and_section()
        {
            Assert.Throws<MissingHtmlException>(() => Section("scope1TextInputFieldId"));
            Assert.Throws<MissingHtmlException>(() => Section("fieldsetScope2"));
        }
    }
}