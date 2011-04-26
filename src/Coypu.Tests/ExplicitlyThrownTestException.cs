using System;

namespace Coypu.Tests
{
	public class ExplicitlyThrownTestException : Exception
	{
		public ExplicitlyThrownTestException(string message) : base(message)
		{
		}
	}
}