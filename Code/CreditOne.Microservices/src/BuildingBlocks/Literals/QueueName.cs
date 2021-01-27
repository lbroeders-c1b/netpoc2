namespace CreditOne.Microservices.BuildingBlocks.Literals
{	
    public class QueueName : Enumeration
    {
        public static QueueName FutureAddress = new QueueName(nameof(FutureAddress), "Future Address");

        public QueueName(string id, string name) : base(id, name)
        {
        }
    }
}
