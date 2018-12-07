using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    public class FindAllWithPredicateAndRetries : WaitAndRetryExamples
    {
        private const string ShouldFindCss = "#inspectingContent ul#cssTest li";
        private const string ShouldFindXPath = "//*[@id='inspectingContent']//ul[@id = 'cssTest']//li";

        [Test]
        public void FindAllCss_with_predicate_example()
        {
            var all = Browser.FindAllCss(ShouldFindCss, elements => elements.Count() == 3);
            CheckExpectedElements(all);
        }

        [Test]
        public void FindAllCss_with_failing_assertions_in_predicate_example()
        {
            Assert.Throws<AssertionException>(() => Browser.FindAllCss(ShouldFindCss,
                                                                       elements =>
                                                                       {
                                                                           Assert.That(elements.Count(), Is.EqualTo(4));
                                                                           return true;
                                                                       }
                                              ));
        }

        [Test]
        public void FindAllCss_with_passing_assertions_in_predicate_example()
        {
            Browser.FindCss("#cssTest")
                   .Hover();
            var all = Browser.FindAllCss(ShouldFindCss,
                                         elements =>
                                         {
                                             CheckExpectedElements(elements);
                                             return true;
                                         });

            CheckExpectedElements(all);
        }

        [Test]
        public void FindAllXPath_with_predicate_example()
        {
            var all = Browser.FindAllXPath(ShouldFindXPath, elements => elements.Count() == 3);
            CheckExpectedElements(all);
        }

        [Test]
        public void FindAllXPath_with_failing_assertions_in_predicate_example()
        {
            Assert.Throws<AssertionException>(() => Browser.FindAllXPath(ShouldFindXPath,
                                                                         elements =>
                                                                         {
                                                                             Assert.That(elements.Count(), Is.EqualTo(4));
                                                                             return true;
                                                                         }
                                              ));
        }

        [Test]
        public void FindAllXPath_with_passing_assertions_in_predicate_example()
        {
            Browser.FindCss("#cssTest")
                   .Hover();
            var all = Browser.FindAllXPath(ShouldFindXPath,
                                           elements =>
                                           {
                                               CheckExpectedElements(elements);
                                               return true;
                                           });

            CheckExpectedElements(all);
        }

        private static void CheckExpectedElements(IEnumerable<Coypu.SnapshotElementScope> all)
        {
            var snapshotElementScopes = all.ToList();
            Assert.That(snapshotElementScopes.Count, Is.EqualTo(3));
            Assert.That(snapshotElementScopes.ElementAt(1)
                           .Text,
                        Is.EqualTo("two"));
            Assert.That(snapshotElementScopes.ElementAt(2)
                           .Text,
                        Is.EqualTo("Me! Pick me!"));
        }
    }
}