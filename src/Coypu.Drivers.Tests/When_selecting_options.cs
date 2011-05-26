using System;
using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_selecting_options : DriverSpecs
    {
        internal override void Specs()
        {
            it["sets text of selected option"] = () =>
            {
                var textField = driver.FindField("containerLabeledSelectFieldId");
                textField.SelectedOption.should_be("select two option one");

                driver.Select(textField, "select2value2");

                textField = driver.FindField("containerLabeledSelectFieldId");
                textField.SelectedOption.should_be("select two option two");
            };
        }
    }
}