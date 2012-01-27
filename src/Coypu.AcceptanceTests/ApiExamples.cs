using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests
{
    /// <summary>
    /// Simple examples for each API method - to show usage and check everything is wired up properly
    /// </summary>
    [TestFixture]
    public class Examples
    {
        private Session browser
        {
            get { return Browser.Session; }
        }

        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(2000);
            ReloadTestPageWithDelay();
        }

        private void ApplyAsyncDelay()
        {
            // Hide the HTML then bring back after a short delay to test robustness
            browser.ExecuteScript("window.holdIt = document.body.innerHTML;document.body.innerHTML = ''");
            browser.ExecuteScript("setTimeout(function() {document.body.innerHTML = window.holdIt},250)");
        }

        private void ReloadTestPage()
        {
            browser.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }

        private void ReloadTestPageWithDelay()
        {
            ReloadTestPage();
            ApplyAsyncDelay();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Browser.EndSession();
        }

        [Test]
        public void AcceptModalDialog_example()
        {
            browser.ClickLink("Trigger an alert");
            Assert.IsTrue(browser.HasDialog("You have triggered an alert and this is the text."));

            browser.AcceptModalDialog();
            Assert.IsTrue(browser.HasNoDialog("You have triggered an alert and this is the text."));
        }

        [Test]
        public void CancelModalDialog_example()
        {
            browser.ClickLink("Trigger a confirm");
            browser.CancelModalDialog();
            browser.FindLink("Trigger a confirm - cancelled");
        }

        [Test]
        public void Check_example()
        {
            browser.Check("uncheckedBox");
            Assert.IsTrue(browser.FindField("uncheckedBox").Selected);
        }
        
        [Test]
        public void Uncheck_example()
        {
            browser.Uncheck("checkedBox");
            Assert.IsFalse(browser.FindField("checkedBox").Selected);
        }

        [Test]
        public void Choose_example()
        {
            browser.Choose("chooseRadio1");

            Assert.IsTrue(browser.FindField("chooseRadio1").Selected);

            browser.Choose("chooseRadio2");

            Assert.IsTrue(browser.FindField("chooseRadio2").Selected);
            Assert.IsFalse(browser.FindField("chooseRadio1").Selected);
        }

        [Test]
        public void Click_example()
        {
            var element = browser.FindButton("clickMeTest");
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me"));

            browser.Click(element);
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void Click_with_finder_example()
        {
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me"));

            browser.Click(() => browser.FindButton("clickMeTest"));
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickButton_example()
        {
            browser.ClickButton("clickMeTest");
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }

        [Test]
        public void ClickLink_example()
        {
            browser.ClickLink("Trigger a confirm");
            browser.CancelModalDialog();
        }

        [Test]
        public void ExecuteScript_example()
        {
            ReloadTestPage();
            Assert.That(browser.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;"),
                        Is.EqualTo("first button"));
        }

        [Test]
        public void FillInWith_example()
        {
            browser.FillIn("containerLabeledTextInputFieldName").With("New text input value");
            Assert.That(browser.FindField("containerLabeledTextInputFieldName").Value, Is.EqualTo("New text input value"));
        }

        [Test]
        public void FindAllCss_example()
        {
            ReloadTestPage();

            const string shouldFind = "#inspectingContent ul#cssTest li";
            var all = browser.FindAllCss(shouldFind);
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1).Text, Is.EqualTo("two"));
            Assert.That(all.ElementAt(2).Text, Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindAllXPath_example()
        {
            ReloadTestPage();

            const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
            var all = browser.FindAllXPath(shouldFind);
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1).Text, Is.EqualTo("two"));
            Assert.That(all.ElementAt(2).Text, Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindButton_example()
        {
            Assert.That(browser.FindButton("Click me").Id, Is.EqualTo("clickMeTest"));
        }

        [Test]
        public void FindCss_example()
        {
            var first = browser.FindCss("#inspectingContent ul#cssTest li");
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindXPath_example()
        {
            var first = browser.FindXPath("//*[@id='inspectingContent']//ul[@id='cssTest']/li");
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindField_examples()
        {
            Assert.That(browser.FindField("text input field linked by for").Id, Is.EqualTo("forLabeledTextInputFieldId"));
            Assert.That(browser.FindField("checkbox field in a label container").Id, Is.EqualTo("containerLabeledCheckboxFieldId"));
            Assert.That(browser.FindField("containerLabeledSelectFieldId").Name, Is.EqualTo("containerLabeledSelectFieldName"));
            Assert.That(browser.FindField("containerLabeledPasswordFieldName").Id, Is.EqualTo("containerLabeledPasswordFieldId"));
        }

        [Test]
        public void FindFieldset_example()
        {
            Assert.That(browser.FindFieldset("Scope 1").Id, Is.EqualTo("fieldsetScope1"));
        }

        [Test]
        public void FindId_example()
        {
            Assert.That(browser.FindId("containerLabeledSelectFieldId").Name, Is.EqualTo("containerLabeledSelectFieldName"));
        }

        [Test]
        public void FindLink_example()
        {
            Assert.That(browser.FindLink("Trigger an alert").Id, Is.EqualTo("alertTriggerLink"));
        }

        [Test]
        public void FindSection_example()
        {
            Assert.That(browser.FindSection("Inspecting Content").Id, Is.EqualTo("inspectingContent"));
            Assert.That(browser.FindSection("Div Section Two h2 with link").Id, Is.EqualTo("divSectionTwoWithLink"));
        }

        [Test]
        public void SelectFrom_example()
        {
            var textField = browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option one"));

            browser.Select("select2value2").From("containerLabeledSelectFieldId");

            textField = browser.FindField("containerLabeledSelectFieldId");
            Assert.That(textField.SelectedOption, Is.EqualTo("select two option two"));
        }

        [Test]
        public void Has_example()
        {
            Assert.IsTrue(browser.Has(() => browser.FindSection("Inspecting Content")));
            Assert.IsFalse(browser.Has(() => browser.FindCss("#no-such-element")));
        }

        [Test]
        public void HasNo_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"no-such-element\">asdf</div>'");
            Assert.IsTrue(browser.HasNo(() => browser.FindCss("#no-such-element")));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNo(() => browser.FindSection("Inspecting Content")));
        }

        [Test]
        public void HasContent_example()
        {
            Assert.IsTrue(browser.HasContent("This is what we are looking for"));
            Assert.IsFalse(browser.HasContent("This is not in the page"));
        }

        [Test]
        public void HasNoContent_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.IsTrue(browser.HasNoContent("This is not in the page"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoContent("This is what we are looking for"));
        }

        [Test]
        public void HasContentMatch_example()
        {
            Assert.IsTrue(browser.HasContentMatch(new Regex("This is what (we are|I am) looking for")));
            Assert.IsFalse(browser.HasContentMatch(new Regex("This is ?n[o|']t in the page")));
        }

        [Test]
        public void HasNoContentMatch_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"no-such-element\">This is not in the page</div>'");
            Assert.IsTrue(browser.HasNoContentMatch(new Regex("This is ?n[o|']t in the page")));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoContentMatch(new Regex("This is what (we are|I am) looking for")));
        }

        [Test]
        public void HasCss_example()
        {
            Assert.IsTrue(browser.HasCss("#inspectingContent ul#cssTest"));
            Assert.IsFalse(browser.HasCss("#inspectingContent ul#nope"));
        }

        [Test]
        public void HasNoCss_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"inspectingContent\"><ul id=\"nope\"><li>This is not in the page</li></ul></div>'");
            Assert.IsTrue(browser.HasNoCss("#inspectingContent ul#nope"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoCss("#inspectingContent ul#cssTest"));
        }

        [Test]
        public void HasXPath_example()
        {
            Assert.IsTrue(browser.HasXPath("//*[@id='inspectingContent']//ul[@id='cssTest']"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasXPath("//*[@id='inspectingContent']//ul[@id='nope']"));
        }

        [Test]
        public void HasNoXpath_example()
        {
            browser.ExecuteScript("document.body.innerHTML = '<div id=\"inspectingContent\"><ul id=\"nope\"><li>This is not in the page</li></ul></div>'");
            Assert.IsTrue(browser.HasNoXPath("//*[@id='inspectingContent']//ul[@id='nope']"));

            ReloadTestPage();
            Assert.IsFalse(browser.HasNoXPath("//*[@id='inspectingContent']//ul[@id='cssTest']"));
        }


        [Test]
        public void Hover_example()
        {
            Assert.That(browser.FindId("hoverOnMeTest").Text, Is.EqualTo("Hover on me"));
            browser.Hover(() => browser.FindId("hoverOnMeTest"));
            Assert.That(browser.FindId("hoverOnMeTest").Text, Is.EqualTo("Hover on me - hovered"));
        }

        [Test]
        public void Native_example()
        {
            var button = (IWebElement) browser.FindButton("clickMeTest").Native;
            button.Click();
            Assert.That(browser.FindButton("clickMeTest").Value, Is.EqualTo("Click me - clicked"));
        }


        [Test]
        public void Within_example()
        {
            const string locatorThatAppearsInMultipleScopes = "scoped text input field linked by for";

            browser.Within(() => browser.FindId("scope1"),
                () => Assert.That(browser.FindField(locatorThatAppearsInMultipleScopes).Id, Is.EqualTo("scope1TextInputFieldId")));

            browser.Within(() => browser.FindId("scope2"),
                () => Assert.That(browser.FindField(locatorThatAppearsInMultipleScopes).Id, Is.EqualTo("scope2TextInputFieldId")));
        }
        
        [Test]
        public void WithinFieldset_example()
        {
            const string locatorThatAppearsInMultipleScopes = "scoped text input field linked by for";

            browser.WithinFieldset("Scope 1",
                () => Assert.That(browser.FindField(locatorThatAppearsInMultipleScopes).Id, Is.EqualTo("scope1TextInputFieldId")));

            browser.WithinFieldset("Scope 2",
                () => Assert.That(browser.FindField(locatorThatAppearsInMultipleScopes).Id, Is.EqualTo("scope2TextInputFieldId")));
        }

        [Test]
        public void WithinSection_example()
        {
            const string selectorThatAppearsInMultipleScopes = "h2";

            browser.WithinSection("Section One h1",
                () => Assert.That(browser.FindCss(selectorThatAppearsInMultipleScopes).Text, Is.EqualTo("Section One h2")));

            browser.WithinSection("Div Section Two h1",
                () => Assert.That(browser.FindCss(selectorThatAppearsInMultipleScopes).Text, Is.EqualTo("Div Section Two h2")));
        }

        [Test]
        public void WithinIFrame_example()
        {
            const string selectorThatAppearsInMultipleScopes = "scoped button";

            browser.WithinIFrame("iframe1",
                () => Assert.That(browser.FindButton(selectorThatAppearsInMultipleScopes).Id, Is.EqualTo("iframe1ButtonId")));

            browser.WithinIFrame("iframe2",
                () => Assert.That(browser.FindButton(selectorThatAppearsInMultipleScopes).Id, Is.EqualTo("iframe2ButtonId")));
        }

        [Test]
        public void Multiple_interactions_within_iframe_example()
        {
            browser.WithinIFrame("I am iframe one", () =>
                {
                    browser.FillIn("text input in iframe").With("filled in");
                    Assert.That(browser.FindField("text input in iframe").Value, Is.EqualTo("filled in"));
                });

        }
            
        [Test]
        public void FillIn_file_example()
        {
            const string someLocalFile = @"local.file";
            try
            {
                var directoryInfo = new DirectoryInfo(".");
                var fullPath = Path.Combine(directoryInfo.FullName, someLocalFile);
                using (File.Create(fullPath)) { }

                browser.FillIn("forLabeledFileFieldId").With(fullPath);

                var findAgain = browser.FindField("forLabeledFileFieldId");
                Assert.That(findAgain.Value, Is.StringEnding("\\" + someLocalFile));
            }
            finally
            {
                File.Delete(someLocalFile);
            }
        }

        [Test]
        public void ConsideringVisibleElements_action_example()
        {
            browser.ConsideringInvisibleElements(
                () =>
                {
                    browser.FindButton("firstInvisibleInputId");
                }
            );
        }
        [Test]
        public void ConsideringVisibleElements_func_example()
        {
            var button = browser.ConsideringInvisibleElements(
                () => browser.FindButton("firstInvisibleInputId"));

            Assert.That(button.Id, Is.EqualTo("firstInvisibleInputId"));

            Assert.Throws<MissingHtmlException>(() => browser.FindButton("firstInvisibleInputId"));
        }

        [Test]
        public void CustomProfile()
        {
            Configuration.Driver = typeof (CustomFirefoxProfileSeleniumWebDriver);

            Browser.Session.Visit("https://www.relishapp.com/");
        }

        public class CustomFirefoxProfileSeleniumWebDriver : SeleniumWebDriver
        {
            public CustomFirefoxProfileSeleniumWebDriver() : base(CustomProfile())
            {
            }

            private static RemoteWebDriver CustomProfile()
            {
                var yourCustomProfile = new FirefoxProfile();
                return new FirefoxDriver(yourCustomProfile);
            }
        }
    }
}