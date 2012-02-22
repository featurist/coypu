using System;
using System.Diagnostics;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class AlwaysSucceedsQuery<T> : Query<T>
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private T result;
        private readonly T actualResult;
        private readonly T expecting;

        public int Tries { get; set; }
        public long LastCall { get; set; }

        public AlwaysSucceedsQuery(T actualResult)
        {
            this.actualResult = actualResult;
            this.stopWatch.Start();
        }

        public AlwaysSucceedsQuery(T actualResult, T expecting)
            : this(actualResult)
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

    }

    public class ThrowsSecondTimeQuery<T> : Query<T>
    {
        private readonly T result;

        public ThrowsSecondTimeQuery(T result)
        {
            this.result = result;
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
    }

    public class AlwaysThrowsQuery<TException> : Query<object> where TException : Exception
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        
        public AlwaysThrowsQuery()
        {
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
            get { return null; }
        }


        public object Result
        {
            get { return null; }
        }

        public int Tries { get; set; }
        public long LastCall { get; set; }
    }

    public class TestDriverAction : DriverAction
    {
        public Query<object> FakeQuery { get; set; }

        public TestDriverAction(Query<object> fakeQuery)
        {
            FakeQuery = fakeQuery;
        }

        public void Act()
        {
            FakeQuery.Run();
        }
    }

    public class ThrowsThenSubsequentlySucceedsQuery<T> : Query<T>
    {
        private readonly Stopwatch stopWatch = new Stopwatch();
        private T result;
        private readonly T actualResult;
        private readonly T expectedResult;
        private readonly int throwsHowManyTimes;

        public ThrowsThenSubsequentlySucceedsQuery(T actualResult, T expectedResult, int throwsHowManyTimes)
        {
            stopWatch.Start();
            this.actualResult = actualResult;
            this.expectedResult = expectedResult;
            this.throwsHowManyTimes = throwsHowManyTimes;
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

    }
}