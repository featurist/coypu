using System.Threading.Tasks.Dataflow;
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
        public void SelectFrom_WithOptions()
        {
            var textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.SelectedOption, Is.EqualTo("option"));

            Browser.Select("value3")
                   .From("selectField", Options.First);
            textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.SelectedOption, Is.EqualTo("option option"));
        }

        [Test]
        public void Select_WithOptions_From_WithOptions()
        {
            var textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.SelectedOption, Is.EqualTo("option"));

            Browser.Select("four", Options.Single)
                   .From("selectField", Options.First);
            textField = Browser.FindField("selectField", Options.First);
            Assert.That(textField.Value, Is.EqualTo("value4"));
        }

        [Test]
        public void SelectFrom_OptionGroup_Workaround()
        {
            var select = Browser.FindField("selectFieldWithOptionGroups");
            Assert.That(select.SelectedOption, Is.EqualTo("Barbie"));

            var group = Browser.FindId("Male", Options.Invisible);
            group.SelectOption("value2", Options.Invisible);

            select = Browser.FindField("selectFieldWithOptionGroups - changed");
            Assert.That(select.SelectedOption, Is.EqualTo("Brendon"));
        }
    }
}
