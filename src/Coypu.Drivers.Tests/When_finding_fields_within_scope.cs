using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_within_scope : DriverSpecs
    {
        private DriverScope scope1;
        private DriverScope scope2;

        [SetUp]
        public void SetUpScope()
        {
            scope1 = new DriverScope(new SessionConfiguration(),new IdFinder(Driver, "scope1", Root), Driver,null,null,null);
            scope2 = new DriverScope(new SessionConfiguration(), new IdFinder(Driver, "scope2", Root), Driver, null, null, null);
        }

        [Test]
        public void Finds_text_input_by_for()
        {
            Driver.FindField("scoped text input field linked by for", scope1).Id.should_be("scope1TextInputFieldId");
            Driver.FindField("scoped text input field linked by for", scope2).Id.should_be("scope2TextInputFieldId");
        }

        [Test]
        public void Finds_text_input_in_container_label()
        {
            Driver.FindField("scoped text input field in a label container", scope1).Id.should_be("scope1ContainerLabeledTextInputFieldId");
            Driver.FindField("scoped text input field in a label container", scope2).Id.should_be("scope2ContainerLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_text_input_by_placeholder()
        {
            Driver.FindField("scoped text input field with a placeholder", scope1).Id.should_be("scope1TextInputFieldWithPlaceholder");
            Driver.FindField("scoped text input field with a placeholder", scope2).Id.should_be("scope2TextInputFieldWithPlaceholder");
        }

        [Test]
        public void Finds_text_input_by_name()
        {
            Driver.FindField("containerLabeledTextInputFieldName", scope1).Id.should_be("scope1ContainerLabeledTextInputFieldId");
            Driver.FindField("containerLabeledTextInputFieldName", scope2).Id.should_be("scope2ContainerLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_radio_button_by_value()
        {
            Driver.FindField("scoped radio field one val", scope1).Id.should_be("scope1RadioFieldId");
            Driver.FindField("scoped radio field one val", scope2).Id.should_be("scope2RadioFieldId");
        }

        [Test]
        public void Finds_not_find_text_input_by_id_outside_scope()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindField("containerLabeledTextInputFieldId", scope1));
            Assert.Throws<MissingHtmlException>(() => Driver.FindField("containerLabeledTextInputFieldId", scope2));
        }
    }

}