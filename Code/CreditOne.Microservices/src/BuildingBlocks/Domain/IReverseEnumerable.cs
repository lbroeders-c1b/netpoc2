using System.Collections.Generic;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// Defines a collection type that knows how to
    /// navigate itself in reverse order.
    /// </summary>
    public interface IReverseEnumerable<T>
    {
        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// of items in reverse order.
        /// </summary>
        IEnumerable<T> ReverseEnumerator { get; }
    }
}
