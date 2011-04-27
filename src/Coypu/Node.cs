namespace Coypu
{
	public abstract class Node
	{
		public abstract string Id { get; }
		public abstract string Text { get; }
		public abstract string Value { get; }
		public abstract string Name { get; }
	    public abstract string SelectedOption { get; }
	    public abstract bool Selected { get;}
	    public object Native { get; protected set; }
	}
}