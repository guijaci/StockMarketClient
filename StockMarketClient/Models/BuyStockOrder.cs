namespace StockMarketClient.Models
{
    public class BuyStockOrder:StockOrder
    {
        public override bool IsBuying { get => true; }
        public override bool IsSelling { get => false; }
    }
}
