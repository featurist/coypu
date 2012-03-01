using System;

namespace Coypu.Queries
{
    public class LambdaQuery<T> : Query<T>
    {
        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }

        private readonly Func<T> _query;

        public object ExpectedResult { get; private set; }
        public T Result { get; private set; }

        public LambdaQuery(Func<T> query)
        {
            _query = query;
        }

        public LambdaQuery(Func<T> query, TimeSpan timeout)
        {
            Timeout = timeout;
            _query = query;
        }

        public LambdaQuery(Func<T> query, object expectedResult)
        {
            _query = query;
            ExpectedResult = expectedResult;
        }

        public void Run()
        {
            Result = _query();
        }

    }
}