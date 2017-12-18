using LendRateCalculator;
using System;

namespace LendRateCalculatorConsole
{
    public class LendRateConsole
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine(UsageMessage);
                Environment.Exit(1);
            }
            int requestedAmount;
            int.TryParse(args[1], out requestedAmount);
            try
            {
                var stringFileName = args[0];
                var lendRateCalculator = OptimalLendRateCalculatorFactory.GetOptimalLendRateCalculator(stringFileName, requestedAmount);                
                var calculatedRate = lendRateCalculator.GetOptimalLendRateInformation();
                Console.WriteLine(calculatedRate.ToString());
            }
            catch (InvalidLoanRequest ilr)
            {
                Console.WriteLine(ilr.Message);
            }
            catch (InsufficientFundsException isfe)
            {
                Console.WriteLine(isfe.Message);
            }
        }

        private static string UsageMessage
        {
            get
            {
                return string.Format("Usage:\n LendRateConsole.exe <lenderDataCsvFileAbsolutePath> <borrowAmount>\n");
            }
        }
    }
}
