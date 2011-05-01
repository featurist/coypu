namespace Coypu.Tests.TestDoubles
{
	public class StubNode : Node
	{
		public override string Id
		{
			get { return null; }
		}

		public override string Text
		{
			get { return null; }
		}

		public override string Value
		{
			get { return null; }
		}

		public override string Name
		{
			get { return null; }
		}

		public override string SelectedOption
		{
			get { return null; }
		}

		public override bool Selected
		{
			get { return false; }
		}
	}
}