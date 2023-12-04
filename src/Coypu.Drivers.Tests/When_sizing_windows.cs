using System;
using System.Drawing;
using Coypu.Actions;
using Coypu.Drivers.Playwright;
using Coypu.Finders;
using Coypu.Timing;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_sizing_windows : DriverSpecs
    {
        [Test]
        public void MaximisesWindow()
        {
            using (Driver)
            {
                AssertMaximisesWindow(Root);
            }
        }

        [Test]
        public void MaximisesCorrectWindowScope()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                            Driver, null, null, null, DisambiguationStrategy);
                Retry(() => popUp.Now());
                try
                {
                    AssertMaximisesWindow(popUp);
                }
                finally
                {
                    Driver.ExecuteScript("window.setTimeout(() => self.close(), 1);", popUp);
                }
            }
        }

        private static void AssertMaximisesWindow(DriverScope driverScope)
        {
            if (Driver is PlaywrightDriver)
            {
                Assert.Ignore("Playwright does not support window maximisation");
            }
            Driver.ResizeTo(new Size(768, 400), driverScope);
            Driver.MaximiseWindow(driverScope);

            Assert.That( Driver.ExecuteScript("return window.outerWidth;", driverScope), Is.GreaterThan(768));
            Assert.That( Driver.ExecuteScript("return window.outerHeight;", driverScope), Is.GreaterThan(400));
        }

        [Test]
        public void ResizesWindow()
        {
            using (Driver)
            {
                AssertResizesWindow(Root);
            }
        }

        [Test]
        public void ResizesCorrectWindowScope()
        {
            using (Driver)
            {
                Driver.Click(Link("Open pop up window"));
                var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions),
                                              Driver, null, null, null, DisambiguationStrategy);
                Retry(() => popUp.Now());
                try
                {
                    AssertResizesWindow(popUp);
                }
                finally
                {
                    Driver.ExecuteScript("window.setTimeout(() => self.close(), 1);", popUp);
                }
            }
        }

        private static void AssertResizesWindow(DriverScope driverScope)
        {
            var initalWidth = Driver.ExecuteScript("return window.outerWidth;", driverScope).ToString();
            var initalHeight = Driver.ExecuteScript("return window.outerHeight;", driverScope).ToString();

            Assert.That(int.Parse(initalWidth), Is.Not.EqualTo(768));
            Assert.That(int.Parse(initalHeight), Is.Not.EqualTo(400));

            Driver.ResizeTo(new Size(768, 400), driverScope);

            Assert.That(Driver.ExecuteScript("return window.outerWidth;", driverScope).ToString(), Is.EqualTo("768"));
            Assert.That(Driver.ExecuteScript("return window.outerHeight;", driverScope).ToString(), Is.EqualTo("400"));
        }

        private void Retry(Scope popUp)
        {
            Retry(() => popUp.Now());
        }

        private void Retry(Action popUpAction)
        {
            new RetryUntilTimeoutTimingStrategy().Synchronise(
                new LambdaBrowserAction(popUpAction, DefaultOptions)
            );
        }
    }
}
