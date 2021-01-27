using System.Collections;
using System.Collections.Generic;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    public class MultiComparer<T> : IComparer, IComparer<T>
    {
        #region Constructors

        /// <summary>
        /// Default constructor that creates a new instance of multicomparer.
        /// </summary>
        public MultiComparer()
        { }

        #endregion

        #region Private Members

        private List<IComparer> _comparers = new List<IComparer>();

        #endregion

        #region IMultiComparer Members

        /// <summary>
        /// Indexer property of MultiComparer.
        /// </summary>
        /// <returns>
        /// The comparer at the specified index.
        /// </returns>
        public IComparer this[int index]
        {
            get
            {
                // Returns the comparer from our comparer array list.
                return this._comparers[index];
            }
        }

        /// <summary>
        /// Adds a comparer.
        /// </summary>
        /// <param name="comparer">The comparer to add.</param>
        public void Add(IComparer comparer)
        {
            // If the MultiComparer does not already contain the comparer, add it.
            if (!(this._comparers.Contains(comparer)))
                this._comparers.Add(comparer);
        }

        #endregion

        #region IDirectionalComparer Members

        /// <summary>
        /// Indicates whether the comparer is directional.  A MultiComparer
        /// is directional if any of the comparers it contains are directional.
        /// </summary>
        public bool IsDirectional
        {
            get
            {
                // Loop through each comparer that this contains.
                foreach (IComparer comparer in this._comparers)
                {
                    // If the comparer is a directional comparer, returns true
                    IDirectionalComparer dc = comparer as IDirectionalComparer;
                    if ((dc != null) && dc.IsDirectional)
                        return true;
                }
                // None of the contained comparers were directional, returns false.
                return false;
            }
        }

        #endregion

        #region IComparer Members

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>
        /// 1 if the first object is greater than the second object.
        /// 0 if the first object is equal to the second object.
        /// -1 if the first object is less than the second object.
        /// </returns>
        public int Compare(object x, object y)
        {
            // Loop through each internal comparer. The first comparer in
            // the array list takes precedence.
            foreach (IComparer comparer in this._comparers)
            {
                // Get the result of the current comparer.
                int result = comparer.Compare(x, y);
                // If the two objects are not equal then returns the result.
                if (result != 0) return result;
            }
            // We've looped through all comparers and each has determined
            // the two objects to be equal, so returns 0.
            return 0;
        }

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>
        /// 1 if the first object is greater than the second object.
        /// 0 if the first object is equal to the second object.
        /// -1 if the first object is less than the second object.
        /// </returns>
        public int Compare(T x, T y)
        {
            return ((IComparer)this).Compare(x, y);
        }

        #endregion
    }
}
