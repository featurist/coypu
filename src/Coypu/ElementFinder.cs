using System;

namespace Coypu
{
    public interface ElementFinder
    {
        Element Find();
        TimeSpan Timeout { get; set; }
    }
}