using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields : DriverSpecs
    {
        internal override void Specs()
        {
            describe["finding by for attribute"] = () =>
            {
                it["finds text input"] = () =>
                {
                    driver.FindField("text input field linked by for", Root).Id.should_be("forLabeledTextInputFieldId");
                };
                it["finds password field"] = () =>
                {
                    driver.FindField("password field linked by for", Root).Id.should_be("forLabeledPasswordFieldId");
                };
                it["finds select field"] = () =>
                {
                    driver.FindField("select field linked by for", Root).Id.should_be("forLabeledSelectFieldId");
                };
                it["finds checkbox"] = () =>
                {
                    driver.FindField("checkbox field linked by for", Root).Id.should_be("forLabeledCheckboxFieldId");
                };
                it["finds radio button"] = () =>
                {
                    driver.FindField("radio field linked by for", Root).Id.should_be("forLabeledRadioFieldId");
                };
                it["finds textarea"] = () =>
                {
                    driver.FindField("textarea field linked by for", Root).Id.should_be("forLabeledTextareaFieldId");
                };
                it["finds file input"] = () =>
                {
                    driver.FindField("file field linked by for", Root).Id.should_be("forLabeledFileFieldId");
                };
            };
            describe["finding by container label"] = () =>
            {
                it["finds text input"] = () =>
                {
                    driver.FindField("text input field in a label container", Root).Id.should_be("containerLabeledTextInputFieldId");
                };
                it["finds password"] = () =>
                {
                    driver.FindField("password field in a label container", Root).Id.should_be("containerLabeledPasswordFieldId");
                };
                it["finds checkbox"] = () =>
                {
                    driver.FindField("checkbox field in a label container", Root).Id.should_be("containerLabeledCheckboxFieldId");
                };
                it["finds radio"] = () =>
                {
                    driver.FindField("radio field in a label container", Root).Id.should_be("containerLabeledRadioFieldId");
                };
                it["finds select"] = () =>
                {
                    driver.FindField("select field in a label container", Root).Id.should_be("containerLabeledSelectFieldId");
                };
                it["finds textarea"] = () =>
                {
                    driver.FindField("textarea field in a label container", Root).Id.should_be("containerLabeledTextareaFieldId");
                };
                it["finds file field"] = () =>
                {
                    driver.FindField("file field in a label container", Root).Id.should_be("containerLabeledFileFieldId");
                };
            };
            it["finds text field by placeholder"] = () =>
            {
                driver.FindField("text input field with a placeholder", Root).Id.should_be("textInputFieldWithPlaceholder");
                driver.FindField("textarea field with a placeholder", Root).Id.should_be("textareaFieldWithPlaceholder");
            };
            describe["finding by id"] = () =>
            {
                it["finds field"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldId", Root).Value.should_be("text input field two val");
                };
                it["finds textarea"] = () =>
                {
                    driver.FindField("containerLabeledTextareaFieldId", Root).Value.should_be("textarea field two val");
                };
                it["finds select"] = () =>
                {
                    driver.FindField("containerLabeledSelectFieldId", Root).Name.should_be("containerLabeledSelectFieldName");
                };
                it["finds checkbox"] = () =>
                {
                    driver.FindField("containerLabeledCheckboxFieldId", Root).Value.should_be("checkbox field two val");
                };
                it["finds radio"] = () =>
                {
                    driver.FindField("containerLabeledRadioFieldId", Root).Value.should_be("radio field two val");
                };
                it["finds password"] = () =>
                {
                    driver.FindField("containerLabeledPasswordFieldId", Root).Name.should_be("containerLabeledPasswordFieldName");
                };
                it["finds file"] = () =>
                {
                    driver.FindField("containerLabeledFileFieldId", Root).Name.should_be("containerLabeledFileFieldName");
                };
                
                it["finds by id ends with"] = () =>
                {
                    driver.FindField("tainerLabeledFileFieldId", Root).Name.should_be("containerLabeledFileFieldName");
                };
            };
            describe["finding by name"] = () =>
            {
                it["finds text input"] = () =>
                {
                    driver.FindField("containerLabeledTextInputFieldName", Root).Value.should_be("text input field two val");
                };
                it["finds textarea"] = () =>
                {
                    driver.FindField("containerLabeledTextareaFieldName", Root).Value.should_be("textarea field two val");
                };
                it["finds select"] = () =>
                {
                    driver.FindField("containerLabeledSelectFieldName", Root).Id.should_be("containerLabeledSelectFieldId");
                };
                it["finds checkbox"] = () =>
                {
                    driver.FindField("containerLabeledCheckboxFieldName", Root).Value.should_be("checkbox field two val");
                };
                it["finds radio button"] = () =>
                {
                    driver.FindField("containerLabeledRadioFieldName", Root).Value.should_be("radio field two val");
                };
                it["finds password input"] = () =>
                {
                    driver.FindField("containerLabeledPasswordFieldName", Root).Id.should_be("containerLabeledPasswordFieldId");
                };
                it["finds file input"] = () =>
                {
                    driver.FindField("containerLabeledFileFieldName", Root).Id.should_be("containerLabeledFileFieldId");
                };
            };
            it["finds radio button by value"] = () =>
            {
                driver.FindField("radio field one val", Root).Name.should_be("forLabeledRadioFieldName");
                driver.FindField("radio field two val", Root).Name.should_be("containerLabeledRadioFieldName");
            };
        }
    }
}