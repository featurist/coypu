using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class CheckboxExamples : WaitAndRetryExamples
    {
        [Test]
        public void Can_find_checkbox_and_check_it()
        {
            var checkbox = Browser.FindCss("#uncheckedBox");
            checkbox.Check();
            Assert.IsTrue(Browser.FindField("uncheckedBox")
                                 .Selected);
        }

        [Test]
        public void Can_find_checkbox_and_uncheck_it()
        {
            var checkbox = Browser.FindCss("#checkedBox");
            checkbox.Uncheck();
            Assert.IsFalse(Browser.Query(() => Browser.FindField("checkedBox")
                                                      .Selected,
                                         false));
        }

        [Test]
        public void Check_example()
        {
            Browser.Check("uncheckedBox");
            Assert.IsTrue(Browser.FindField("uncheckedBox")
                                 .Selected);
        }

        [Test]
        public void Uncheck_example()
        {
            Browser.Uncheck("checkedBox");
            Assert.IsFalse(Browser.Query(() => Browser.FindField("checkedBox")
                                                      .Selected,
                                         false));
        }
    }
}