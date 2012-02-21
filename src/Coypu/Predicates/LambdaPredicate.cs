using System;

namespace Coypu.Predicates
{
    internal class LambdaPredicate : Predicate
    {
        private readonly Func<bool> predicate;

        public LambdaPredicate(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public bool Satisfied()
        {
            return predicate();
        }
    }
}