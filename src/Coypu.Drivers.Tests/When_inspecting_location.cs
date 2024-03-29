﻿using System;
using System.Threading;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Timing;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_navigating : DriverSpecs
    {
        [Test]
        public void Gets_the_current_browser_location()
        {
            Driver.Visit(TestSiteUrl("/"), Root);
            Assert.That(Driver.Location(Root), Is.EqualTo(new Uri(TestSiteUrl("/"))));

            Driver.Visit(TestSiteUrl("/auto_login"), Root);
            Assert.That(Driver.Location(Root), Is.EqualTo(new Uri(TestSiteUrl("/auto_login"))));
        }

        [Test]
        public void Gets_location_for_correct_window_scope()
        {
            Driver.Click(Link("Open pop up window"));
            var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver, "Pop Up Window", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            RetryUntilTimeoutTimingStrategy.Retry(() => popUp.Now());
            Assert.That(Driver.Location(popUp)
                              .AbsoluteUri,
                        Does.EndWith("src/Coypu.Drivers.Tests/bin/Debug/net6.0/html/popup.htm"));
        }

        [Test]
        public void Not_just_when_set_by_visit()
        {
            Driver.Visit(TestSiteUrl("/auto_login"), Root);
            Driver.ExecuteScript("document.location.href = '" + TestSiteUrl("/resource/bdd") + "'", Root);

            // Seems like WebDriver is not waiting on JS, has exec been made asnyc?
            Thread.Sleep(500);

            Assert.That(Driver.Location(Root), Is.EqualTo(new Uri(TestSiteUrl("/resource/bdd"))));
        }
    }
}
