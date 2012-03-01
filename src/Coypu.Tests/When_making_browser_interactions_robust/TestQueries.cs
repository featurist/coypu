using System;
using System.Diagnostics;
using Coypu.Queries;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class AlwaysSucceedsQuery<T> : Query<T>
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private T result;
        private readonly T actualResult;
        private readonly T expecting;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _retryInterval;

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public AlwaysSucceedsQuery(T actualResult, TimeSpan timeout, TimeSpan retryInterval)
        {
            _timeout = timeout;
            _retryInterval = retryInterval;
            this.actualResult = actualResult;
            stopWatch.Start();
        }

        public AlwaysSucceedsQuery(T actualResult, T expecting, TimeSpan timeout, TimeSpan retryInterval)
            : this(actualResult,timeout,retryInterval)
        {
            this.expecting = expecting;
        }

        public void Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            result = actualResult;
        }

        public object ExpectedResult
        {
            get { return expecting; }
        }

        public T Result
        {
            get { return result; }
        }

        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class ThrowsSecondTimeQuery<T> : Query<T>
    {
        private readonly T result;
        private readonly TimeSpan _retryInterval;
        public TimeSpan Timeout { get; set; }

        public ThrowsSecondTimeQuery(T result, TimeSpan timeout, TimeSpan retryInterval)
        {
            this.result = result;
            _retryInterval = retryInterval;
            Timeout = timeout;
        }

        public void Run()
        {
            Tries++;
            if (Tries == 1)
                throw new TestException("Fails first time");
        }

        public object ExpectedResult
        {
            get { return null; }
        }

        public T Result
        {
            get { return result; }
        }

        public int Tries { get; set; }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class AlwaysThrowsQuery<TResult,TException> : Query<TResult> where TException : Exception
    {
        private readonly TimeSpan _retryInterval;
        private readonly Stopwatch stopWatch = new Stopwatch();
        
        public AlwaysThrowsQuery(TimeSpan timeout, TimeSpan retryInterval)
        {
            _retryInterval = retryInterval;
            Timeout = timeout;
            stopWatch.Start();
        }

        public void Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;
            throw (TException)Activator.CreateInstance(typeof(TException), "Test Exception");
        }

        public object ExpectedResult
        {
            get { return default(TResult); }
        }


        public TResult Result
        {
            get { return default(TResult); }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public TimeSpan Timeout { get; set; }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }

    public class ThrowsThenSubsequentlySucceedsQuery<T> : Query<T>
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private T result;
        private readonly T actualResult;
        private readonly T expectedResult;
        private readonly int throwsHowManyTimes;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _retryInterval;

        public ThrowsThenSubsequentlySucceedsQuery(T actualResult, T expectedResult, int throwsHowManyTimes, TimeSpan timeout, TimeSpan retryInterval)
        {
            stopWatch.Start();
            this.actualResult = actualResult;
            this.expectedResult = expectedResult;
            this.throwsHowManyTimes = throwsHowManyTimes;
            _timeout = timeout;
            _retryInterval = retryInterval;
        }

        public void Run()
        {
            Tries++;
            LastCall = stopWatch.ElapsedMilliseconds;

            if (Tries <= throwsHowManyTimes)
                throw new TestException("Fails first time");

            result = actualResult;
        }

        public object ExpectedResult
        {
            get { return expectedResult; }
        }

        public T Result
        {
            get { return result; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }


        public TimeSpan Timeout
        {
            get { return _timeout; }
        }

        public TimeSpan RetryInterval
        {
            get { return _retryInterval; }
        }
    }
}