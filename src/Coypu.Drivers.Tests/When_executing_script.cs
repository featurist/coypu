using System.Threading;
using Coypu.Drivers.Watin;
using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_executing_script : DriverSpecs
    {
        internal override void Specs()
        {
            it["runs the script in the browser"] = () =>
            {
                driver.FindButton("firstButtonId").Text.should_be("first button");

                driver.ExecuteScript("document.getElementById('firstButtonId').innerHTML = 'script executed';");

                driver.FindButton("firstButtonId").Text.should_be("script executed");
            };

            it["returns the result"] = () =>
            {
                driver.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;").should_be("first button");
            };
        }
    }
}