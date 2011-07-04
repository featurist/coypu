using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
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
            Console.WriteLine(Uri.IsWellFormedUriString("file:///" + new FileInfo(@"html\InteractionTestsPage.htm"),UriKind.Absolute));
            browser.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\","/"));
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
            const string shouldFind = "#inspectingContent ul#cssTest li";
            var all = browser.FindAllCss(shouldFind);
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1).Text, Is.EqualTo("two"));
            Assert.That(all.ElementAt(2).Text, Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindAllXPath_example()
        {
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
/*
        [Test]
        public void FindSection_example()
        {
            browser.FindSection();
        }

        [Test]
        public void Has_example()
        {
            browser.Has();
        }

        [Test]
        public void HasContent_example()
        {
            browser.HasContent();
        }

        [Test]
        public void HasContentMatch_example()
        {
            browser.HasContentMatch();
        }

        [Test]
        public void HasCss_example()
        {
            browser.HasCss();
        }

        [Test]
        public void HasDialog_example()
        {
            browser.HasDialog();
        }

        [Test]
        public void HasNo_example()
        {
            browser.HasNo();
        }

        [Test]
        public void HasNoContent_example()
        {
            browser.HasNoContent();
        }

        [Test]
        public void HasNoContentMatch_example()
        {
            browser.HasNoContentMatch();
        }

        [Test]
        public void HasNoCss_example()
        {
            browser.HasNoCss();
        }

        [Test]
        public void HasNoDialog_example()
        {
            browser.HasNoDialog();
        }

        [Test]
        public void HasNoXPath_example()
        {
            browser.HasNoXPath();
        }

        [Test]
        public void Hover_example()
        {
            browser.Hover();
        }

        [Test]
        public void Native_example()
        {
            browser.Native;
        }

        [Test]
        public void Uncheck_example()
        {
            browser.Uncheck();
        }

        [Test]
        public void Within_example()
        {
            browser.Within();
        }

        [Test]
        public void WithIndividualTimeout_example()
        {
            browser.WithIndividualTimeout();
        }

        [Test]
        public void WithinFieldset_example()
        {
            browser.WithinFieldset();
        }

        [Test]
        public void WithinIFrame_example()
        {
            browser.WithinIFrame();
        }

        [Test]
        public void WithinSection_example()
        {
            browser.WithinSection();
        }
 */
    }
}