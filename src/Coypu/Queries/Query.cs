using System;

namespace Coypu.Queries
{
    public interface Query<out TReturn>
    {
        void Run();
        object ExpectedResult { get; }
        TReturn Result { get; }
        TimeSpan Timeout { get; }
    }
}