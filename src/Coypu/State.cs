using Coypu.Queries;

namespace Coypu
{
    ///<summary>
    /// A possible state for the current page
    ///</summary>
    public class State 
    {
        private readonly Query<bool> condition;

        ///<summary>
        /// Describe a possible state for the page with a condition to identify this state.
        ///</summary>
        ///<param name="condition">How to identify this state</param>
        public State(Query<bool> condition) 
        {
            this.condition = condition;
        }

        internal bool ConditionWasMet { get; private set; }

        internal bool CheckCondition()
        {
            condition.Run();
            return ConditionWasMet = condition.Result;
        }
    }
}