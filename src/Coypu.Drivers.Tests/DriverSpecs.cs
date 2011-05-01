using System;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	public interface DriverSpecs
	{
		Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it);
	}
}