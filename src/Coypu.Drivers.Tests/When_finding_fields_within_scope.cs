using System;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_within_scope : DriverSpecs
    {
        internal override void Specs()
        {
            describe["within scope1"] = () =>
            {
                before = () => driver.SetScope(() => driver.FindId("scope1", Root));

                it["finds text input by for"] = () =>
                {
                    driver.FindField("scoped text input field linked by for", Root).Id.should_be("scope1TextInputFieldId");
                };
                it["finds text input in container label"] = () =>
                {
                    driver.FindField("scoped text input field in a label container", Root).Id.should_be("scope1ContainerLabeledTextInputFieldId");
                };
                it["finds text input by placeholder"] = () =>
                {
                    driver.FindField("scoped text input field with a placeholder", Root).Id.should_be("scope1TextInputFieldWithPlaceholder");
                };
                it["finds text input by name"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName", Root).Id.should_be("scope1ContainerLabeledTextInputFieldId");
                };
                it["finds radio button by value"] = () =>
                {
                    driver.FindField("scoped radio field one val", Root).Id.should_be("scope1RadioFieldId");
                };
                it["finds not find text input by id outside scope"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindField("containerLabeledTextInputFieldId", Root));
                };
            };
            describe["within scope2"] = () =>
            {
                before = () => driver.SetScope(() => driver.FindId("scope2", Root));

                it["finds text input"] = () =>
                {
                    driver.FindField("scoped text input field linked by for", Root).Id.should_be("scope2TextInputFieldId");
                };
                it["finds text input in container label"] = () =>
                {
                    driver.FindField("scoped text input field in a label container", Root).Id.should_be("scope2ContainerLabeledTextInputFieldId");
                };
                it["finds text input by placeholder"] = () =>
                {
                    driver.FindField("scoped text input field with a placeholder", Root).Id.should_be("scope2TextInputFieldWithPlaceholder");
                };
                it["finds text input by name"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName", Root).Id.should_be("scope2ContainerLabeledTextInputFieldId");
                };
                it["finds radio button by value"] = () =>
                {
                    driver.FindField("scoped radio field one val", Root).Id.should_be("scope2RadioFieldId");
                };
                it["finds not find text input by id outside scope"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindField("containerLabeledTextInputFieldId", Root));
                };

            };
        }
    }
}