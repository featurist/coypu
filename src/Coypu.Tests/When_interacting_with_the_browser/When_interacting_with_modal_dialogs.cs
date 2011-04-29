using System;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
	[TestFixture]
	public class When_inspecting_with_modal_dialogs : When_inspecting
	{
		[Test]
		public void HasDialog_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(true, true, Session.HasDialog, Driver.StubDialog);
		}

		[Test]
		public void HasDialog_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(true, false, Session.HasDialog, Driver.StubDialog);
		}

		[Test]
		public void HasNoDialog_should_wait_for_robustly_Positive_example()
		{
			Should_wait_for_robustly(false, true, Session.HasNoDialog, Driver.StubDialog);
		}

		[Test]
		public void HasNoDialog_should_wait_for_robustly_Negative_example()
		{
			Should_wait_for_robustly(false, false, Session.HasNoDialog, Driver.StubDialog);
		}

		// Next : Accept + Cancel
	}
}