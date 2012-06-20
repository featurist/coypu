using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_id_ends_with : DriverSpecs
    {
        [Test]
        public void Finds_by_id_ends_with()
        {
            Driver.FindField("aspWebFormsContainerLabeledFileFieldId", Root).Id.should_be("_ctrl01_ctrl02_aspWebFormsContainerLabeledFileFieldId");
        }

        [Test]
        public void Finds_by_complete_id_before_finding_by_id_ends_with()
        {
            Driver.FindField("checkedBox",Root).Id.should_be("checkedBox");
        }
    }
}