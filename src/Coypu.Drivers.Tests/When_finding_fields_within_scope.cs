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
            scope1 = new DriverScope(DefaultSessionConfiguration, new IdFinder(Driver, "scope1", Root, DefaultOptions), Driver, null, null, null);
            scope2 = new DriverScope(DefaultSessionConfiguration, new IdFinder(Driver, "scope2", Root, DefaultOptions), Driver, null, null, null);
        }

        [Test]
        public void Finds_text_input_by_for()
        {
            Field("scoped text input field linked by for", scope1).Id.should_be("scope1TextInputFieldId");
            Field("scoped text input field linked by for", scope2).Id.should_be("scope2TextInputFieldId");
        }

        [Test]
        public void Finds_text_input_in_container_label()
        {
            Field("scoped text input field in a label container", scope1).Id.should_be("scope1ContainerLabeledTextInputFieldId");
            Field("scoped text input field in a label container", scope2).Id.should_be("scope2ContainerLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_text_input_by_placeholder()
        {
            Field("scoped text input field with a placeholder", scope1).Id.should_be("scope1TextInputFieldWithPlaceholder");
            Field("scoped text input field with a placeholder", scope2).Id.should_be("scope2TextInputFieldWithPlaceholder");
        }

        [Test]
        public void Finds_text_input_by_name()
        {
            Field("text input field in a label container", scope1).Id.should_be("scope1ContainerLabeledTextInputFieldId");
            Field("text input field in a label container", scope2).Id.should_be("scope2ContainerLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_radio_button_by_value()
        {
            Field("scoped radio field one val", scope1).Id.should_be("scope1RadioFieldId");
            Field("scoped radio field one val", scope2).Id.should_be("scope2RadioFieldId");
        }

        [Test]
        public void Finds_not_find_text_input_by_id_outside_scope()
        {
            Assert.Throws<MissingHtmlException>(() => Field("containerLabeledTextInputFieldId", scope1));
            Assert.Throws<MissingHtmlException>(() => Field("containerLabeledTextInputFieldId", scope2));
        }
    }

}