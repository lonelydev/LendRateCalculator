using System;
using System.Collections.Generic;

namespace LendRateCalculatorDataAccessLayer
{
    public interface ILenderRepository : IDisposable
    {
        IEnumerable<Lender> GetLenders();
        IEnumerable<Lender> GetOptimalRateLendersForAmount(decimal requestedAmount);
    }
}
