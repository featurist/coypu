using Coypu;
using Coypu.Finders;

public interface DisambiguationStrategy
{
    ElementFound ResolveQuery(ElementFinder elementFinder);
}