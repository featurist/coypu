using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_constructing_xpath
    {
        [Test]
        public void It_handles_double_and_single_quotes_with_concat()
        {
            It_handles_double_and_single_quotes_with_concat("foo",            "\"foo\"");  // no quotes
            It_handles_double_and_single_quotes_with_concat("\"foo",          "'\"foo'");  // double quotes only
            It_handles_double_and_single_quotes_with_concat("'foo",           "\"'foo\""); // single quotes only
            It_handles_double_and_single_quotes_with_concat("'foo\"bar",      "concat(\"'foo\", '\"', \"bar\")");  // both; double quotes in mid-string
            It_handles_double_and_single_quotes_with_concat("'foo\"bar\"baz", "concat(\"'foo\", '\"', \"bar\", '\"', \"baz\")");  // multiple double quotes in mid-string
            It_handles_double_and_single_quotes_with_concat("'foo\"",         "concat(\"'foo\", '\"')");    // string ends with double quotes
            It_handles_double_and_single_quotes_with_concat("'foo\"\"",       "concat(\"'foo\", '\"', '\"')");  // string ends with run of double quotes
            It_handles_double_and_single_quotes_with_concat("\"'foo",         "concat('\"', \"'foo\")");    // string begins with double quotes
            It_handles_double_and_single_quotes_with_concat("\"\"'foo",       "concat('\"', '\"', \"'foo\")");  // string begins with run of double quotes
            It_handles_double_and_single_quotes_with_concat("'foo\"\"bar",    "concat(\"'foo\", '\"', '\"', \"bar\")");  // run of double quotes in mid-string
        }
       
        public void It_handles_double_and_single_quotes_with_concat(string input, string escaped)
        {
            Assert.That(new XPath().Literal(input), Is.EqualTo(escaped));
        }

    }
}
