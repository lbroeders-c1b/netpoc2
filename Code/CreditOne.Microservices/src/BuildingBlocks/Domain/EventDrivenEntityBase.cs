using System.Collections.Generic;

using MediatR;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// Represents the base class for Domain Driven Events
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <description>Description</description>
    /// </listheader>
    /// <item>
    ///     <term>2/4/2020</term>
    ///     <term>Luis Petitjean</term>
    ///     <term>RM-80</term>
    ///     <description>Added a <c>List<MediatR.INotification></c> and methods for managing Domain Events</description>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class EventDrivenEntityBase : EntityBase
    {
        #region Private Members

        private List<INotification> _domainEvents;

        #endregion

        #region Public Properties

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        #endregion

        #region Public Methods

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        #endregion
    }
}
