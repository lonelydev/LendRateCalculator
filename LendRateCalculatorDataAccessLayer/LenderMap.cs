namespace LendRateCalculatorDataAccessLayer
{
    public class LenderMap : CsvHelper.Configuration.ClassMap<Lender>
    {
        public LenderMap()
        {
            //AutoMap();
            Map(m => m.Name).Name("Lender");
            Map(m => m.Rate).Name("Rate");
            Map(m => m.AvailableAmount).Name("Available");
        }
    }
}
