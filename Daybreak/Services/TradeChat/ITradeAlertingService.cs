using Daybreak.Models.Trade;
using System.Collections.Generic;

namespace Daybreak.Services.TradeChat;

public interface ITradeAlertingService
{
    IEnumerable<TradeAlert> TradeAlerts { get; }
    
    void AddTradeAlert(TradeAlert tradeAlert);
    
    void ModifyTradeAlert(TradeAlert modifiedTradeAlert);
    
    void DeleteTradeAlert(string tradeAlertId);
}
