using System;

namespace Coypu.Queries
{
    public class LambdaQuery<T> : Query<T>
    {
        private readonly Func<T> query;
        public Options Options { get; }
        public DriverScope Scope { get; }
        public object ExpectedResult { get; set; }

        public LambdaQuery(Func<T> query)
        {
            this.query = query;
        }

        public LambdaQuery(Func<T> query, Options options) : this(query)
        {
            Options = options;
        }

        public LambdaQuery(Func<T> query, object expectedResult, Options options) : this(query, options)
        {
            ExpectedResult = expectedResult;
        }

        public LambdaQuery(Func<T> query, object expectedResult, DriverScope scope, Options options)
            : this(query, expectedResult, options)
        {
            Scope = scope;
        }

        public T Run()
        {
            return query();
        }

    }
}