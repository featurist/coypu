using System.Threading;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class SelectFromExamples : WaitAndRetryExamples
    {
        [Test]
        public void SelectFrom_element()
        {
            var field = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(field.SelectedOption, Is.EqualTo("select two option one"));

            field.SelectOption("select two option two");
            field = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(field.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void SelectFrom()
        {
            var textField = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option one"));

            Browser.Select("select2value2")
                   .From("containerLabeledSelectFieldId");
            textField = Browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void SelectFromWithOptions()
        {
            var textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.SelectedOption, Is.EqualTo("option"));

            Browser.Select("value3")
                   .From("selectField", Options.First);
            textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.SelectedOption, Is.EqualTo("option option"));
        }

        [Test]
        public void SelectWithOptionsFromWithOptions()
        {
            var textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.SelectedOption, Is.EqualTo("option"));

            Browser.Select("four", Options.Single)
                   .From("selectField", Options.First);
            textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.Value, Is.EqualTo("value4"));
        }

        [Test]
        public void SelectFromOptionGroupWorkaround()
        {
            var textField = Browser.FindField("selectFieldWithOptionGroups");
            Assert.That(textField.SelectedOption, Is.EqualTo("Barbie"));

            var elem = Browser.FindId("Male");
            elem.SelectOption("value2");

            textField = Browser.FindField("selectFieldWithOptionGroups - changed");
            Assert.That(textField.SelectedOption, Is.EqualTo("Brendon"));
        }
    }
}