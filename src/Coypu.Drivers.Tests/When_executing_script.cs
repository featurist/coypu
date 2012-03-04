using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_executing_script : DriverSpecs
    {
        [Test]
        public void Runs_the_script_in_the_browser()
        {
            Driver.FindButton("firstButtonId", Root).Text.should_be("first button");

            Driver.ExecuteScript("document.getElementById('firstButtonId').innerHTML = 'script executed';", Root);

            Driver.FindButton("firstButtonId", Root).Text.should_be("script executed");
        }


        [Test]
        public void Returns_the_result()

        {
            Driver.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;", Root).should_be("first button");
        }
    }
}