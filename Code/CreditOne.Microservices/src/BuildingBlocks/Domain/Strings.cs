
namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    public static class Strings
    {
        #region User Friendly Messages

        public static class UserFriendlyMessages
        {
            public static readonly string UnableToSaveInvalidBusinessObject = @"We were unable to complete the Save request.";
            public static readonly string UnableToSaveInvalidBusinessObjectCollection = @"We were unable to complete the Save request.";
            public static readonly string UnableToLoadPrimaryCardholder = @"We were unable to load the primary cardholder.";
            public static readonly string UnableToLoadSecondaryCardholder = @"We were unable to load the secondary cardholder.";
        }

        #endregion


        #region Exception Messages

        public static class ExceptionMessages
        {
            public static readonly string CodingStandardsViolation = @"Credit One Middleware coding pattarn has been violated.  Please review the coding pattern or seek assistance for further understanding.";
            public static readonly string ExecuteCommandError = @"There was a problem while executing a database command.";
        }

        #endregion
    }
}
