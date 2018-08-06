using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu
{
    public interface HasSession
    {
        void SetScope(BrowserSession s);
    }
}
