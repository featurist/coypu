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

        public string Id
        {
            get { return id; }
        }

        public string Text
        {
            get { return string.Empty; }
        }

        public string Value
        {
            get { return string.Empty; }
        }

        public string Name
        {
            get { return string.Empty; }
        }

        public string SelectedOption
        {
            get { return string.Empty; }
        }

        public bool Selected
        {
            get { return false; }
        }

        public object Native
        {
            get { return null; }
        }

        public string this[string attributeName]
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