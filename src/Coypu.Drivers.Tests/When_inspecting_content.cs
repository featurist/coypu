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
                driver.HasContent("Some missing text").should_be_false();
            };

            it["finds text with parts marked up variously"] = () =>
            {
                driver.HasContent("Some text with parts marked up variously").should_be_true();
            };

            it["finds text in a table row"] = () =>
            {
                driver.HasContent("Some text in a table row").should_be_true();
            };

            it["finds text in a list"] = () =>
            {
                driver.HasContent("Some text in a list").should_be_true();
            };

            it["finds text split over multiple lines in source"] = () =>
            {
                driver.HasContent("Some text split over multiple lines in source").should_be_true();
            };

            it["finds text displayed over multiple lines in source"] = () =>
            {
                driver.HasContent("Some text displayed over\r\nmultiple lines").should_be_true();
                driver.HasContent("Some text displayed over\r\ntwo paragraphs").should_be_true();
            };

            it["does not find single line text displayed over multiple lines in source"] = () =>
            {
                driver.HasContent("Some text displayed over multiple lines").should_be_false();
            };

            it["finds text by regex"] = () =>
            {
                driver.HasContentMatch(new Regex(@"\bSome (text)? with [Pp]arts marked \w* variously")).should_be_true();
            };
        }
    }
}