using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    public class PropertyComparer<T> : IDirectionalComparer, IComparer<T>
    {
        #region Private Members

        private PropertyDescriptor _property;
        private IComparer _comparer;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to create a PropertyComparer
        /// </summary>
        /// <param name="property">The PropertyDescriptor of the property to sort</param>
        /// <param name="comparer">The comparer to to be used for comparing the property values.</param>
        public PropertyComparer(PropertyDescriptor property, IComparer comparer)
            : this(property, comparer, ListSortDirection.Ascending)
        { }

        /// <summary>
        /// Constructor to create a PropertyComparer
        /// </summary>
        /// <param name="property">The PropertyDescriptor of the property to sort</param>
        /// <param name="comparer">The comparer to to be used for comparing the property values.</param>
        /// <param name="direction">Sort direction to be applied.</param>
        public PropertyComparer(PropertyDescriptor property, IComparer comparer, ListSortDirection direction)
        {
            //Validate parameters.
            if (property == null) throw new ArgumentNullException("property");
            if (comparer == null) throw new ArgumentNullException("comparer");

            //Set field values.
            this._property = property;
            this._comparer = comparer;
            this.Direction = direction;
        }

        #endregion

        #region IDirectionalComparer Members

        /// <summary>
        /// Indicates whether the comparer is directional. Always returns true
        /// because all property comparers are directional.
        /// </summary>
        public bool IsDirectional
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the sort direction of the comparer.
        /// </summary>
        public ListSortDirection Direction { get; }

        #endregion

        #region IComparer Members

        /// <summary>
        /// Compares two objects based on one of their properties and returns a value indicating whether one is less than,
        /// equal to, or greater than the other.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>
        /// An integer value representing the result of the compare.
        /// If the sort direction is ascending then: 1 = x greater than y, 0 = x equal to y, -1 = x less than y.
        /// If the sort direction is descending then: 1 = x less than y, 0 = x equal to y, -1 = x greater than y.
        /// </returns>
        public int Compare(object x, object y)
        {
            // Compare the values of the compare property of the two objects.
            int result = this._comparer.Compare(this._property.GetValue(x), this._property.GetValue(y));
            // If the sort direction is descending, we need to multiple the
            // result by -1.
            if (this.Direction == ListSortDirection.Descending) result *= -1;
            // Returns the result
            return result;
        }

        /// <summary>
        /// Compares two objects of type T based on one of their properties and returns a value indicating whether one is less than,
        /// equal to, or greater than the other.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>
        /// An integer value representing the result of the compare.
        /// If the sort direction is ascending then: 1 = x greater than y, 0 = x equal to y, -1 = x less than y.
        /// If the sort direction is descending then: 1 = x less than y, 0 = x equal to y, -1 = x greater than y.
        /// </returns>
        public int Compare(T x, T y)
        {
            return ((IComparer)this).Compare(x, y);
        }

        #endregion

        #region Equals / Hasttable Override

        /// <summary>
        /// Indicates whether an object is equal to this property comparer.
        /// </summary>
        /// <param name="obj">The object to compare against.</param>
        /// <returns>True if the object equals the current property comparer instance.</returns>
        public override bool Equals(object obj)
        {
            PropertyComparer<T> obj2 = obj as PropertyComparer<T>;
            if (obj2 != null)
            {
                return this._property.Equals(obj2._property) && this.Direction.Equals(obj2.Direction) && this._comparer.Equals(obj2._comparer);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the hash code of the property comparer instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
