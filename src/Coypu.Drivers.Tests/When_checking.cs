using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_checking : DriverSpecs
    {
        [Test]
        public void Checks_an_unchecked_checkbox()
        {
            var checkbox = Driver.FindField("uncheckedBox", Root);
            checkbox.Selected.should_be_false();

            Driver.Check(checkbox);

            var findAgain = Driver.FindField("uncheckedBox", Root);
            findAgain.Selected.should_be_true();
        }


        [Test]
        public void Leaves_a_checked_checkbox_checked()
        {
            var checkbox = Driver.FindField("checkedBox", Root);
            checkbox.Selected.should_be_true();

            Driver.Check(checkbox);

            var findAgain = Driver.FindField("checkedBox", Root);
            findAgain.Selected.should_be_true();
        }


        [Test]
        public void Unchecks_a_checked_checkbox()
        {
            var checkbox = Driver.FindField("checkedBox", Root);
            checkbox.Selected.should_be_true();

            Driver.Uncheck(checkbox);

            var findAgain = Driver.FindField("checkedBox", Root);
            findAgain.Selected.should_be_false();
        }


        [Test]
        public void Leaves_an_unchecked_checkbox_unchecked()
        {
            var checkbox = Driver.FindField("uncheckedBox", Root);
            checkbox.Selected.should_be_false();

            Driver.Uncheck(checkbox);

            var findAgain = Driver.FindField("uncheckedBox", Root);
            findAgain.Selected.should_be_false();
        }


        [Test]
        public void Fires_onclick_event_on_check()
        {
            var checkbox = Driver.FindField("uncheckedBox", Root);
            checkbox.Value.should_be("unchecked");

            Driver.Check(checkbox);

            Driver.FindField("uncheckedBox", Root).Value.should_be("unchecked - clicked");
        }


        [Test]
        public void Fires_onclick_event_on_uncheck()
        {
            var checkbox = Driver.FindField("checkedBox", Root);
            checkbox.Value.should_be("checked");

            Driver.Uncheck(checkbox);

            Driver.FindField("checkedBox", Root).Value.should_be("checked - clicked");
        }
    }
}