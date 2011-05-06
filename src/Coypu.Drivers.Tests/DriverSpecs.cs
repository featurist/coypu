using System;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	public class DriverSpecs
	{
		public DriverNSpec DriverNSpec { get; set; }

		protected Driver driver { get { return DriverNSpec.Driver; } }
		protected ActionRegister describe { get { return DriverNSpec.NSpecDescribe; } }
		protected ActionRegister it { get { return DriverNSpec.NSpecIt; } }
		protected Action before { set { DriverNSpec.NSpecBefore = value; } }
		protected Action after { set { DriverNSpec.NSpecAfter = value; } }

		internal virtual Action Specs()
		{
			// Override in each class with specs
			// TODO: Make this abstract -- but nspec needs a bug fix first
			return null;
		}
	}
}