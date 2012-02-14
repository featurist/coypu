using System;

namespace Coypu.Predicates
{
    internal class LambdaPredicate : BrowserSessionPredicate
    {
        private readonly Func<bool> predicate;

        public LambdaPredicate(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public bool Satisfied(Session session)
        {
            return predicate();
        }
    }
}