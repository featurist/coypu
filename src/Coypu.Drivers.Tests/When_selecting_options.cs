using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	internal class When_selecting_options : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () =>
			{
				it["should set text of selected option"] = () =>
				{
					var textField = driver().FindField("containerLabeledSelectFieldId");
					textField.SelectedOption.should_be("select two option one");

					driver().Select(textField, "select2value2");

					textField = driver().FindField("containerLabeledSelectFieldId");
					textField.SelectedOption.should_be("select two option two");
				};

			};
		}
	}
}