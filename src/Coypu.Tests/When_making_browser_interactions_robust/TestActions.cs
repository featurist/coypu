using System.Threading;
using Coypu.Actions;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class CountTriesAction : DriverAction
    {
        public int Tries { get; private set; }

        public void Act()
        {
            Tries++;
        }
    }

    public class AlwaysTruePredicate : Predicates.Predicate
    {
        public bool Satisfied()
        {
            return true;
        }
    }

    public class AlwaysFalsePredicate : Predicates.Predicate
    {
        public bool Satisfied()
        {
            return false;
        }
    }

    public class AlwaysThrowsPredicate : Predicates.Predicate
    {
        public bool Satisfied()
        {
            Thread.Sleep(Configuration.Timeout);
            throw new MissingHtmlException("From test");
        }
    }

    public class TrueAfterSoManyTriesPredicate : Predicates.Predicate
    {
        private readonly int trueAfterSoManyTries;
        private int tries;

        public TrueAfterSoManyTriesPredicate(int trueAfterSoManyTries)
        {
            this.trueAfterSoManyTries = trueAfterSoManyTries;
        }

        public bool Satisfied()
        {
            tries++;
            return tries >= trueAfterSoManyTries;
        }
    }
}