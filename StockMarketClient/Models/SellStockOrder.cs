namespace StockMarketClient.Models
{
    public class SellStockOrder:StockOrder
    {
        public override bool IsBuying { get => false; }
        public override bool IsSelling { get => true; }
    }
}
