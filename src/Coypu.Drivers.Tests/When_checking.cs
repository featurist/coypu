using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_checking : DriverSpecs
    {
        [Test]
        public void Checks_an_unchecked_checkbox()
        {
            var checkbox = Field("uncheckedBox"); 
            checkbox.Selected.ShouldBeFalse();

            Driver.Check(checkbox);

            var findAgain = Field("uncheckedBox");
            findAgain.Selected.ShouldBeTrue();
        }


        [Test]
        public void Leaves_a_checked_checkbox_checked()
        {
            var checkbox = Field("checkedBox");
            checkbox.Selected.ShouldBeTrue();

            Driver.Check(checkbox);

            var findAgain = Field("checkedBox");
            findAgain.Selected.ShouldBeTrue();
        }


        [Test]
        public void Unchecks_a_checked_checkbox()
        {
            var checkbox = Field("checkedBox");
            checkbox.Selected.ShouldBeTrue();

            Driver.Uncheck(checkbox);

            var findAgain = Field("checkedBox");
            findAgain.Selected.ShouldBeFalse();
        }


        [Test]
        public void Leaves_an_unchecked_checkbox_unchecked()
        {
            var checkbox = Field("uncheckedBox");
            checkbox.Selected.ShouldBeFalse();

            Driver.Uncheck(checkbox);

            var findAgain = Field("uncheckedBox");
            findAgain.Selected.ShouldBeFalse();
        }


        [Test]
        public void Fires_onclick_event_on_check()
        {
            var checkbox = Field("uncheckedBox");
            checkbox.Value.ShouldBe("unchecked");

            Driver.Check(checkbox);

            Field("uncheckedBox", Root).Value.ShouldBe("unchecked - clicked");
        }


        [Test]
        public void Fires_onclick_event_on_uncheck()
        {
            var checkbox = Field("checkedBox");
            checkbox.Value.ShouldBe("checked");

            Driver.Uncheck(checkbox);

            Field("checkedBox", Root).Value.ShouldBe("checked - clicked");
        }
    }
}