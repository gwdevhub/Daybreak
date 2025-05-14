using Daybreak.Shared.Models.Trade;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.TradeChat;

public interface ITradeAlertingService
{
    IEnumerable<ITradeAlert> TradeAlerts { get; }
    
    void AddTradeAlert(ITradeAlert tradeAlert);
    
    void ModifyTradeAlert(ITradeAlert modifiedTradeAlert);
    
    void DeleteTradeAlert(string tradeAlertId);
}
