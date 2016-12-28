using System;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests
{
    public class When_configuring_options
    {
        [Fact]
        public void ConsiderInvisibleElements_defaults_to_false()
        {
            Assert.False(new Options().ConsiderInvisibleElements);
        }

        [Fact]
        public void Text_precision_defaults_to_prefer_exact()
        {
            Assert.Equal(TextPrecision.PreferExact, new Options().TextPrecision);
        }
        
        [Fact]
        public void Match_defaults_to_Single()
        {
            Assert.Equal(Match.Single, new Options().Match);
        }

        [Fact]
        public void RetryInterval_defaults_to_5_hundredths_of_a_second()
        {
            Assert.Equal(TimeSpan.FromSeconds(0.05), new Options().RetryInterval);
        }

        [Fact]
        public void Timeout_defaults_to_1_second()
        {
            Assert.Equal(TimeSpan.FromSeconds(1), new Options().Timeout);
        }

        [Fact]
        public void WaitBeforeClick_defaults_to_zero()
        {
            Assert.Equal(TimeSpan.Zero, new Options().WaitBeforeClick);
        }

        [Fact]
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

            Assert.NotSame(mergeWithOptions, merged);
            Assert.NotSame(defaultOptions, merged);

            var otherWayMerge = Options.Merge(mergeWithOptions,defaultOptions);
            AssertOptionsEqual(otherWayMerge, mergeWithOptions);
        }

        private static void AssertOptionsEqual(Options merged, Options mergeWithOptions)
        {
            Assert.Equal(mergeWithOptions.ConsiderInvisibleElements, merged.ConsiderInvisibleElements);
            Assert.Equal(mergeWithOptions.TextPrecision, merged.TextPrecision);
            Assert.Equal(mergeWithOptions.Match, merged.Match);
            Assert.Equal(mergeWithOptions.RetryInterval, merged.RetryInterval);
            Assert.Equal(mergeWithOptions.Timeout, merged.Timeout);
            Assert.Equal(mergeWithOptions.WaitBeforeClick, merged.WaitBeforeClick);
        }

        [Fact]
        public void ToString_shows_all_public_instance_properties()
        {
            Assert.Equal("Timeout: 00:00:01\r\nRetryInterval: 00:00:00.0500000\r\nWaitBeforeClick: 00:00:00\r\nConsiderInvisibleElements: False\r\nTextPrecision: PreferExact\r\nMatch: Single", new Options().ToString());
        }
    }
}