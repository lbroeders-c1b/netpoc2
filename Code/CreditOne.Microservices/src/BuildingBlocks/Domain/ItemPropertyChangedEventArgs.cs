using System.ComponentModel;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    public class ItemPropertyChangedEventArgs<T> : PropertyChangedEventArgs
    {

        #region Private Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ItemPropertyChangedEventArgs instance.
        /// </summary>
        /// <param name="item">The item of the collection whose property value has changed.</param>
        /// <param name="propertyName">The name of property whose value has changed.</param>
        public ItemPropertyChangedEventArgs(T item, string propertyName)
            : base(propertyName)
        {
            this.ChangedItem = item;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The item of the collection whose property value has changed.
        /// </summary>
        public T ChangedItem { get; }

        #endregion
    }
}
