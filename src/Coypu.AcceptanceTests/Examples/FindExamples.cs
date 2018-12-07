using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class FindExamples : WaitAndRetryExamples
    {
        [Test]
        public void FindAllCss_example()
        {
            ReloadTestPage();
            const string shouldFind = "#inspectingContent ul#cssTest li";
            var all = Browser.FindAllCss(shouldFind)
                             .ToList();
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1)
                           .Text,
                        Is.EqualTo("two"));
            Assert.That(all.ElementAt(2)
                           .Text,
                        Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindAllXPath_example()
        {
            ReloadTestPage();
            const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
            var all = Browser.FindAllXPath(shouldFind)
                             .ToArray();
            Assert.That(all.Count(), Is.EqualTo(3));
            Assert.That(all.ElementAt(1)
                           .Text,
                        Is.EqualTo("two"));
            Assert.That(all.ElementAt(2)
                           .Text,
                        Is.EqualTo("Me! Pick me!"));
        }

        [Test]
        public void FindButton_example()
        {
            Assert.That(Browser.FindButton("Click me")
                               .Id,
                        Is.EqualTo("clickMeTest"));
        }

        [Test]
        public void FindCss_example()
        {
            var first = Browser.FindCss("#inspectingContent ul#cssTest li", Options.First);
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindCss_with_text_example()
        {
            var two = Browser.FindCss("#inspectingContent ul#cssTest li", "two");
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void FindCss_with_text_matching()
        {
            var two = Browser.FindCss("#inspectingContent ul#cssTest li", new Regex("wo"));
            Assert.That(two.Text, Is.EqualTo("two"));
        }

        [Test]
        public void FindField_examples()
        {
            Assert.That(Browser.FindField("text input field linked by for", Options.Exact)
                               .Id,
                        Is.EqualTo("forLabeledTextInputFieldId"));
            Assert.That(Browser.FindField("checkbox field in a label container")
                               .Id,
                        Is.EqualTo("containerLabeledCheckboxFieldId"));
            Assert.That(Browser.FindField("containerLabeledSelectFieldId")
                               .Name,
                        Is.EqualTo("containerLabeledSelectFieldName"));
            Assert.That(Browser.FindField("containerLabeledPasswordFieldName")
                               .Id,
                        Is.EqualTo("containerLabeledPasswordFieldId"));
        }

        [Test]
        public void FindFieldset_example()
        {
            Assert.That(Browser.FindFieldset("Scope 1")
                               .Id,
                        Is.EqualTo("fieldsetScope1"));
        }

        [Test]
        public void FindId_example()
        {
            Assert.That(Browser.FindId("containerLabeledSelectFieldId")
                               .Name,
                        Is.EqualTo("containerLabeledSelectFieldName"));
        }

        [Test]
        public void FindIdEndingWith_example()
        {
            Assert.That(Browser.FindIdEndingWith("aspWebFormsContainerLabeledFileFieldId")
                               .Id,
                        Is.EqualTo("_ctrl01_ctrl02_aspWebFormsContainerLabeledFileFieldId"));
        }

        [Test]
        public void FindLink_example()
        {
            Assert.That(Browser.FindLink("Trigger an alert")
                               .Id,
                        Is.EqualTo("alertTriggerLink"));
        }

        [Test]
        public void Finds_link_by_href()
        {
            Assert.That(Browser.FindLink("#link1href")
                               .Id,
                        Is.EqualTo("firstLinkId"));
            Assert.That(Browser.FindLink("#link2href")
                               .Id,
                        Is.EqualTo("secondLinkId"));
        }

        [Test]
        public void FindSection_example()
        {
            Assert.That(Browser.FindSection("Inspecting Content")
                               .Id,
                        Is.EqualTo("inspectingContent"));
            Assert.That(Browser.FindSection("Div Section Two h2 with link")
                               .Id,
                        Is.EqualTo("divSectionTwoWithLink"));
        }

        [Test]
        public void FindXPath_example()
        {
            var first = Browser.FindXPath("//*[@id='inspectingContent']//ul[@id='cssTest']/li", Options.First);
            Assert.That(first.Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindXPath_with_text_example()
        {
            var two = Browser.FindXPath("//*[@id='inspectingContent']//ul[@id='cssTest']/li", "two");
            Assert.That(two.Text, Is.EqualTo("two"));
        }
    }
}