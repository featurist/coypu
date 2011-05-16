using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_checking : DriverSpecs
	{
		internal override void Specs()
		{
			it["checks an unchecked checkbox"] = () =>
			{
				var checkbox = driver.FindField("uncheckedBox");
				checkbox.Selected.should_be_false();
	
				driver.Check(checkbox);
	
				var findAgain = driver.FindField("uncheckedBox");
				findAgain.Selected.should_be_true();
			};

			it["leaves a checked checkbox checked"] = () =>
			{
				var checkbox = driver.FindField("checkedBox");
				checkbox.Selected.should_be_true();

				driver.Check(checkbox);

				var findAgain = driver.FindField("checkedBox");
				findAgain.Selected.should_be_true();
			};

			it["unchecks a checked checkbox"] = () =>
			{
				var checkbox = driver.FindField("checkedBox");
				checkbox.Selected.should_be_true();

				driver.Uncheck(checkbox);

				var findAgain = driver.FindField("checkedBox");
				findAgain.Selected.should_be_false();
			};

			it["leaves an unchecked checkbox unchecked"] = () =>
			{
				var checkbox = driver.FindField("uncheckedBox");
				checkbox.Selected.should_be_false();

				driver.Uncheck(checkbox);

				var findAgain = driver.FindField("uncheckedBox");
				findAgain.Selected.should_be_false();
			};

			it["fires onclick event"] = () =>
			{
				var checkbox = driver.FindField("uncheckedBox");
				checkbox.Value.should_be("unchecked");

				driver.Check(checkbox);

				driver.FindField("uncheckedBox").Value.should_be("unchecked - clicked");
			};

			it["fires onclick event"] = () =>
			{
				var checkbox = driver.FindField("checkedBox");
				checkbox.Value.should_be("checked");

				driver.Uncheck(checkbox);

				driver.FindField("checkedBox").Value.should_be("checked - clicked");
			};
		}
	}
}