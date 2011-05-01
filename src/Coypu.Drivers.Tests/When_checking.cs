using System;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	internal class When_checking : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () => { };
		}
	}
}