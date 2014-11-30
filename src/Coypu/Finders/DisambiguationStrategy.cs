using Coypu;
using Coypu.Finders;

public interface DisambiguationStrategy
{
    Element ResolveQuery(ElementFinder elementFinder);
}