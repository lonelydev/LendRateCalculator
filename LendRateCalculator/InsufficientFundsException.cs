using System;

namespace LendRateCalculator
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(): base(string.Format("It is not possible to provide a quote at the moment."))
        {
        }
    }
}
