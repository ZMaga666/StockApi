namespace StockApi.Model
{
    public class Stock
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Price { get; set; }
        public string PercentageOfChange { get; set; }
        public string ChangePrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
