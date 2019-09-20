namespace Domain.Models
{
    public class CurrencyPairInfo
    {
        public string XCurrency { get; set; }

        public string YCurrency { get; set; }

        public float BuyCourse { get; set; }

        public float SellCourse { get; set; }

        public float DeltaBuy { get; set; }

        public float DeltaSale { get; set; }
    }
}