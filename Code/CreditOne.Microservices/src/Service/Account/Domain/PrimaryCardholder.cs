using System;

namespace CreditOne.Microservices.Account.Domain
{
    /// <summary>
    /// Implements the primary card holder domain class
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
    ///		<term>5/29/2019</term>
    ///		<term>Christian Azula</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    ///	</item>
    /// </list>
    /// </remarks> 
    public class PrimaryCardholder : CardholderBase
    {
        public decimal CustomerId { get; set; }

        public string CreditBureauReportingFlag { get; set; }

        public bool IsCreditBureauReportingTriggered
        {
            get { return CreditBureauReportingFlag == "0" || CreditBureauReportingFlag == "1" || CreditBureauReportingFlag == "2"; }
        }

        public bool HasBirthdayToday
        {
            get
            {
                if (DateOfBirth != null && DateOfBirth.HasValue)
                {
                    if (DateOfBirth.Value.Month == DateTime.Today.Month &&
                        DateOfBirth.Value.Day == DateTime.Today.Day)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
