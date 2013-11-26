using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_returns_ambiguous_results : DriverSpecs
    {
        private static DriverScope RootWithExactFieldFinderOption
        {
            get
            {
                var configuration = new SessionConfiguration
                    {
                        FieldFinderPrecision = FieldFinderPrecision.ExactLabel
                    };
                return new DriverScope(configuration, new DocumentElementFinder(Driver), null, null, null, null);
            }
        }

        [Test]
        public void When_more_than_one_for_labeled_partial_match_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("Some for labeled radio option", Root));
        }

        [Test]
        public void When_more_than_one_container_labeled_partial_match_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("Some container labeled radio option", Root));
        }

        [Test]
        public void When_more_than_field_with_the_same_name_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("someFieldNameThatAppearsTwice", Root));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_for_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Driver.FindField("Some for labeled radio option", RootWithExactFieldFinderOption).Id,
                        Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_container_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Driver.FindField("Some container labeled radio option", RootWithExactFieldFinderOption).Id,
                        Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_more_than_field_with_the_same_name_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("someFieldNameThatAppearsTwice", RootWithExactFieldFinderOption));
        }

        [Test]
        public void When_set_to_partial_And_only_one_for_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Driver.FindField("Some for labeled radio option that might conflict", Root).Id,
                        Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_container_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Driver.FindField("Some container labeled radio option that might conflict", Root).Id,
                        Is.EqualTo("containerLabeledRadioFieldPartialMatchId"));
        }
    }
}