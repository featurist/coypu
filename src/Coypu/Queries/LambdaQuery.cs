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

        public LambdaQuery(Func<T> query, Options options)
        {
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
            _query = query;
        }

        public LambdaQuery(Func<T> query, object expectedResult, Options options)
        {
            _query = query;
            ExpectedResult = expectedResult;
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public void Run()
        {
            Result = _query();
        }

    }
}