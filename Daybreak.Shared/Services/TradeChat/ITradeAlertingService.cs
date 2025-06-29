using Daybreak.Shared.Models.Trade;

namespace Daybreak.Shared.Services.TradeChat;

public interface ITradeAlertingService
{
    IEnumerable<ITradeAlert> TradeAlerts { get; }
    
    void AddTradeAlert(ITradeAlert tradeAlert);
    
    void ModifyTradeAlert(ITradeAlert modifiedTradeAlert);
    
    void DeleteTradeAlert(string tradeAlertId);
}
