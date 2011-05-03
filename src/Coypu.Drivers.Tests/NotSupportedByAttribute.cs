using System;

namespace Coypu.Drivers.Tests
{
	internal class NotSupportedByAttribute : Attribute
	{
		public Type[] Types { get; private set; }

		public NotSupportedByAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}