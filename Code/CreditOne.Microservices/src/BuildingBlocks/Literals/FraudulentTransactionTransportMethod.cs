namespace CreditOne.Microservices.BuildingBlocks.Literals
{
    /// <summary>
    /// Describes the transport method that should be used for a fraudulent transaction.
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
    ///		<term>03-11-2020</term>
    ///		<term>Jonatan Marquez</term>
    ///		<term>RM-80</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class FraudulentTransactionTransportMethod : Enumeration
    {
        public static FraudulentTransactionTransportMethod CDSXCall = new FraudulentTransactionTransportMethod(nameof(CDSXCall), "CDSXCall");

        public static FraudulentTransactionTransportMethod Adjustments = new FraudulentTransactionTransportMethod(nameof(Adjustments), "Adjustments");

        public FraudulentTransactionTransportMethod(string id, string name) : base(id, name)
        {
        }
    }
}
