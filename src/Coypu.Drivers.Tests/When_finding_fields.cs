using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields : DriverSpecs
    {
        public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it, Action<Action> setBefore)
        {
            return () =>
            {
                describe["when finding by for attribute"] = () =>
                {
                    it["should find text input"] = () =>
                    {
                        driver().FindField("text input field linked by for").Id.should_be("forLabeledTextInputFieldId");
                    };
                    it["should find password field"] = () =>
                    {
                        driver().FindField("password field linked by for").Id.should_be("forLabeledPasswordFieldId");
                    };
                    it["should find select field"] = () =>
                    {
                        driver().FindField("select field linked by for").Id.should_be("forLabeledSelectFieldId");
                    };
                    it["should find checkbox"] = () =>
                    {
                        driver().FindField("checkbox field linked by for").Id.should_be("forLabeledCheckboxFieldId");
                    };
                    it["should find radio button"] = () =>
                    {
                        driver().FindField("radio field linked by for").Id.should_be("forLabeledRadioFieldId");
                    };
                    it["should find textarea"] = () =>
                    {
                        driver().FindField("textarea field linked by for").Id.should_be("forLabeledTextareaFieldId");
                    };
                };
                it["should find field by container label"] = () =>
                {
                    driver().FindField("text input field in a label container").Id.should_be("containerLabeledTextInputFieldId");
                    driver().FindField("password field in a label container").Id.should_be("containerLabeledPasswordFieldId");
                    driver().FindField("checkbox field in a label container").Id.should_be("containerLabeledCheckboxFieldId");
                    driver().FindField("radio field in a label container").Id.should_be("containerLabeledRadioFieldId");
                    driver().FindField("select field in a label container").Id.should_be("containerLabeledSelectFieldId");
                    driver().FindField("textarea field in a label container").Id.should_be("containerLabeledTextareaFieldId");

                };
                it["should find text field by placeholder"] = () =>
                {
                    driver().FindField("text input field with a placeholder").Id.should_be("textInputFieldWithPlaceholder");
                    driver().FindField("textarea field with a placeholder").Id.should_be("textareaFieldWithPlaceholder");
                };
                it["should find field by id"] = () =>
                {
                    driver().FindField("containerLabeledTextInputFieldId").Value.should_be("text input field two val");
                    driver().FindField("containerLabeledTextareaFieldId").Value.should_be("textarea field two val");
                    driver().FindField("containerLabeledSelectFieldId").Name.should_be("containerLabeledSelectFieldName");
                    driver().FindField("containerLabeledCheckboxFieldId").Value.should_be("checkbox field two val");
                    driver().FindField("containerLabeledRadioFieldId").Value.should_be("radio field two val");
                    driver().FindField("containerLabeledPasswordFieldId").Name.should_be("containerLabeledPasswordFieldName");
                };
                it["should find field by name"] = () =>
                {
                    driver().FindField("containerLabeledTextInputFieldName").Value.should_be("text input field two val");
                    driver().FindField("containerLabeledTextareaFieldName").Value.should_be("textarea field two val");
                    driver().FindField("containerLabeledSelectFieldName").Id.should_be("containerLabeledSelectFieldId");
                    driver().FindField("containerLabeledCheckboxFieldName").Value.should_be("checkbox field two val");
                    driver().FindField("containerLabeledRadioFieldName").Value.should_be("radio field two val");
                    driver().FindField("containerLabeledPasswordFieldName").Id.should_be("containerLabeledPasswordFieldId");
                };
                it["should find radio button by value"] = () =>
                {
                    driver().FindField("radio field one val").Name.should_be("forLabeledRadioFieldName");
                    driver().FindField("radio field two val").Name.should_be("containerLabeledRadioFieldName");
                };
            };
        }
    }
}