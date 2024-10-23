﻿using Realms;
using System;

namespace Daybreak.Services.TradeChat.Models;

public sealed class TraderQuoteDTO : RealmObject
{
    public int ItemId { get; set; }

    public int Price { get; set; }

    public string? ModifiersHash { get; set; }

    public int TraderQuoteType { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public DateTimeOffset InsertionTime { get; set; }
}
