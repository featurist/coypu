using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class SelectFromExamples : WaitAndRetryExamples
    {
        [Test]
        public void SelectFrom_element_example()
        {
            var field = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(field.SelectedOption, Is.EqualTo("select two option one"));

            field.SelectOption("select two option two");
            field = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(field.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void SelectFrom_example()
        {
            var textField = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option one"));

            Browser.Select("select2value2")
                   .From("containerLabeledSelectFieldId");
            textField = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option two"));
        }
    }
}