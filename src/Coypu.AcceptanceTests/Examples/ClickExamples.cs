using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.DevTools.V85.Network;

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
        public void DblClick()
        {
            var element = Browser.FindButton("dblClickMeTest");
            Assert.That(Browser.FindButton("dblClickMeTest")
                               .Value,
                        Is.EqualTo("Double Click me"));

            element.DblClick();
            Assert.That(Browser.FindButton("dblClickMeTest")
                               .Value,
                        Is.EqualTo("Double Click me - dblclicked"));
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
            Browser.CancelConfirm(() => {
                Browser.ClickLink("Trigger a confirm");
            });
        }

        [Test]
        public void ClickLink_WaitBeforeClick()
        {
            var stopWatch = Stopwatch.StartNew();
            long actualWait = 0;
            Browser.CancelConfirm(() => {
              Browser.ClickLink("Trigger a confirm", _optionsWaitBeforeClick);
              actualWait = stopWatch.ElapsedMilliseconds;
              Console.WriteLine($"\t-> Actual wait before click {actualWait} milliseconds");
            });

            Assert.That(actualWait > WaitBeforeClickInSec.TotalMilliseconds, "\tDidn't wait enough!");
        }

        [Test]
        public void ClickLink_WithTitle()
        {
            Browser.CancelConfirm(() => {
              Browser.ClickLink("Link with title");
            });
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
