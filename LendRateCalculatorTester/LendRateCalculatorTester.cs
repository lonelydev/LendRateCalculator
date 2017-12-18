using NUnit.Framework;
using LendRateCalculator;
using LendRateCalculatorDataAccessLayer;
using FakeItEasy;

namespace LendRateCalculatorTester
{
    public class LendRateCalculatorTester
    {
        [Test]
        public void Test1_CalculatLendRate()
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            var requestAmount = 1100;
            var optimalLendRateCalculator = OptimalLendRateCalculatorFactory.GetOptimalLendRateCalculator(filePath, requestAmount);
            var oRate = optimalLendRateCalculator.GetOptimalLendRateInformation();
            Assert.IsNotNull(oRate);
            Assert.AreEqual(oRate.OptimumLendRate, oRate.OptimumLendRate);
        }

        [TestCase(1101)]
        [TestCase(999)]
        [TestCase(999)]
        [TestCase(15001)]
        public void Test2_InvalidRequestAmount(int requestAmount)
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            Assert.Throws<InvalidLoanRequest>(
                () => OptimalLendRateCalculatorFactory.GetOptimalLendRateCalculator(filePath, requestAmount),
                string.Format("Requested amount: {0} , should throw an InvalidLoanRequest exception!", requestAmount));
        }

        [TestCase(1000)]
        [TestCase(15000)]
        public void Test3_ValidRequestAmount_EqualToValue(int requestAmount)
        {
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            Assert.DoesNotThrow(
                () => OptimalLendRateCalculatorFactory.GetOptimalLendRateCalculator(filePath, requestAmount),
                string.Format("Requested amount: {0} , should not throw an InvalidLoanRequest exception!", requestAmount));
        }

        [Test, Description(@"Check if output string matches format")]
        public void Test4_CalculatedOptimalLendRate()
        {
            var requestAmount = 1100;
            var filePath = TestContext.CurrentContext.TestDirectory + @"\TestData\test_data_1.csv";
            var calculator = OptimalLendRateCalculatorFactory.GetOptimalLendRateCalculator(filePath, requestAmount);
            var oRate = calculator.GetOptimalLendRateInformation();
            Assert.IsNotNull(oRate);
            var outputString = "Rate: 7.0%\nMonthly repayment: £37.73\nTotal repayment: £1,358.21\n";
            Assert.AreEqual(outputString, oRate.ToString(), "ToString doesn't seem right!");
        }

        [Test, Description(@"FakeItEasy to pass a fake ILenderRepository")]
        public void Test5_CalculatedOptimalLendRateForFakeRepository()
        {
            var fakeLenderRepository = A.Fake<ILenderRepository>();
            var fakeListOfLenders = new System.Collections.Generic.List<Lender>
            {
                new Lender { Name = "first", Rate = 0.067m, AvailableAmount = 980m},
                new Lender { Name = "second", Rate = 0.052m, AvailableAmount = 300m},

            };
            A.CallTo(() => fakeLenderRepository.GetLenders()).Returns(fakeListOfLenders);
            A.CallTo(() => fakeLenderRepository.GetOptimalRateLendersForAmount(1000)).Returns(fakeListOfLenders);
            var optimalLendRateCalculator = new OptimalLendRateCalculator(fakeLenderRepository, 1000);
            var oRate = optimalLendRateCalculator.GetOptimalLendRateInformation();
            Assert.IsNotNull(oRate);
            var outputString = "Rate: 8.1%\nMonthly repayment: £35.42\nTotal repayment: £1,275.02\n";
            Assert.AreEqual(outputString, oRate.ToString());
        }
    }
}
