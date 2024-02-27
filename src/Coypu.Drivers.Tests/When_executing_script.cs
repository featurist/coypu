using Coypu.Finders;
using Shouldly;
using NUnit.Framework;
using System;
using Coypu.Tests;

namespace Coypu.Drivers.Tests
{
    internal class When_executing_script : DriverSpecs
    {
        [Test]
        public void Runs_the_script_in_the_browser()
        {
            Button("firstButtonId").Text.ShouldBe("first button");

            Driver.ExecuteScript("document.getElementById('firstButtonId').innerHTML = 'script executed';", Root);

            Button("firstButtonId").Text.ShouldBe("script executed");
        }

        [Test]
        public void Passes_the_arguments_to_the_browser()
        {
            Button("firstButtonId").Text.ShouldBe("first button");

            Driver.ExecuteScript("arguments[0].innerHTML = 'script executed ' + arguments[1];", Root, Button("firstButtonId"), 5);

            Button("firstButtonId").Text.ShouldBe("script executed 5");
        }

        [Test]
        public void Returns_the_result()
        {
            Driver.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;", Root).ShouldBe("first button");
            Driver.ExecuteScript("return 1234;", Root).ShouldBe(1234);
        }

         [Test]
        public void Returns_the_result_typed()
        {
            string str = Driver.ExecuteScript<string>("return 'miles';", Root);
            str.GetType().ShouldBe(typeof(string));

            int integer = Driver.ExecuteScript<int>("return 1234;", Root);
            integer.GetType().ShouldBe(typeof(int));

            DateTime dateTime = Driver.ExecuteScript<DateTime>("return new Date();", Root);
            dateTime.GetType().ShouldBe(typeof(DateTime));

            try {
              Driver.ExecuteScript<int>("return 'bob';", Root);
              throw new TestException("Expected an exception");
            } catch(Exception e) {
              if (e is TestException) throw;
            };
        }
    }
}
