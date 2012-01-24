using System;

namespace Coypu
{
    public interface ElementFinder
    {
        Element Now();
        TimeSpan Timeout { get; set; }
    }
}