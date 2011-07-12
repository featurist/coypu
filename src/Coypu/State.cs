using System;

namespace Coypu
{
    ///<summary>
    /// A possible state for the current page
    ///</summary>
    public class State 
    {
        private Func<bool> condition;

        public State(Func<bool> condition) 
        {
            this.condition = condition;
        }

        public bool ConditionMet { get; private set; }

        public bool CheckCondition()
        {
            return ConditionMet = condition();
        }
    }
}