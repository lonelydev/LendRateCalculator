namespace LendRateCalculatorDataAccessLayer
{
    /// <summary>
    /// Lender record as read from the CSV file
    /// </summary>
    public class Lender
    {
        public string Name { get; set; }

        public decimal Rate { get; set; }

        public decimal AvailableAmount { get; set; }
    }
}
