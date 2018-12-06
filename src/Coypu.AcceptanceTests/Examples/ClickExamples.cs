using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class ClickExamples : WaitAndRetryExamples
    {
        [Test]
        public void Click()
        {
            var element = Browser.FindButton("clickMeTest");
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me"));

            element.Click();
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickButton()
        {
            Browser.ClickButton("clickMeTest");
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickButtonWaitBeforeClick()
        {
            var stopWatch = Stopwatch.StartNew();
            Browser.ClickButton("clickMeTest", new Options {WaitBeforeClick = TimeSpan.FromMilliseconds(2000)});
            var actualWait = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Actual wait before click {actualWait} milliseconds");
            Assert.That(actualWait > 2000, "\tDidn't wait enough!");
        }

        [Test]
        public void ClickLink()
        {
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
        }

        [Test]
        public void ClickLinkWithTitle()
        {
            Browser.ClickLink("Link with title");
            Browser.CancelModalDialog();
        }

        [Test]
        public void ClickWaitBeforeClick()
        {
            var stopWatch = Stopwatch.StartNew();
            var element = Browser.FindButton("clickMeTest");
            element.Click(new Options {WaitBeforeClick = TimeSpan.FromMilliseconds(2000)});
            var actualWait = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Actual wait before click {actualWait} milliseconds");
            Assert.That(actualWait > 2000, "\tDidn't wait enough!");


            //var waiter = new StopwatchWaiter();
            //var stopWatch = Stopwatch.StartNew();
            //var expectedDuration = TimeSpan.FromMilliseconds(expectedDurationMilliseconds);

            //waiter.Wait(expectedDuration);

            //var actualWait = stopWatch.ElapsedMilliseconds;

            //const int toleranceMilliseconds = AccuracyMilliseconds;

            //Assert.That((int)actualWait, Is.InRange(expectedDurationMilliseconds - toleranceMilliseconds,
            //                                        expectedDurationMilliseconds + toleranceMilliseconds));
        }
    }
}