using System.Collections.Generic;


namespace Osls.St.Boolean
{
    /// <summary>
    /// Base class for all boolean expressions
    /// May convert it to an interface
    /// </summary>
    public abstract class BooleanExpression
    {
        /// <summary>
        /// Calculates the result of this boolean expression
        /// </summary>
        public abstract bool Result(IProcessingUnit pu);
        
        /// <summary>
        /// returns true, if this or sub-expressions are valid.
        /// </summary>
        public abstract bool IsValid();
        
        protected static HashSet<string> Union(HashSet<string> a, HashSet<string> b)
        {
            HashSet<string> union = new HashSet<string>(a);
            union.UnionWith(b);
            return union;
        }
    }
}