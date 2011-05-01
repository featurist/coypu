using System;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	internal class When_choosing : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () => { };
		}
	}
}