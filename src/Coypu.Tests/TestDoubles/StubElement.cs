namespace Coypu.Tests.TestDoubles
{
	public class StubElement : Element
	{
	    private string id;

        public void SetId(string newId)
        {
			id = newId;
        }

	    public override string Id
	    {
	        get { return id; }
	    }

	    public override string Text
	    {
	        get { return string.Empty; }
	    }

	    public override string Value
	    {
	        get { return string.Empty; }
	    }

	    public override string Name
	    {
	        get { return string.Empty; }
	    }

	    public override string SelectedOption
	    {
			get { return string.Empty; }
	    }

	    public override bool Selected
	    {
	        get { return false; }
	    }

	    public override string this[string attributeName]
	    {
            get { return string.Empty; }
	    }
	}
}