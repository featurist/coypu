using System;

namespace Coypu.Tests
{
    public class TestException : Exception
    {
        public TestException(string message) : base(message)
        {
        }
    }
}