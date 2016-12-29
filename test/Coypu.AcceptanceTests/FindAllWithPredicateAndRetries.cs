using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class FindAllWithPredicateAndRetries : WaitAndRetryExamples
    {
        const string shouldFindCSS = "#inspectingContent ul#cssTest li";
        const string shouldFindXPath = "//*[@id='inspectingContent']//ul[@id = 'cssTest']//li";

        public FindAllWithPredicateAndRetries(WaitAndRetryExamplesFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void FindAllCss_with_predicate_example()
        {
            var all = browser.FindAllCss(shouldFindCSS, (elements) => elements.Count() == 3);
            CheckExpectedElements(all);
        }

        [Fact]
        public void FindAllCss_with_failing_asertions_in_predicate_example()
        {
            Assert.Throws<Exception>(() => browser.FindAllCss(shouldFindCSS, (elements) =>
                {
                    Assert.Equal(4, elements.Count());
                    return true;
                }
            ));
        }

        [Fact]
        public void FindAllCss_with_passing_asertions_in_predicate_example()
        {
            var all = browser.FindAllCss(shouldFindCSS, (elements) =>
            {
                CheckExpectedElements(elements);
                return true;
            });

            CheckExpectedElements(all);
        }

        [Fact]
        public void FindAllXPath_with_predicate_example()
        {
            var all = browser.FindAllXPath(shouldFindXPath, (elements) => elements.Count() == 3);
            CheckExpectedElements(all);
        }

        [Fact]
        public void FindAllXPath_with_failing_asertions_in_predicate_example()
        {
            Assert.Throws<Exception>(() => browser.FindAllXPath(shouldFindXPath, (elements) =>
            {
                Assert.Equal(4, elements.Count());
                return true;
            }
            ));
        }

        [Fact]
        public void FindAllXPath_with_passing_asertions_in_predicate_example()
        {
            var all = browser.FindAllXPath(shouldFindXPath, (elements) =>
            {
                CheckExpectedElements(elements);
                return true;
            });

            CheckExpectedElements(all);
        }

        private static void CheckExpectedElements(IEnumerable<Coypu.SnapshotElementScope> all)
        {
            Assert.Equal(3, all.Count());
            Assert.Equal("two", all.ElementAt(1).Text);
            Assert.Equal("Me! Pick me!", all.ElementAt(2).Text);
        }
    }
}