using System.Collections.Generic;
using System.Net;

namespace Coypu.Drivers.Watin
{
    public class CookieNameEqualityComparer : IEqualityComparer<Cookie>
    {
        public bool Equals(Cookie x, Cookie y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(Cookie obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}