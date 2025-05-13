using Daybreak.Models.Trade;
using System.Collections.Generic;

namespace Daybreak.Services.TradeChat;

public interface ITradeAlertingService
{
    IEnumerable<ITradeAlert> TradeAlerts { get; }
    
    void AddTradeAlert(ITradeAlert tradeAlert);
    
    void ModifyTradeAlert(ITradeAlert modifiedTradeAlert);
    
    void DeleteTradeAlert(string tradeAlertId);
}
