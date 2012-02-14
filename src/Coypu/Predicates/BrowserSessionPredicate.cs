namespace Coypu.Predicates
{
    public interface BrowserSessionPredicate
    {
        bool Satisfied(Session session);
    }
}