using System;
using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_checking : DriverSpecs
	{
		internal override Action Specs()
		{
			return () =>
			{
				it["should check an unchecked checkbox"] = () =>
				{
					var checkbox = driver.FindField("uncheckedBox");
					checkbox.Selected.should_be_false();
		
					driver.Check(checkbox);
		
					var findAgain = driver.FindField("uncheckedBox");
					findAgain.Selected.should_be_true();
				};

				it["should leave a checked checkbox checked"] = () =>
				{
					var checkbox = driver.FindField("checkedBox");
					checkbox.Selected.should_be_true();

					driver.Check(checkbox);

					var findAgain = driver.FindField("checkedBox");
					findAgain.Selected.should_be_true();
				};

				it["should uncheck a checked checkbox"] = () =>
				{
					var checkbox = driver.FindField("checkedBox");
					checkbox.Selected.should_be_true();

					driver.Uncheck(checkbox);

					var findAgain = driver.FindField("checkedBox");
					findAgain.Selected.should_be_false();
				};

				it["should leave an unchecked checkbox unchecked"] = () =>
				{
					var checkbox = driver.FindField("uncheckedBox");
					checkbox.Selected.should_be_false();

					driver.Uncheck(checkbox);

					var findAgain = driver.FindField("uncheckedBox");
					findAgain.Selected.should_be_false();
				};

				it["should fire onclick event"] = () =>
				{
					var checkbox = driver.FindField("uncheckedBox");
					checkbox.Value.should_be("unchecked");

					driver.Check(checkbox);

					driver.FindField("uncheckedBox").Value.should_be("unchecked - clicked");
				};

				it["should fire onclick event"] = () =>
				{
					var checkbox = driver.FindField("checkedBox");
					checkbox.Value.should_be("checked");

					driver.Uncheck(checkbox);

					driver.FindField("checkedBox").Value.should_be("checked - clicked");
				};
			};
		   
		}
	}
}