namespace CreditOne.Microservices.BuildingBlocks.Literals
{
    public class WorkCaseType : Enumeration
    {
        public static WorkCaseType CeaseAndDesist = new WorkCaseType(nameof(CeaseAndDesist), "Cease and Desist");

        public static WorkCaseType LostStolen = new WorkCaseType(nameof(LostStolen), "Lost Stolen");

        public static WorkCaseType FutureAddress = new WorkCaseType(nameof(FutureAddress), "Future Address");

        public static WorkCaseType CardHolderInfo = new WorkCaseType(nameof(CardHolderInfo), "Cardholder Info");

        public WorkCaseType(string id, string name) : base(id, name)
        {
        }
    }
}
