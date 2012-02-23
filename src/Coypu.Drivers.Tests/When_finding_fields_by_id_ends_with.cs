using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_id_ends_with : DriverSpecs
    {
        [Test]
        public void Finds_by_id_ends_with()
        {
            Driver.FindField("tainerLabeledFileFieldId", Root).Name.should_be("containerLabeledFileFieldName");
        }

        [Test]
        public void Finds_by_complete_id_before_finding_by_id_ends_with()
        {
            Driver.FindField("checkedBox",Root).Id.should_be("checkedBox");
        }
    }
}