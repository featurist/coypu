using System.Text.RegularExpressions;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_content : DriverSpecs
    {
        [Test]
        public void Does_not_find_missing_text()
        {
            Driver.HasContent("Some missing text", Root).should_be_false();
        }


        [Test]
        public void Finds_text_with_parts_marked_up_variously()
        {
            Driver.HasContent("Some text with parts marked up variously", Root).should_be_true();
        }


        [Test]
        public void Finds_text_in_a_table_row()
        {
            Driver.HasContent("Some text in a table row", Root).should_be_true();
        }


        [Test]
        public void Finds_text_in_a_list()
        {
            Driver.HasContent("Some\r\ntext\r\nin\r\na\r\nlist", Root).should_be_true();
        }


        [Test]
        public void Finds_text_split_over_multiple_lines_in_source()
        {
            Driver.HasContent("Some text split over multiple lines in source", Root).should_be_true();
        }


        [Test]
        public void Finds_text_displayed_over_multiple_lines_in_source()
        {
            Driver.HasContent("Some text displayed over\r\nmultiple lines", Root).should_be_true();
            Driver.HasContent("Some text displayed over\r\ntwo paragraphs", Root).should_be_true();
        }


        [Test]
        public void Does_not_find_single_line_text_displayed_over_multiple_lines_in_source()
        {
            Driver.HasContent("Some text displayed over multiple lines", Root).should_be_false();
        }


        [Test]
        public void Finds_text_by_regex()
        {
            Driver.HasContentMatch(new Regex(@"\bSome (text)? with [Pp]arts marked \w* variously"), Root).should_be_true();
        }
    }
}