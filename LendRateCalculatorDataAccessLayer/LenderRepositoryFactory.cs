using System;
using System.IO;

namespace LendRateCalculatorDataAccessLayer
{
    public enum RepositoryType
    {
        Csv
    };

    public class LenderRepositoryFactory
    {
        /// <summary>
        /// Handle different types of LenderRepository Creation in here. 
        /// Currently supports creation of CSV version
        /// </summary>
        /// <param name="connectionStringOrFileName"></param>
        /// <param name="rType"></param>
        /// <returns></returns>
        public static ILenderRepository GetRepositoryInstance(string connectionStringOrFileName, RepositoryType rType)
        {
            switch (rType)
            {
                case RepositoryType.Csv:
                    return new LenderCsvRepository(connectionStringOrFileName);
                default:
                    return new LenderCsvRepository(connectionStringOrFileName);
            }
        }

        public static ILenderRepository GetRepositoryInstance(string connectionStringOrFileName)
        {
            bool isPathToFile = connectionStringOrFileName.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            ILenderRepository repository;
            if (isPathToFile)
            {
                repository = GetRepositoryInstance(connectionStringOrFileName, RepositoryType.Csv);
                return repository;
            }
            throw new NotImplementedException();
        }
    }
}
