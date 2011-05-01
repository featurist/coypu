using System.IO;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	public abstract class DriverImplementationTests : nspec
	{
		private const string INTERACTION_TESTS_PAGE = @"..\..\html\InteractionTestsPage.htm";
		private Driver driver;

		[TestFixtureSetUp]
		public void FixtureSetUp()
		{

		}

		public void NewSession()
		{
			driver.Dispose();
			driver = GetDriver();
		}

		[SetUp]
		public void SetUp()
		{
			driver.Visit(new FileInfo(INTERACTION_TESTS_PAGE).FullName);
		}

		[TestFixtureTearDown]
		public void Dispose()
		{
			driver.Dispose();
		}

		protected abstract Driver GetDriver();



	


		



	   


	}
}
