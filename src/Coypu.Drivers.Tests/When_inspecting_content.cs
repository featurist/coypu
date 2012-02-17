using NSpec;
using System.Text.RegularExpressions;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_content : DriverSpecs
    {
        internal override void Specs()
        {
            it["does not find missing text"] = () =>
            {
                driver.HasContent("Some missing text", Root).should_be_false();
            };

            it["finds text with parts marked up variously"] = () =>
            {
                driver.HasContent("Some text with parts marked up variously", Root).should_be_true();
            };

            it["finds text in a table row"] = () =>
            {
                driver.HasContent("Some text in a table row", Root).should_be_true();
            };

            it["finds text in a list"] = () =>
            {
                driver.HasContent("Some\r\ntext\r\nin\r\na\r\nlist", Root).should_be_true();
            };

            it["finds text split over multiple lines in source"] = () =>
            {
                driver.HasContent("Some text split over multiple lines in source", Root).should_be_true();
            };

            it["finds text displayed over multiple lines in source"] = () =>
            {
                driver.HasContent("Some text displayed over\r\nmultiple lines", Root).should_be_true();
                driver.HasContent("Some text displayed over\r\ntwo paragraphs", Root).should_be_true();
            };

            it["does not find single line text displayed over multiple lines in source"] = () =>
            {
                driver.HasContent("Some text displayed over multiple lines", Root).should_be_false();
            };

            it["finds text by regex"] = () =>
            {
                driver.HasContentMatch(new Regex(@"\bSome (text)? with [Pp]arts marked \w* variously"), Root).should_be_true();
            };
        }
    }
}