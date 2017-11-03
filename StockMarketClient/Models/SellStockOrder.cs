namespace StockMarketClient.Models
{
    /// <summary>
    /// Objeto representa uma ordem de venda de ações no mercado de ações
    /// </summary>
    public class SellStockOrder:StockOrder
    {
        public override bool IsBuying { get => false; }
        public override bool IsSelling { get => true; }
    }
}
