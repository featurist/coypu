using System;
using System.Runtime.Serialization;

namespace Coypu
{
    /// <summary>
    /// Thrown whenever a snapshot element returned from FindAllCss or FindAllXPath has been removed
    /// from the DOM and cannot be refound due to the snapshot nature of the FindAll methods
    /// </summary>
    public class StaleElementException : Exception
    {
        private new const string Message = "This element has been removed from the DOM. Coypu will normally re-find elements using the original locators in this situation, except if you have captured a snapshot list of all matching elements using FindAllCss() or FindAllXPath()";

        public StaleElementException()
            : base(Message)
        {
        }

        public StaleElementException(Exception innerException)
            : base(Message, innerException)
        {
        }

        public StaleElementException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}