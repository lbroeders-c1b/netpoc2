
namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// Delegate definition for the ItemPropertyChangedEventHandler.
    /// </summary>
    /// <typeparam name="T">The type of the item whose property value has changed.</typeparam>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ItemPropertyChangedEventHandler<T>(object sender, ItemPropertyChangedEventArgs<T> e);
}
