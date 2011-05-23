using System;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	public class DriverSpecs
	{
		public DriverSpecRunner DriverSpecRunner { get; set; }

		protected Driver driver { get { return DriverSpecRunner.Driver; } }
		protected ActionRegister describe { get { return DriverSpecRunner.NSpecDescribe; } }
		protected ActionRegister context { get { return DriverSpecRunner.NSpecContext; } }
		protected ActionRegister it { get { return DriverSpecRunner.NSpecIt; } }
		protected Action before { set { DriverSpecRunner.NSpecBefore = value; } }
		protected Action after { set { DriverSpecRunner.NSpecAfter = value; } }

		internal virtual void Specs()
		{
			// Override in each class with specs
			// TODO: Make this abstract -- but nspec needs a bug fix first
		}
	}
}