namespace Services.Models
{
    public class Stock
    {
        public string Symbol { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public decimal CurrentPrice { get; set; }
        public decimal ChangePercentage { get; set; }
        public string LastUpdated { get; set; } = "";
    }
}
