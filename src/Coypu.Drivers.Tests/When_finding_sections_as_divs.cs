using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_sections_as_divs : DriverSpecs
    {
        [Test]
        public void Finds_by_h1_text()
        {
            Driver.FindSection("Div Section One h1", Root).Id.should_be("divSectionOne");
            Driver.FindSection("Div Section Two h1", Root).Id.should_be("divSectionTwo");
        }

        [Test]
        public void Finds_by_h2_text()
        {
            Driver.FindSection("Div Section One h2", Root).Id.should_be("divSectionOne");
            Driver.FindSection("Div Section Two h2", Root).Id.should_be("divSectionTwo");
        }

        [Test]
        public void Finds_by_h3_text()
        {
            Driver.FindSection("Div Section One h3", Root).Id.should_be("divSectionOne");
            Driver.FindSection("Div Section Two h3", Root).Id.should_be("divSectionTwo");
        }

        [Test]
        public void Finds_by_h6_text()
        {
            Driver.FindSection("Div Section One h6", Root).Id.should_be("divSectionOne");
            Driver.FindSection("Div Section Two h6", Root).Id.should_be("divSectionTwo");
        }


        [Test]
        public void Finds_by_h2_text_within_child_link()
        {
            Driver.FindSection("Div Section One h2 with link", Root).Id.should_be("divSectionOneWithLink");
            Driver.FindSection("Div Section Two h2 with link", Root).Id.should_be("divSectionTwoWithLink");
        }


        [Test]
        public void Finds_by_div_by_id()
        {
            Driver.FindSection("divSectionOne", Root).Native.should_be(Driver.FindSection("Div Section One h1", Root).Native);
            Driver.FindSection("divSectionTwo", Root).Native.should_be(Driver.FindSection("Div Section Two h1", Root).Native);
        }
    }
}