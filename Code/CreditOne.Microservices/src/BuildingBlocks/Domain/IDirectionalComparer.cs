using System.ComponentModel;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    public interface IDirectionalComparer : System.Collections.IComparer
    {
        /// <summary>
        /// Indicates whether the comparer is directional.
        /// </summary>
        bool IsDirectional { get; }

        /// <summary>
        /// Returns the sort direction of the comparer.
        /// </summary>
        ListSortDirection Direction { get; }
    }
}
