using Daybreak.Shared.Models.Trade;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Daybreak.Shared.Converters;
public sealed class TradeAlertConverter : JsonConverter<ITradeAlert>
{
    public override ITradeAlert? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        // Discriminate based on presence of MessageCheck property
        if (root.TryGetProperty(nameof(TradeAlert.MessageCheck), out _))
        {
            return new TradeAlert
            {
                Name = root.GetProperty(nameof(TradeAlert.Name)).GetString() ?? string.Empty,
                Id = root.GetProperty(nameof(TradeAlert.Id)).GetString() ?? string.Empty,
                Enabled = root.GetProperty(nameof(TradeAlert.Enabled)).GetBoolean(),
                MessageCheck = root.TryGetProperty(nameof(TradeAlert.MessageCheck), out var msgCheck) ? msgCheck.GetString() : null,
                MessageRegexCheck = root.TryGetProperty(nameof(TradeAlert.MessageRegexCheck), out var msgRegex) && msgRegex.GetBoolean(),
                SenderCheck = root.TryGetProperty(nameof(TradeAlert.SenderCheck), out var senderCheck) ? senderCheck.GetString() : null,
                SenderRegexCheck = root.TryGetProperty(nameof(TradeAlert.SenderRegexCheck), out var senderRegex) && senderRegex.GetBoolean()
            };
        }
        else
        {
            return new QuoteAlert
            {
                Name = root.GetProperty(nameof(QuoteAlert.Name)).GetString() ?? string.Empty,
                Id = root.GetProperty(nameof(QuoteAlert.Id)).GetString() ?? string.Empty,
                Enabled = root.GetProperty(nameof(QuoteAlert.Enabled)).GetBoolean(),
                ItemId = root.TryGetProperty(nameof(QuoteAlert.ItemId), out var itemId) ? itemId.GetInt32() : 0,
                TraderQuoteType = root.TryGetProperty(nameof(QuoteAlert.TraderQuoteType), out var quoteType)
                    ? Enum.Parse<TraderQuoteType>(quoteType.GetString() ?? nameof(TraderQuoteType.Buy))
                    : default,
                UpperPriceTarget = root.TryGetProperty(nameof(QuoteAlert.UpperPriceTarget), out var upperPrice) ? upperPrice.GetInt32() : 0,
                UpperPriceTargetEnabled = root.TryGetProperty(nameof(QuoteAlert.UpperPriceTargetEnabled), out var upperEnabled) && upperEnabled.GetBoolean(),
                LowerPriceTarget = root.TryGetProperty(nameof(QuoteAlert.LowerPriceTarget), out var lowerPrice) ? lowerPrice.GetInt32() : 0,
                LowerPriceTargetEnabled = root.TryGetProperty(nameof(QuoteAlert.LowerPriceTargetEnabled), out var lowerEnabled) && lowerEnabled.GetBoolean()
            };
        }
    }

    public override void Write(Utf8JsonWriter writer, ITradeAlert value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(nameof(ITradeAlert.Id), value.Id);
        writer.WriteString(nameof(ITradeAlert.Name), value.Name);
        writer.WriteBoolean(nameof(ITradeAlert.Enabled), value.Enabled);

        if (value is TradeAlert tradeAlert)
        {
            writer.WriteString(nameof(TradeAlert.MessageCheck), tradeAlert.MessageCheck);
            writer.WriteBoolean(nameof(TradeAlert.MessageRegexCheck), tradeAlert.MessageRegexCheck);
            writer.WriteString(nameof(TradeAlert.SenderCheck), tradeAlert.SenderCheck);
            writer.WriteBoolean(nameof(TradeAlert.SenderRegexCheck), tradeAlert.SenderRegexCheck);
        }
        else if (value is QuoteAlert quoteAlert)
        {
            writer.WriteString(nameof(QuoteAlert.TraderQuoteType), quoteAlert.TraderQuoteType.ToString());
            writer.WriteNumber(nameof(QuoteAlert.ItemId), quoteAlert.ItemId);
            writer.WriteNumber(nameof(QuoteAlert.UpperPriceTarget), quoteAlert.UpperPriceTarget);
            writer.WriteBoolean(nameof(QuoteAlert.UpperPriceTargetEnabled), quoteAlert.UpperPriceTargetEnabled);
            writer.WriteNumber(nameof(QuoteAlert.LowerPriceTarget), quoteAlert.LowerPriceTarget);
            writer.WriteBoolean(nameof(QuoteAlert.LowerPriceTargetEnabled), quoteAlert.LowerPriceTargetEnabled);
        }

        writer.WriteEndObject();
    }
}
