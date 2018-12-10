using System;
using System.Diagnostics;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class ClickExamples : WaitAndRetryExamples
    {
        private static readonly TimeSpan WaitBeforeClickInSec = TimeSpan.FromSeconds(1);

        private readonly Options _optionsWaitBeforeClick = new Options {WaitBeforeClick = WaitBeforeClickInSec};

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
        public void ClickButton_WaitBeforeClick()
        {
            var stopWatch = Stopwatch.StartNew();

            Browser.ClickButton("clickMeTest", _optionsWaitBeforeClick);
            var actualWait = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"\t-> Actual wait before click {actualWait} milliseconds");

            Assert.That(actualWait > WaitBeforeClickInSec.TotalMilliseconds, "\tDidn't wait enough!");
        }

        [Test]
        public void ClickLink()
        {
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
        }

        [Test]
        public void ClickLink_WaitBeforeClick()
        {
            var stopWatch = Stopwatch.StartNew();

            Browser.ClickLink("Trigger a confirm", _optionsWaitBeforeClick);
            var actualWait = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"\t-> Actual wait before click {actualWait} milliseconds");
            Browser.CancelModalDialog();

            Assert.That(actualWait > WaitBeforeClickInSec.TotalMilliseconds, "\tDidn't wait enough!");
        }

        [Test]
        public void ClickLink_WithTitle()
        {
            Browser.ClickLink("Link with title");
            Browser.CancelModalDialog();
        }

        [Test]
        public void Click_WaitBeforeClick()
        {
            var stopWatch = Stopwatch.StartNew();

            Browser.FindButton("clickMeTest")
                   .Click(_optionsWaitBeforeClick);
            var actualWait = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"\t-> Actual wait before click {actualWait} milliseconds");

            Assert.That(actualWait > WaitBeforeClickInSec.TotalMilliseconds, "\tDidn't wait enough!");
        }
    }
}