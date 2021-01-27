namespace CreditOne.Microservices.BuildingBlocks.Literals
{
    public class WorkCaseStatus : Enumeration
    {
        public static WorkCaseStatus Canceled = new WorkCaseStatus(nameof(Canceled), "CANCELED");

        public static WorkCaseStatus ErrorClosed = new WorkCaseStatus(nameof(ErrorClosed), "ERRCLOSED");

        public static WorkCaseStatus Open = new WorkCaseStatus(nameof(Open), "OPEN");

        public static WorkCaseStatus Pending = new WorkCaseStatus(nameof(Pending), "PEND");

        public static WorkCaseStatus ResolvedClosed = new WorkCaseStatus(nameof(ResolvedClosed), "RCLOSED");

        public WorkCaseStatus(string id, string name) : base(id, name)
        {
        }
    }
}
