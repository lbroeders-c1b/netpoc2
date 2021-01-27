using System.Linq;
using System.Threading.Tasks;

using MediatR;


namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// Implements the mediator extension class
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///		<term>Date</term>
    ///		<term>Who</term>
    ///		<term>BR/WO</term>
    ///		<description>Description</description>
    /// </listheader>
	/// <item>
	///		<term>4/30/2020</term>
	///		<term>Armando Soto</term>
	///		<term>RM-79</term>
	///		<description>Initial implementation</description>
	/// </item>
    /// </list>
    /// </remarks> 	
    public static class MediatorExtension
    {
        #region Public Methods

        /// <summary>
        /// Publishes the domain events
        /// </summary>
        /// <param name="mediator">Mediator interface</param>
        /// <param name="entity">Domain entity</param>
        /// <returns></returns>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, EventDrivenEntityBase entity)
        {
            if (entity.DomainEvents != null)
            {
                var domainEvents = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();

                foreach (var domainEvent in domainEvents)
                    await mediator.Publish(domainEvent);
            }            
        }

        #endregion
    }


}
