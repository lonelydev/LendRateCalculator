using System;
using LendRateCalculatorDataAccessLayer;
using System.Collections.Generic;
using System.Linq;

namespace LendRateCalculator
{
    public class OptimalLendRateCalculator : IOptimalLendRateCalculator, IDisposable
    {
        private ILenderRepository _lenderRepository;

        private int _requestedAmount;
        private List<Lender> _optimalLenders;
        public int RequestedAmount
        {
            get { return _requestedAmount; }
            private set
            {
                if (((value % 100) != 0) || ((value > 15000) || (value < 1000)))
                {
                    throw new InvalidLoanRequest();
                }
                _requestedAmount = value;
            }
        }

        public OptimalLendRateCalculator(ILenderRepository lenderRepository, int requestedAmount)
        {
            _lenderRepository = lenderRepository;
            RequestedAmount = requestedAmount;
        }        

        /// <summary>
        /// Returns optimal lender rate information from appropriate lenderrepostiory
        /// </summary>
        /// <returns></returns>
        public CalculatedOptimalLendRate GetOptimalLendRateInformation()
        {
            _optimalLenders = _lenderRepository.GetOptimalRateLendersForAmount(RequestedAmount).ToList();
            
            //check if we have sufficient to borrow
            var totalAmountLent = _optimalLenders.Sum(orl => orl.AvailableAmount);
            if (totalAmountLent < RequestedAmount)
            {
                throw new InsufficientFundsException();
            }
            var optimalLendRate = CalculateOptimalLendRate();
            return new CalculatedOptimalLendRate(RequestedAmount, optimalLendRate);
        }

        /// <summary>
        /// This method calculates the blended interest rate and returns the result as CalculatedOptimalLendRate object
        /// Blended Interest Rate calculation formula:
        /// ((AvailableAmountFromLender[i] * Rate[i]) + ...) / TotalAmountLent
        /// </summary>
        /// <param name="optimalLenders"></param>
        /// <returns></returns>
        private decimal CalculateOptimalLendRate()
        {
            decimal sumOfProductOfAmountAndInterest = 0m;
            foreach(var optimalLender in _optimalLenders)
            {
                sumOfProductOfAmountAndInterest += (optimalLender.AvailableAmount * optimalLender.Rate);
            }
            return sumOfProductOfAmountAndInterest / RequestedAmount;
        }

        /// <summary>
        /// Dispose the repostory
        /// </summary>
        public void Dispose()
        {
            _lenderRepository.Dispose();            
        }
    }
}
