using System;

namespace Coypu.Queries
{
    internal class LambdaQuery<T> : Query<T>
    {
        private readonly Func<T> _query;

        public object ExpectedResult { get; private set; }
        public T Result { get; private set; }

        public LambdaQuery(Func<T> query)
        {
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