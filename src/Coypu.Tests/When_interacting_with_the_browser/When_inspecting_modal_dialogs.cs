using System;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_modal_dialogs : When_inspecting
    {
        [Test]
        public void HasDialog_should_wait_for_robustly_Positive_example()
        {
            Queries_robustly(true, true, session.HasDialog, driver.StubDialog);
        }

        [Test]
        public void HasDialog_should_wait_for_robustly_Negative_example()
        {
            Queries_robustly(true, false, session.HasDialog, driver.StubDialog);
        }

        [Test]
        public void HasNoDialog_should_wait_for_robustly_Positive_example()
        {
            Queries_robustly(false, true, session.HasNoDialog, driver.StubDialog);
        }

        [Test]
        public void HasNoDialog_should_wait_for_robustly_Negative_example()
        {
            Queries_robustly(false, false, session.HasNoDialog, driver.StubDialog);
        }
    }
}