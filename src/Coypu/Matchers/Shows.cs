using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Coypu.Matchers
{
    internal class Obsoletion
    {
        internal const string ObsoleteMessage =
            "NUnit matchers have moved into the package Coypu.NUnit. You need to install this package from nuget: > install-package Coypu.NUnit - and use Coypu.NUnit.Matchers.Shows";
    }

    [Obsolete(Obsoletion.ObsoleteMessage)]
    public static class Shows {
    }

    
    [Obsolete(Obsoletion.ObsoleteMessage)]
    public class ShowsNo
    {
    }
}