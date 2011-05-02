using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_interacting_with_modal_dialogs
	{
		private FakeDriver driver;
		private SpyRobustWrapper spyRobustWrapper;
		private Session session;

		[SetUp]
		public void SetUp()
		{
			driver = new FakeDriver();
			spyRobustWrapper = new SpyRobustWrapper();
			session = new Session(driver, spyRobustWrapper);
		}

		[Test]
		public void AcceptDialog_should_make_robust_call_to_underlying_driver()
		{
			session.AcceptModalDialog();

			Assert.That(driver.ModalDialogsAccepted, Is.EqualTo(0));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ModalDialogsAccepted, Is.EqualTo(1));
		}

		[Test]
		public void CancelDialog_should_make_robust_call_to_underlying_driver()
		{
			session.CancelModalDialog();

			Assert.That(driver.ModalDialogsCancelled, Is.EqualTo(0));
			spyRobustWrapper.DeferredActions.Single()();
			Assert.That(driver.ModalDialogsCancelled, Is.EqualTo(1));
		}

	}
}