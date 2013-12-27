using System;

namespace Coypu.Queries
{
    public class LambdaQuery<T> : Query<T>
    {
        private readonly Func<T> _query;
        public Options Options { get; private set; }
        public T ExpectedResult { get; private set; }

        public LambdaQuery(Func<T> query)
        {
            _query = query;
        }

        public LambdaQuery(Func<T> query, Options options) : this(query)
        {
            Options = options;
        }

        public LambdaQuery(Func<T> query, T expectedResult, Options options) : this(query, options)
        {
            ExpectedResult = expectedResult;
        }

        public T Run()
        {
            return _query();
        }

    }
}