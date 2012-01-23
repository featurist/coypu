using System.Collections.Generic;
namespace Coypu.Tests.TestDoubles
{
    public class StubElement : Element
    {
        private readonly Dictionary<string,string> attributes = new Dictionary<string,string>();
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
            get 
            {
                return attributes.ContainsKey(attributeName) ? attributes[attributeName] : string.Empty;
            }
        }

        public void StubAttribute(string attributeName, string value)
        { 
            attributes[attributeName] = value; 
        }
    }
}