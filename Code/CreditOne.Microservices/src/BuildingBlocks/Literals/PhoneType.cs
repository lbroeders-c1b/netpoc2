namespace CreditOne.Microservices.BuildingBlocks.Literals
{
    public class PhoneType : Enumeration
    {
        public static PhoneType HomePhone = new PhoneType(nameof(HomePhone), "HOME");

        public static PhoneType MobilePhone = new PhoneType(nameof(MobilePhone), "MOBILE");

        public static PhoneType WorkPhone = new PhoneType(nameof(WorkPhone), "WORK");

        public PhoneType(string id, string name) : base(id, name)
        {
        }
    }
}
