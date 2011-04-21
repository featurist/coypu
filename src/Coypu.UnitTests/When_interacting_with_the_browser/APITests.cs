using Coypu.UnitTests.TestDoubles;
using NUnit.Framework;

namespace Coypu.UnitTests.When_interacting_with_the_browser
{
	public class APITests
	{
		protected FakeDriver Driver;
		protected SpyRobustWrapper SpyRobustWrapper;
		protected Session Session;

		[SetUp]
		public void SetUp()
		{
			Driver = new FakeDriver();
			SpyRobustWrapper = new SpyRobustWrapper();
			Session = new Session(Driver, SpyRobustWrapper);
		}
	}
}