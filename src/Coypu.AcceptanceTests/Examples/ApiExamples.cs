using System;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests.Examples
{
    /// <summary>
    ///     Simple examples for each API method - to show usage and check everything is wired up properly
    /// </summary>
    [TestFixture]
    public class ApiExamples : WaitAndRetryExamples
    {
        public class CustomFirefoxOptionsSeleniumWebDriver : SeleniumWebDriver
        {
            public CustomFirefoxOptionsSeleniumWebDriver(Browser browser) : base(CustomOptions(), browser) { }

            private static IWebDriver CustomOptions()
            {
                return new FirefoxDriver(new FirefoxOptions());
            }
        }

        [Test]
        public void Attributes_on_stale_scope_example()
        {
            var field = Browser.FindField("find-this-field");
            Assert.That(field.Value, Is.EqualTo("This value is what we are looking for"));

            ReloadTestPage();
            Assert.That(field.Value, Is.EqualTo("This value is what we are looking for"));
            Assert.That(field.Id, Is.EqualTo("find-this-field"));
            Assert.That(field["id"], Is.EqualTo("find-this-field"));
        }

        [Test]
        public void Choose_example()
        {
            Browser.Choose("chooseRadio1");
            Assert.IsTrue(Browser.FindField("chooseRadio1")
                                 .Selected);

            Browser.Choose("chooseRadio2");
            Assert.IsTrue(Browser.FindField("chooseRadio2")
                                 .Selected);
            Assert.IsFalse(Browser.FindField("chooseRadio1")
                                  .Selected);
        }

        [Test]
        public void ConsideringInvisibleElements()
        {
            Browser.FindButton("firstInvisibleInputId", new Options {ConsiderInvisibleElements = true})
                   .Now();
        }

        [Test]
        public void ConsideringOnlyVisibleElements()
        {
            Assert.Throws<MissingHtmlException>(() => Browser.FindButton("firstInvisibleInputId")
                                                             .Now());
        }

        [Test]
        public void CustomOptions()
        {
            var configuration = new SessionConfiguration {Driver = typeof(CustomFirefoxOptionsSeleniumWebDriver)};
            using (var custom = new BrowserSession(configuration))
            {
                custom.Visit("https://www.relishapp.com/");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        [Test]
        public void DisabledButton_example()
        {
            Assert.That(Browser.FindButton("Disabled button")
                               .Disabled,
                        Is.True,
                        "Expected button to be disabled");
            Assert.That(Browser.FindButton("Click me")
                               .Disabled,
                        Is.False,
                        "Expected button to be enabled");
        }

        [Test]
        public void ExecuteScript_example()
        {
            ReloadTestPage();
            Assert.That(Browser.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;"),
                        Is.EqualTo("first button"));
        }

        [Test]
        public void ExecuteScriptWithArgs_example()
        {
            ReloadTestPage();
            Assert.That(Browser.ExecuteScript("return arguments[0].innerHTML;", Browser.FindId("firstButtonId")),
                        Is.EqualTo("first button"));
        }

        [Test]
        public void Hover_example()
        {
            Assert.That(Browser.FindId("hoverOnMeTest")
                               .Text,
                        Is.EqualTo("Hover on me"));
            Browser.FindId("hoverOnMeTest")
                   .Hover();
            Assert.That(Browser.FindId("hoverOnMeTest")
                               .Text,
                        Is.EqualTo("Hover on me - hovered"));
        }

        [Test]
        public void Multiple_interactions_within_iframe_example()
        {
            var iframe = Browser.FindFrame("I am iframe one");
            iframe.FillIn("text input in iframe")
                  .With("filled in");
            Assert.That(iframe.FindField("text input in iframe")
                              .Value,
                        Is.EqualTo("filled in"));
        }

        [Test]
        public void Native_example()
        {
            var button = (IWebElement) Browser.FindButton("clickMeTest")
                                              .Native;
            button.Click();
            Assert.That(Browser.FindButton("clickMeTest")
                               .Value,
                        Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void Title_example()
        {
            Assert.That(Browser.Title, Is.EqualTo("Coypu interaction tests page"));
        }

        [Test]
        public void TryUntil_example()
        {
            var tryThisButton = Browser.FindButton("try this");
            Assert.That(tryThisButton.Exists());
            Browser.TryUntil(() => tryThisButton.Click(),
                             () => Browser.HasContent("try until 5"),
                             TimeSpan.FromMilliseconds(50),
                             new Options {Timeout = TimeSpan.FromMilliseconds(10000)});
        }
    }
}