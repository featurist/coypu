using Coypu.Drivers;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_constructing_xpath
    {
        [Fact]
        public void It_handles_double_and_single_quotes_with_concat()
        {
            It_handles_double_and_single_quotes_with_concat_theory("foo",            "\"foo\"");  // no quotes
            It_handles_double_and_single_quotes_with_concat_theory("\"foo",          "'\"foo'");  // double quotes only
            It_handles_double_and_single_quotes_with_concat_theory("'foo",           "\"'foo\""); // single quotes only
            It_handles_double_and_single_quotes_with_concat_theory("'foo\"bar",      "concat(\"'foo\", '\"', \"bar\")");  // both; double quotes in mid-string
            It_handles_double_and_single_quotes_with_concat_theory("'foo\"bar\"baz", "concat(\"'foo\", '\"', \"bar\", '\"', \"baz\")");  // multiple double quotes in mid-string
            It_handles_double_and_single_quotes_with_concat_theory("'foo\"",         "concat(\"'foo\", '\"')");    // string ends with double quotes
            It_handles_double_and_single_quotes_with_concat_theory("'foo\"\"",       "concat(\"'foo\", '\"', '\"')");  // string ends with run of double quotes
            It_handles_double_and_single_quotes_with_concat_theory("\"'foo",         "concat('\"', \"'foo\")");    // string begins with double quotes
            It_handles_double_and_single_quotes_with_concat_theory("\"\"'foo",       "concat('\"', '\"', \"'foo\")");  // string begins with run of double quotes
            It_handles_double_and_single_quotes_with_concat_theory("'foo\"\"bar",    "concat(\"'foo\", '\"', '\"', \"bar\")");  // run of double quotes in mid-string
        }
       
        private void It_handles_double_and_single_quotes_with_concat_theory(string input, string escaped)
        {
            Assert.Equal(escaped, new XPath().Literal(input));
        }

    }
}
