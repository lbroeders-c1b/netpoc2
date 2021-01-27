using System;
using System.Collections;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    public class ToStringComparer : IComparer
    {
        #region Private Fields

        private StringComparer _comparer = StringComparer.OrdinalIgnoreCase;

        #endregion

        #region IComparer Members

        /// <summary>
        /// Compares two objects based on one of their ToString value and returns a value indicating whether one is less than,
        /// equal to, or greater than the other.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>
        /// An integer value representing the result of the compare.
        /// 1 = x greater than y, 0 = x equal to y, -1 = x less than y.
        /// </returns>
        public int Compare(object x, object y)
        {
            //if only one of x or y are null, then returns -1 
            //if x is null, else 1
            if (x == null ^ y == null)
            {
                if (x == null)
                    return -1;
                else
                    return 1;
            }
            else
            {
                //if x is null, that means they are both null so returns 0
                //because null is equal to null when calling compare
                if (x == null)
                    return 0;
                //neither will be null if we reach this so just compare the ToString values
                return this._comparer.Compare(x.ToString(), y.ToString());
            }
        }

        #endregion
    }
}
