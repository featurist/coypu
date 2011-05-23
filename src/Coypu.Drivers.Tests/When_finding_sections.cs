using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_sections : DriverSpecs
    {
        internal override void Specs()
        {
			describe["finding sections by header text"] = () =>
			{
				context["with section tags"] = () =>
				{
					it["finds by h1 text"] = () =>
					{
						driver.FindSection("Section One h1").Id.should_be("sectionOne");
						driver.FindSection("Section Two h1").Id.should_be("sectionTwo");
					};
					it["finds by h2 text"] = () =>
					{
						driver.FindSection("Section One h2").Id.should_be("sectionOne");
						driver.FindSection("Section Two h2").Id.should_be("sectionTwo");
					};
					it["finds by h3 text"] = () =>
					{
						driver.FindSection("Section One h3").Id.should_be("sectionOne");
						driver.FindSection("Section Two h3").Id.should_be("sectionTwo");
					};
					it["finds by h6 text"] = () =>
					{
						driver.FindSection("Section One h6").Id.should_be("sectionOne");
						driver.FindSection("Section Two h6").Id.should_be("sectionTwo");
					};
				};

				context["with div tags"] = () =>
				{
					it["finds by h1 text"] = () =>
					{
						driver.FindSection("Div Section One h1").Id.should_be("divSectionOne");
						driver.FindSection("Div Section Two h1").Id.should_be("divSectionTwo");
					};
					it["finds by h2 text"] = () =>
					{
						driver.FindSection("Div Section One h2").Id.should_be("divSectionOne");
						driver.FindSection("Div Section Two h2").Id.should_be("divSectionTwo");
					};
					it["finds by h3 text"] = () =>
					{
						driver.FindSection("Div Section One h3").Id.should_be("divSectionOne");
						driver.FindSection("Div Section Two h3").Id.should_be("divSectionTwo");
					};
					it["finds by h6 text"] = () =>
					{
						driver.FindSection("Div Section One h6").Id.should_be("divSectionOne");
						driver.FindSection("Div Section Two h6").Id.should_be("divSectionTwo");
					};
				};
			};
			it["finds by section by id"] = () =>
			{
				driver.FindSection("sectionOne").Native.should_be(driver.FindSection("Section One h1").Native);
				driver.FindSection("sectionTwo").Native.should_be(driver.FindSection("Section Two h1").Native);
			};
			it["finds by div by id"] = () =>
			{
				driver.FindSection("divSectionOne").Native.should_be(driver.FindSection("Div Section One h1").Native);
				driver.FindSection("divSectionTwo").Native.should_be(driver.FindSection("Div Section Two h1").Native);
			};
			it["only finds div and section"] = () =>
			{
				Assert.Throws<MissingHtmlException>(() => driver.FindSection("scope1TextInputFieldId"));
				Assert.Throws<MissingHtmlException>(() => driver.FindSection("fieldsetScope2"));
			};
        }
    }
}