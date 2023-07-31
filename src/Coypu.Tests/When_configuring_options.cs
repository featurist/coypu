using System;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests
{
    [TestFixture]
    public class When_configuring_options
    {
        [Test]
        public void ConsiderInvisibleElements_defaults_to_false()
        {
            Assert.That(new Options().ConsiderInvisibleElements, Is.EqualTo(false));
        }

        [Test]
        public void Text_precision_defaults_to_prefer_exact()
        {
            Assert.That(new Options().TextPrecision, Is.EqualTo(TextPrecision.PreferExact));
        }
        
        [Test]
        public void Match_defaults_to_Single()
        {
            Assert.That(new Options().Match, Is.EqualTo(Match.Single));
        }

        [Test]
        public void RetryInterval_defaults_to_5_hundredths_of_a_second()
        {
            Assert.That(new Options().RetryInterval, Is.EqualTo(TimeSpan.FromSeconds(0.05)));
        }

        [Test]
        public void Timeout_defaults_to_1_second()
        {
            Assert.That(new Options().Timeout, Is.EqualTo(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void WaitBeforeClick_defaults_to_zero()
        {
            Assert.That(new Options().WaitBeforeClick, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void Merging_options_sets_unset_options()
        {
            var defaultOptions = new Options();
            var mergeWithOptions = new Options
                {
                    ConsiderInvisibleElements = true,
                    TextPrecision = TextPrecision.Substring,
                    Match = Match.First,
                    RetryInterval = TimeSpan.FromSeconds(123),
                    Timeout = TimeSpan.FromSeconds(456),
                    WaitBeforeClick = TimeSpan.FromSeconds(789)
                };

            var merged = Options.Merge(defaultOptions,mergeWithOptions);

            AssertOptionsEqual(merged, mergeWithOptions);

            Assert.That(merged, Is.Not.SameAs(mergeWithOptions));
            Assert.That(merged, Is.Not.SameAs(defaultOptions));

            var otherWayMerge = Options.Merge(mergeWithOptions,defaultOptions);
            AssertOptionsEqual(otherWayMerge, mergeWithOptions);
        }

        private static void AssertOptionsEqual(Options merged, Options mergeWithOptions)
        {
            Assert.That(merged.ConsiderInvisibleElements, Is.EqualTo(mergeWithOptions.ConsiderInvisibleElements));
            Assert.That(merged.TextPrecision, Is.EqualTo(mergeWithOptions.TextPrecision));
            Assert.That(merged.Match, Is.EqualTo(mergeWithOptions.Match));
            Assert.That(merged.RetryInterval, Is.EqualTo(mergeWithOptions.RetryInterval));
            Assert.That(merged.Timeout, Is.EqualTo(mergeWithOptions.Timeout));
            Assert.That(merged.WaitBeforeClick, Is.EqualTo(mergeWithOptions.WaitBeforeClick));
        }

        [Test]
        public void ToString_shows_all_public_instance_properties()
        {
            Assert.That(new Options().ToString(), Is.EqualTo("Timeout: 00:00:01" + Environment.NewLine +
                                                             "RetryInterval: 00:00:00.0500000" + Environment.NewLine +
                                                             "WaitBeforeClick: 00:00:00" + Environment.NewLine +
                                                             "ConsiderInvisibleElements: False" + Environment.NewLine +
                                                             "TextPrecision: PreferExact" + Environment.NewLine +
                                                             "Match: Single"));
        }
    }
}