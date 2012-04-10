using System;

namespace Coypu.Queries
{
    public interface Query<out TReturn>
    {
        TReturn Run();
        TReturn ExpectedResult { get; }
        TimeSpan Timeout { get; }
        TimeSpan RetryInterval { get; }
    }
}