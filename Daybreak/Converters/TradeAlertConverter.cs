using Daybreak.Models.Trade;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Reflection.Emit;

namespace Daybreak.Converters;
public sealed class TradeAlertConverter : JsonConverter<ITradeAlert>
{
    public override ITradeAlert? ReadJson(JsonReader reader, Type objectType, ITradeAlert? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader).ToObject<dynamic>();
        if (jObject is null)
        {
            throw new InvalidOperationException($"Unable to deserialize {nameof(ITradeAlert)}");
        }

        if (jObject[nameof(TradeAlert.MessageCheck)] is null)
        {
            return new QuoteAlert
            {
                Name = jObject[nameof(QuoteAlert.Name)],
                Id = jObject[nameof(QuoteAlert.Id)],
                Enabled = jObject[nameof(QuoteAlert.Enabled)],
                UpperPriceTarget = jObject[nameof(QuoteAlert.UpperPriceTarget)],
                LowerPriceTarget = jObject[nameof(QuoteAlert.LowerPriceTarget)],
                UpperPriceTargetEnabled = jObject[nameof(QuoteAlert.UpperPriceTargetEnabled)],
                LowerPriceTargetEnabled = jObject[nameof(QuoteAlert.LowerPriceTargetEnabled)],
                ItemId = jObject[nameof(QuoteAlert.ItemId)]
            };
        }
        else
        {
            return new TradeAlert
            {
                Name = jObject[nameof(TradeAlert.Name)],
                Id = jObject[nameof(TradeAlert.Id)],
                Enabled = jObject[nameof(TradeAlert.Enabled)],
                MessageCheck = jObject[nameof(TradeAlert.MessageCheck)],
                MessageRegexCheck = jObject[nameof(TradeAlert.MessageRegexCheck)],
                SenderCheck = jObject[nameof(TradeAlert.SenderCheck)],
                SenderRegexCheck = jObject[nameof(TradeAlert.SenderRegexCheck)]
            };
        }
    }

    public override void WriteJson(JsonWriter writer, ITradeAlert? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var jObject = new JObject();
        jObject[nameof(ITradeAlert.Name)] = value.Name;
        jObject[nameof(ITradeAlert.Enabled)] = value.Enabled;
        jObject[nameof(ITradeAlert.Id)] = value.Id;
        if (value is TradeAlert tradeAlert)
        {
            jObject[nameof(TradeAlert.MessageCheck)] = tradeAlert.MessageCheck;
            jObject[nameof(TradeAlert.MessageRegexCheck)] = tradeAlert.MessageRegexCheck;
            jObject[nameof(TradeAlert.SenderCheck)] = tradeAlert.SenderCheck;
            jObject[nameof(TradeAlert.SenderRegexCheck)] = tradeAlert.SenderRegexCheck;
        }
        else if (value is QuoteAlert quoteAlert)
        {
            jObject[nameof(QuoteAlert.TraderQuoteType)] = quoteAlert.TraderQuoteType.ToString();
            jObject[nameof(QuoteAlert.ItemId)] = quoteAlert.ItemId;
            jObject[nameof(QuoteAlert.UpperPriceTarget)] = quoteAlert.UpperPriceTarget;
            jObject[nameof(QuoteAlert.UpperPriceTargetEnabled)] = quoteAlert.UpperPriceTargetEnabled;
            jObject[nameof(QuoteAlert.LowerPriceTarget)] = quoteAlert.LowerPriceTarget;
            jObject[nameof(QuoteAlert.LowerPriceTargetEnabled)] = quoteAlert.LowerPriceTargetEnabled;
        }

        serializer.Serialize(writer, jObject);
    }
}
