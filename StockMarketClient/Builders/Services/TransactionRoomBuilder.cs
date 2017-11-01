using StockMarketClient.Services;
using System.Timers;

namespace StockMarketClient.Builders.Services
{
    static class TransactionRoomBuilder
    {
        static public TransactionRoomFacade WithStockMarketService(this TransactionRoomFacade transactionRoom, StockMarketService stockMarketService)
        {
            transactionRoom.StockMarketService = stockMarketService;
            return transactionRoom;
        }

        static public TransactionRoomFacade WithEventPollTimer(this TransactionRoomFacade transactionRoom, Timer pollEventTimer)
        {
            transactionRoom.EventPollTimer = pollEventTimer;
            return transactionRoom;
        }
    }
}
