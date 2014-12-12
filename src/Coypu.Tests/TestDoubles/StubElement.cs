using System.Collections.Generic;
namespace Coypu.Tests.TestDoubles
{
    public class StubElement : Element
    {
        private readonly Dictionary<string,string> attributes = new Dictionary<string,string>();

        public string Id { get; set; }

        public string Text { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }

        public string InnerHTML { get; set; }
        
        public string Title { get; set; }

        public bool Disabled { get; set; }

        public string OuterHTML { get; set; }

        public string SelectedOption { get; set; }

        public bool Selected { get; set; }

        public object Native { get; set; }

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