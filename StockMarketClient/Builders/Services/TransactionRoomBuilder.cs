using StockMarketClient.Services;

namespace StockMarketClient.Builders.Services
{
    static class TransactionRoomBuilder
    {
        static public TransactionRoomFacade WithStockMarketService(this TransactionRoomFacade transactionRoom, StockMarketService stockMarketService)
        {
            transactionRoom.StockMarketService = stockMarketService;
            return transactionRoom;
        }
    }
}
