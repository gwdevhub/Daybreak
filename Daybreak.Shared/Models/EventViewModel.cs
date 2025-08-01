﻿using Daybreak.Shared.Models.Guildwars;
using System.ComponentModel;
using System.Extensions;

namespace Daybreak.Models;
public partial class EventViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    [GenerateNotifyPropertyChanged]
    private Event? seasonalEvent;
    [GenerateNotifyPropertyChanged]
    private bool active;
    [GenerateNotifyPropertyChanged]
    private bool upcoming;
}
