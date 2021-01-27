using System;

namespace CreditOne.Microservices.BuildingBlocks.EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        public class SubscriptionInfo
        {
            #region Public Properties

            public bool IsDynamic { get; }

            public Type HandlerType{ get; }

            #endregion

            #region Constructors

            private SubscriptionInfo(bool isDynamic, Type handlerType)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
            }

            #endregion

            #region Public Methods

            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }

            public static SubscriptionInfo Typed(Type handlerType)
            {
                return new SubscriptionInfo(false, handlerType);
            }

            #endregion
        }
    }
}
