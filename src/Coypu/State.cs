using System;

namespace Coypu
{
    ///<summary>
    /// A possible state for the current page
    ///</summary>
    public class State 
    {
        private Func<bool> condition;


        ///<summary>
        /// Describe a possible state for the page with a condition to identify this state.
        ///</summary>
        ///<param name="condition">How to identify this state</param>
        public State(Func<bool> condition) 
        {
            this.condition = condition;
        }

        internal bool ConditionWasMet { get; private set; }

        internal bool CheckCondition()
        {
            return ConditionWasMet = condition();
        }
    }
}