using System;
using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_within_scope : DriverSpecs
    {
        internal override void Specs()
        {
            DriverScope scope = null;
            describe["within scope1"] = () =>
            {
                before = () => scope = new DriverScope(new IdFinder(driver, "scope1", Root), null);

                it["finds text input by for"] = () =>
                {
                    driver.FindField("scoped text input field linked by for", scope).Id.should_be("scope1TextInputFieldId");
                };
                it["finds text input in container label"] = () =>
                {
                    driver.FindField("scoped text input field in a label container", scope).Id.should_be("scope1ContainerLabeledTextInputFieldId");
                };
                it["finds text input by placeholder"] = () =>
                {
                    driver.FindField("scoped text input field with a placeholder", scope).Id.should_be("scope1TextInputFieldWithPlaceholder");
                };
                it["finds text input by name"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName", scope).Id.should_be("scope1ContainerLabeledTextInputFieldId");
                };
                it["finds radio button by value"] = () =>
                {
                    driver.FindField("scoped radio field one val", scope).Id.should_be("scope1RadioFieldId");
                };
                it["finds not find text input by id outside scope"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindField("containerLabeledTextInputFieldId", scope));
                };
            };
            describe["within scope2"] = () =>
            {
                before = () => scope = new DriverScope(new IdFinder(driver, "scope2", Root), null);

                it["finds text input"] = () =>
                {
                    driver.FindField("scoped text input field linked by for", scope).Id.should_be("scope2TextInputFieldId");
                };
                it["finds text input in container label"] = () =>
                {
                    driver.FindField("scoped text input field in a label container", scope).Id.should_be("scope2ContainerLabeledTextInputFieldId");
                };
                it["finds text input by placeholder"] = () =>
                {
                    driver.FindField("scoped text input field with a placeholder", scope).Id.should_be("scope2TextInputFieldWithPlaceholder");
                };
                it["finds text input by name"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName", scope).Id.should_be("scope2ContainerLabeledTextInputFieldId");
                };
                it["finds radio button by value"] = () =>
                {
                    driver.FindField("scoped radio field one val", scope).Id.should_be("scope2RadioFieldId");
                };
                it["finds not find text input by id outside scope"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindField("containerLabeledTextInputFieldId", scope));
                };

            };
        }
    }
}