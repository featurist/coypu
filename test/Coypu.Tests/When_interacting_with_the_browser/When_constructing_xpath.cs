using Coypu.Drivers;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_constructing_xpath
    {
        [Theory]
        [InlineData("foo", "\"foo\"")]  // no quotes
        [InlineData("\"foo", "'\"foo'")]  // double quotes only
        [InlineData("'foo", "\"'foo\"")] // single quotes only
        [InlineData("'foo\"bar", "concat(\"'foo\", '\"', \"bar\")")]  // both; double quotes in mid-string
        [InlineData("'foo\"bar\"baz", "concat(\"'foo\", '\"', \"bar\", '\"', \"baz\")")]  // multiple double quotes in mid-string
        [InlineData("'foo\"", "concat(\"'foo\", '\"')")]    // string ends with double quotes
        [InlineData("'foo\"\"", "concat(\"'foo\", '\"', '\"')")]  // string ends with run of double quotes
        [InlineData("\"'foo", "concat('\"', \"'foo\")")]    // string begins with double quotes
        [InlineData("\"\"'foo", "concat('\"', '\"', \"'foo\")")]  // string begins with run of double quotes
        [InlineData("'foo\"\"bar", "concat(\"'foo\", '\"', '\"', \"bar\")")]  // run of double quotes in mid-string
        public void It_handles_double_and_single_quotes_with_concat(string input, string escaped)
        {
            Assert.Equal(escaped, new XPath().Literal(input));
        }
    }
}
