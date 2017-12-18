using NUnit.Framework;
using LendRateCalculatorDataAccessLayer;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;

namespace LendRateCalculatorDalTester
{
    /// <summary>
    /// As long as you have nUnit Test Adapter you can run tests easily from Visual Studio. 
    /// Else command line is your friend
    /// 
    /// with unit3 - which i am using for the first time in this project, the current working
    /// directory for the extension is the directory from where the nunit3-console is executed
    /// https://github.com/nunit/nunit3-vs-adapter/issues/96
    /// https://github.com/nunit/nunit/issues/1072
    /// even tried using runsettings thinking that would help: 
    /// https://msdn.microsoft.com/en-us/library/jj635153.aspx
    /// Example from nunit3 runsettings - https://github.com/nunit/docs/wiki/Tips-And-Tricks and below
    /// https://github.com/nunit/nunit3-vs-adapter/blob/8a9b8a38b7f808a4a78598542ddaf557950c6790/demo/demo.runsettings
    /// The solution was to use TestContext.CurrentContext.TestDirectory
    /// Wasted a couple of hours debugging to understand why it would not pick the right path earlier.
    /// </summary>
    public class LenderCsvRepositoryTester
    {
        [Test]        
        public void Test1_TestNonExistantFile()
        {
            Assert.Throws<FileNotFoundException>(() => new LenderCsvRepository("Non_existant_file.csv"));            
        }       

        [Test]
        public void Test2_TestEmptyFileName()
        {
            Assert.Throws<FileNotFoundException>(() => new LenderCsvRepository(""));
        }

        [Test]
        public void Test3_TestExistingFile()
        {            
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            Assert.DoesNotThrow(() => new LenderCsvRepository(filePath));
        }

        [Test]
        public void Test4_GetAllLenders()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            using (var lenderCsvR = new LenderCsvRepository(filePath))
            {
                var allLenders = lenderCsvR.GetLenders().ToList();
                Assert.IsNotNull(allLenders,"allLenders must not be null");
                Assert.AreEqual(5, allLenders.Count, "Expected number of lenders do not match!");
            }
        }

        [Test]
        public void Test5_GetLendersForAmount()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            using (var lenderCsvR = new LenderCsvRepository(filePath))
            {
                var allLenders = lenderCsvR.GetOptimalRateLendersForAmount(600).ToList();
                Assert.IsInstanceOf<List<Lender>>(allLenders, "Returned list doesn't match type!");
                Assert.IsNotNull(allLenders, "allLenders must not be null");                
                //Expected result should look something like this: 
                // Jane,0.069,480
                // Fred,0.071,120
                Assert.AreEqual(2, allLenders.Count, "Expected number of lenders do not match!");
                Assert.AreEqual("Jane", allLenders[0].Name, "Jane isn't the expected first lender!");
                Assert.AreEqual("Fred", allLenders[1].Name, "Fred isn't the expected second lender!");
            }
        }

        [Test]
        public void Test6_GetLendersForAmountInsufficientLenders()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_2.csv";
            using (var lenderCsvR = new LenderCsvRepository(filePath))
            {
                var allLenders = lenderCsvR.GetOptimalRateLendersForAmount(1000).ToList();
                Assert.IsInstanceOf<List<Lender>>(allLenders, "Returned list doesn't match type!");
                Assert.IsNotNull(allLenders, "allLenders must not be null");
                //Expected result should look something like this: 
                // Jane,0.069,480
                // Mary,0.104,100
                Assert.AreEqual(2, allLenders.Count, "Expected number of lenders do not match!");
                Assert.AreEqual("Jane", allLenders[0].Name, "Jane isn't the expected first lender!");
                Assert.AreEqual("Mary", allLenders[1].Name, "Mary isn't the expected second lender!");
            }
        }

        [Test]
        public void Test7_GetLendersForAmountSufficientLenders_1100()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            using (var lenderCsvR = new LenderCsvRepository(filePath))
            {
                var allLenders = lenderCsvR.GetOptimalRateLendersForAmount(1100).ToList();
                Assert.IsInstanceOf<List<Lender>>(allLenders, "Returned list doesn't match type!");
                Assert.IsNotNull(allLenders, "allLenders must not be null");
                //Expected result should look something like this: 
                // Jane,0.069,480
                // Fred,0.071,520
                // Bob, 0.075, 100
                Assert.AreEqual(3, allLenders.Count, "Expected number of lenders do not match!");
                Assert.AreEqual("Jane", allLenders[0].Name, "Jane isn't the expected first lender!");
                Assert.AreEqual("Fred", allLenders[1].Name, "Fred isn't the expected second lender!");
                Assert.AreEqual("Bob", allLenders[2].Name, "Bob isn't the expected third lender!");
            }
        }

        [Test]
        public void Test8_TestNonExistantFileUsingRepositoryFactory()
        {
            Assert.Throws<FileNotFoundException>(
                () => LenderRepositoryFactory.GetRepositoryInstance("Non_existant_file.csv", RepositoryType.Csv));
        }

        [Test]
        public void Test9_TestEmptyFileNameUsingRepositoryFactory()
        {
            Assert.Throws<FileNotFoundException>(() => LenderRepositoryFactory.GetRepositoryInstance(""));
        }

        [Test]
        public void Test10_TestInvalidFileNameUsingRepositoryFactory()
        {
            Assert.Throws<FileNotFoundException>(
                () => LenderRepositoryFactory.GetRepositoryInstance(@"data source=eakanpc;multisubnetfailover=true"));
        }

        [Test]
        public void Test11_TestFileWithHeaderOnlyUsingRepositoryFactory()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_3.csv";
            var lenderRepository = LenderRepositoryFactory.GetRepositoryInstance(filePath);
            Assert.IsNotNull(lenderRepository);
            var lenders = lenderRepository.GetLenders();
            Assert.IsNotNull(lenders);
            var expectedCount = 0;
            Assert.AreEqual(expectedCount, lenders.Count(), string.Format("Expected {0} elements from the file with just a header!", expectedCount));
        }

        [Test, Description(@"Extra columns do not matter so long as the columns in LenderMap is present in the file")]
        public void Test12_TestFileWithExtraColumnUsingRepositoryFactory()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_4.csv";
            var lenderRepository = LenderRepositoryFactory.GetRepositoryInstance(filePath);
            Assert.IsNotNull(lenderRepository);
            var lenders = lenderRepository.GetLenders();
            Assert.IsNotNull(lenders);
            var expectedCount = 2;
            Assert.AreEqual(expectedCount, lenders.Count(), string.Format("Expected {0} elements from the file with just a header!", expectedCount));
        }

        [Test, Description(@"Column header with typo throws validation exception")]
        public void Test13_TestFileWithColumnHeaderThatHasTypoUsingRepositoryFactory()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_5.csv";
            var lenderRepository = LenderRepositoryFactory.GetRepositoryInstance(filePath);
            Assert.IsNotNull(lenderRepository);
            Assert.Throws<ValidationException>(() => lenderRepository.GetLenders());
        }
    }
}
