using System;

namespace LendRateCalculator
{
    public class CompoundInterestCalculator
    {
        private int _compoundingPeriodsPerYear;
        private double _yearCount;
        private decimal _principal;
        private decimal _interestRate;

        public CompoundInterestCalculator(int compoundingPeriodsPerYear, double yearCount, decimal principal, decimal interestRate)
        {
            _compoundingPeriodsPerYear = compoundingPeriodsPerYear;
            _yearCount = yearCount;
            _principal = principal;
            _interestRate = interestRate;
        }

        public decimal GetPrincipalWithCompoundedInterest()
        {

            return _principal * (decimal)Math.Pow(
                (double)(1 + _interestRate / _compoundingPeriodsPerYear), 
                _compoundingPeriodsPerYear * _yearCount);
        }
    }
}
