using LendRateCalculatorDataAccessLayer;

namespace LendRateCalculator
{
    public class OptimalLendRateCalculatorFactory
    {
        /// <summary>
        /// Creates an instance of IOptimalLendRateCalculator 
        /// </summary>
        /// <param name="connectionStringOrFileName"></param>
        /// <param name="requestedAmount"></param>
        /// <returns></returns>
        public static IOptimalLendRateCalculator GetOptimalLendRateCalculator(string connectionStringOrFileName, int requestedAmount)
        {            
            ILenderRepository repository = LenderRepositoryFactory.GetRepositoryInstance(connectionStringOrFileName);
            return new OptimalLendRateCalculator(repository, requestedAmount);
        }        
    }
}
