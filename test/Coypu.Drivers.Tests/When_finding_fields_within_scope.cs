using Coypu.Finders;
using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_fields_within_scope : DriverSpecs
    {
        private DriverScope scope1;
        private DriverScope scope2;
        
        public When_finding_fields_within_scope()
        {
            scope1 = new BrowserWindow(DefaultSessionConfiguration, new IdFinder(Driver, "scope1", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            scope2 = new BrowserWindow(DefaultSessionConfiguration, new IdFinder(Driver, "scope2", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
        }

        [Fact]
        public void Finds_text_input_by_for()
        {
            Field("scoped text input field linked by for", scope1).Id.ShouldBe("scope1TextInputFieldId");
            Field("scoped text input field linked by for", scope2).Id.ShouldBe("scope2TextInputFieldId");
        }

        [Fact]
        public void Finds_text_input_in_container_label()
        {
            Field("scoped text input field in a label container", scope1).Id.ShouldBe("scope1ContainerLabeledTextInputFieldId");
            Field("scoped text input field in a label container", scope2).Id.ShouldBe("scope2ContainerLabeledTextInputFieldId");
        }

        [Fact]
        public void Finds_text_input_by_placeholder()
        {
            Field("scoped text input field with a placeholder", scope1).Id.ShouldBe("scope1TextInputFieldWithPlaceholder");
            Field("scoped text input field with a placeholder", scope2).Id.ShouldBe("scope2TextInputFieldWithPlaceholder");
        }

        [Fact]
        public void Finds_text_input_by_name()
        {
            Field("text input field in a label container", scope1).Id.ShouldBe("scope1ContainerLabeledTextInputFieldId");
            Field("text input field in a label container", scope2).Id.ShouldBe("scope2ContainerLabeledTextInputFieldId");
        }

        [Fact]
        public void Finds_radio_button_by_value()
        {
            Field("scoped radio field one val", scope1).Id.ShouldBe("scope1RadioFieldId");
            Field("scoped radio field one val", scope2).Id.ShouldBe("scope2RadioFieldId");
        }

        [Fact]
        public void Finds_not_find_text_input_by_id_outside_scope()
        {
            Assert.Throws<MissingHtmlException>(() => Field("containerLabeledTextInputFieldId", scope1));
            Assert.Throws<MissingHtmlException>(() => Field("containerLabeledTextInputFieldId", scope2));
        }
    }

}