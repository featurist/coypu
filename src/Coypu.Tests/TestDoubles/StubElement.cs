namespace Coypu.Tests.TestDoubles
{
	public class StubElement : Element
	{
	    private string id;
	    private string text;
	    private string value;
	    private string name;
	    private string selectedOption;
	    private bool selected;

        public void SetId(string id)
        {
            this.id = id;
        }

	    public override string Id
	    {
	        get { return id; }
	    }

	    public override string Text
	    {
	        get { return text; }
	    }

	    public override string Value
	    {
	        get { return value; }
	    }

	    public override string Name
	    {
	        get { return name; }
	    }

	    public override string SelectedOption
	    {
	        get { return selectedOption; }
	    }

	    public override bool Selected
	    {
	        get { return selected; }
	    }

	    public override string this[string attributeName]
	    {
            get { return string.Empty; }
	    }
	}
}