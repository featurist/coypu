using System;
using System.Diagnostics;
using Coypu.Queries;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class AlwaysSucceedsPredicateQuery : PredicateQuery
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly bool actualResult;

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public AlwaysSucceedsPredicateQuery(bool actualResult, TimeSpan timeout, TimeSpan retryInterval)
            : base(new Options { Timeout = timeout, RetryInterval = retryInterval })
        {
            this.actualResult = actualResult;
            stopWatch.Start();
        }

        public override bool Predicate()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            return actualResult;
        }

        public override object ExpectedResult
        {
            get { return false; }
        }
    }

    public class AlwaysSucceedsQuery<T> : Query<T>
    {
        public Options Options { get; set; }
        public DriverScope Scope { get; private set; }
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly T actualResult;
        private readonly T expecting;

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public AlwaysSucceedsQuery(T actualResult, TimeSpan timeout, TimeSpan retryInterval)
        {
            Options = new Options { Timeout = timeout, RetryInterval = retryInterval };
            this.actualResult = actualResult;
            stopWatch.Start();
        }

        public AlwaysSucceedsQuery(T actualResult, T expecting, TimeSpan timeout, TimeSpan retryInterval)
            : this(actualResult,timeout, retryInterval)
        {
            this.expecting = expecting;
        }

        public T Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            return actualResult;
        }

        public object ExpectedResult
        {
            get { return expecting; }
        }
    }

    public class ThrowsSecondTimeQuery<T> : Query<T>
    {
        public Options Options { get; set; }
        public DriverScope Scope { get; private set; }
        private readonly T result;
        public TimeSpan Timeout { get; set; }

        public ThrowsSecondTimeQuery(T result, Options options)
        {
            Options = options;
            this.result = result;
        }

        public T Run()
        {
            Tries++;
            if (Tries == 1)
                throw new TestException("Fails first time");

            return result;
        }

        public object ExpectedResult
        {
            get { return default(T); }
        }

        public int Tries { get; set; }

        public TimeSpan RetryInterval
        {
            get { return default(TimeSpan); }
        }
    }

    public class AlwaysThrowsQuery<TResult, TException> : Query<TResult> where TException : Exception
    {
        public Options Options { get; set; }
        public DriverScope Scope { get; private set; }
        private readonly Stopwatch stopWatch = new Stopwatch();

        public AlwaysThrowsQuery(Options options)
        {
            Options = options;
            stopWatch.Start();
        }

        public TResult Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;
            throw (TException)Activator.CreateInstance(typeof(TException), "Test Exception");
        }

        public object ExpectedResult
        {
            get { return default(TResult); }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public TimeSpan Timeout { get; set; }

        public TimeSpan RetryInterval
        {
            get { return default(TimeSpan); }
        }
    }

    public class AlwaysThrowsPredicateQuery<TException> : PredicateQuery where TException : Exception
    {
        private readonly Stopwatch stopWatch = new Stopwatch();

        public AlwaysThrowsPredicateQuery(TimeSpan timeout, TimeSpan retryInterval) : base(new Options{Timeout = timeout,RetryInterval = retryInterval})
        {
            stopWatch.Start();
        }

        public override bool Predicate()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;
            throw (TException)Activator.CreateInstance(typeof(TException), "Test Exception");
        }

        public override object ExpectedResult
        {
            get { return false; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }

    }

    public class ThrowsThenSubsequentlySucceedsQuery<T> : Query<T>
    {
        public Options Options { get; set; }
        public DriverScope Scope { get; private set; }
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly T actualResult;
        private readonly T expectedResult;
        private readonly int throwsHowManyTimes;

        public ThrowsThenSubsequentlySucceedsQuery(T actualResult, T expectedResult, int throwsHowManyTimes, Options options)
        {
            Options = options;
            stopWatch.Start();
            this.actualResult = actualResult;
            this.expectedResult = expectedResult;
            this.throwsHowManyTimes = throwsHowManyTimes;
        }

        public T Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            if (Tries <= throwsHowManyTimes)
                throw new TestException("Fails first time");

            return actualResult;
        }

        public object ExpectedResult
        {
            get { return expectedResult; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }


        public TimeSpan Timeout
        {
            get { return default(TimeSpan); }
        }

        public TimeSpan RetryInterval
        {
            get { return default(TimeSpan); }
        }
    }

    public class ThrowsThenSubsequentlySucceedsPredicateQuery : PredicateQuery
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private readonly bool actualResult;
        private readonly bool expectedResult;
        private readonly int throwsHowManyTimes;


        public ThrowsThenSubsequentlySucceedsPredicateQuery(bool actualResult, bool expectedResult, int throwsHowManyTimes, Options options)
            : base(options)
        {
            stopWatch.Start();
            this.actualResult = actualResult;
            this.expectedResult = expectedResult;
            this.throwsHowManyTimes = throwsHowManyTimes;
        }

        public override bool Predicate()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            if (Tries <= throwsHowManyTimes)
            {
                Console.WriteLine("Fails on try " + Tries + " after " + LastCall + "ms");
                throw new TestException("Fails on try " + Tries + " after " + LastCall + "ms");
            }

            return actualResult;
        }

        public override object ExpectedResult
        {
            get { return expectedResult; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }

    }
}