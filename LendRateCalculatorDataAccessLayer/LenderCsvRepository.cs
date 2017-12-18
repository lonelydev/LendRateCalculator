using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace LendRateCalculatorDataAccessLayer
{
    public class LenderCsvRepository : ILenderRepository
    {
        private string _lenderFilePath;        
        
        public LenderCsvRepository(string lenderCsvFilePath)
        {
            if (!File.Exists(lenderCsvFilePath))
                throw new FileNotFoundException("The lender data file does not exist!", lenderCsvFilePath);

            _lenderFilePath = lenderCsvFilePath;
        }        

        public IEnumerable<Lender> GetLenders()
        {
            using (var sReader = new StreamReader(_lenderFilePath))
            {
                var csvReader = new CsvHelper.CsvReader(sReader);
                csvReader.Configuration.RegisterClassMap<LenderMap>();
                return csvReader.GetRecords<Lender>().ToList();
            }            
        }

        /// <summary>
        /// Get the lenders that provide the best rate for the borrower
        /// Sort in ascending order of Rates and then get borrowing till you get the whole amount
        /// </summary>
        /// <param name="requestedAmount"></param>
        /// <returns></returns>
        public IEnumerable<Lender> GetOptimalRateLendersForAmount(decimal requestedAmount)
        {
            var lenders = GetLenders().OrderBy(lender => lender.Rate).ToList();
            var finalListOfLenders = new List<Lender>();
            var lenderIndex = 0;
            decimal sumOfAmountBorrowed = 0m;
            decimal remainingAmount = requestedAmount;
            while (lenderIndex < lenders.Count && remainingAmount >= 0 && sumOfAmountBorrowed < requestedAmount)
            {
                var lender = lenders[lenderIndex++];
                decimal amountLent;
                if (lender.AvailableAmount <= remainingAmount)
                {
                    amountLent = lender.AvailableAmount;
                }
                else
                {
                    amountLent = remainingAmount;
                }
                sumOfAmountBorrowed += amountLent;
                finalListOfLenders.Add(new Lender { Name = lender.Name, Rate = lender.Rate, AvailableAmount = amountLent });
                remainingAmount = requestedAmount - sumOfAmountBorrowed;                
            }
            return finalListOfLenders;
        }

        //this region is auto generated using vs2015
        //as I don't have streamreader as a member property 
        //i am not disposing it
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~LenderCsvRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }        
        #endregion
    }
}
