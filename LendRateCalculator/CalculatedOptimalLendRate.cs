using System.Text;
using System.Globalization;

namespace LendRateCalculator
{
    public class CalculatedOptimalLendRate
    {
        private CompoundInterestCalculator _compounder;
        private int _compoundingPeriodsPerYear;
        private int _numberOfYears;
        private decimal _requestedAmount;
        private decimal _optimalLendRate;
        private string _cultureInfoName;

        public CalculatedOptimalLendRate(decimal requestedAmount, decimal lendRate,
            int compoundingPeriodsPerYear = 12, int numberOfYears = 3, string cultureInfoName = "en-GB")
        {
            _requestedAmount = requestedAmount;
            _optimalLendRate = lendRate;
            _compoundingPeriodsPerYear = compoundingPeriodsPerYear;
            _numberOfYears = numberOfYears;
            _cultureInfoName = cultureInfoName;
            _compounder = new CompoundInterestCalculator(compoundingPeriodsPerYear, numberOfYears, requestedAmount, lendRate);
        }

        public decimal RequestedAmount { get { return _requestedAmount; } }

        public decimal OptimumLendRate { get { return _optimalLendRate; } }

        public decimal TotalRepayment { get { return _compounder.GetPrincipalWithCompoundedInterest(); } }

        public int Period { get { return _compoundingPeriodsPerYear * _numberOfYears; } }

        public override string ToString()
        {
            var rateFormatInfo = new CultureInfo(_cultureInfoName, false).NumberFormat;
            rateFormatInfo.CurrencyDecimalDigits = 2;
            var stringRepresentation = new StringBuilder();
            stringRepresentation.Append(string.Format("Rate: {0}\n", OptimumLendRate.ToString("P1", rateFormatInfo)));
            stringRepresentation.Append(string.Format("Monthly repayment: {0}\n", (TotalRepayment / Period).ToString("C", rateFormatInfo)));
            stringRepresentation.Append(string.Format("Total repayment: {0}\n", TotalRepayment.ToString("C", rateFormatInfo)));
            return stringRepresentation.ToString();
        }
    }
}
