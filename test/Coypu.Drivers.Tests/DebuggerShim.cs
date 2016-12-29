using System;
using Xunit;
using NSpec.Domain;
using System.Reflection;
using NSpec;
using NSpec.Domain.Formatters;
using System.Linq;

//[TestFixture]
public class DebuggerShim
{
    //[Fact]
    public void debug()
    {
        var tagOrClassName = "class_or_tag_you_want_to_debug";

        var invocation = new RunnerInvocation(Assembly.GetExecutingAssembly().Location, tagOrClassName);

        var contexts = invocation.Run();

        //assert that there aren't any failures
        contexts.Failures().Count().should_be(0);
    }
}
