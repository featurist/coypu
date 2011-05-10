using System;
using Coypu.Drivers.Watin;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver))]
    internal class When_finding_fields_within_scope : DriverSpecs
    {
        internal override void Specs()
        {
            describe["within scope1"] = () =>
            {
                before = () => driver.SetScope(() => driver.FindCss("#scope1"));

                it["should find text input by for"] = () =>
                {
                    driver.FindField("scoped text input field linked by for").Id.should_be("scope1TextInputFieldId");
                };
                it["should find text input in container label"] = () =>
                {
                    driver.FindField("scoped text input field in a label container").Id.should_be("scope1ContainerLabeledTextInputFieldId");
                };
                it["should find text input by placeholder"] = () =>
                {
                    driver.FindField("scoped text input field with a placeholder").Id.should_be("scope1TextInputFieldWithPlaceholder");
                };
                it["should find text input by name"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName").Id.should_be("scope1ContainerLabeledTextInputFieldId");
                };
                it["should find radio button by value"] = () =>
                {
                    driver.FindField("scoped radio field one val").Id.should_be("scope1RadioFieldId");
                };
                it["should find not find text input by id outside scope"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindField("containerLabeledTextInputFieldId"));
                };
            };
            describe["within scope2"] = () =>
            {
                before = () => driver.SetScope(() => driver.FindCss("#scope2"));

                it["should find text input"] = () =>
                {
                    driver.FindField("scoped text input field linked by for").Id.should_be("scope2TextInputFieldId");
                };
                it["should find text input in container label"] = () =>
                {
                    driver.FindField("scoped text input field in a label container").Id.should_be("scope2ContainerLabeledTextInputFieldId");
                };
                it["should find text input by placeholder"] = () =>
                {
                    driver.FindField("scoped text input field with a placeholder").Id.should_be("scope2TextInputFieldWithPlaceholder");
                };
                it["should find text input by name"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName").Id.should_be("scope2ContainerLabeledTextInputFieldId");
                };
                it["should find radio button by value"] = () =>
                {
                    driver.FindField("scoped radio field one val").Id.should_be("scope2RadioFieldId");
                };
                it["should find not find text input by id outside scope"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindField("containerLabeledTextInputFieldId"));
                };

            };
        }
    }
}