using System;

namespace LendRateCalculator
{
    public class InvalidLoanRequest : Exception
    {
        public InvalidLoanRequest() : base(string.Format("Requested loan amount should be in 100 GBP increments between 1000GBP and 15000GBP"))
        {
        }        
    }
}
