namespace StockMarketClient.Models
{
    /// <summary>
    /// Objeto representa uma ordem de compra de ações no mercado de ações
    /// </summary>
    public class BuyStockOrder:StockOrder
    {
        public override bool IsBuying { get => true; }
        public override bool IsSelling { get => false; }
    }
}
