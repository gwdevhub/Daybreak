﻿using Daybreak.Attributes;
using Daybreak.Controls.Buttons;
using Daybreak.Shared;
using Daybreak.Shared.Models.Options;
using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Options;
using Daybreak.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Core.Extensions;
using System.Extensions;
using System.Reflection;
using System.Windows.Controls;

namespace Daybreak.Controls.Options;
/// <summary>
/// Interaction logic for OptionsSection.xaml
/// </summary>
public partial class OptionsSection(
    IOptionsProvider optionsProvider,
    IViewManager viewManager) : UserControl
{
    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();

    public ObservableCollection<OptionSection> Options { get; } = [];

    public OptionsSection()
        : this(Global.GlobalServiceProvider.GetRequiredService<IOptionsProvider>(),
               Global.GlobalServiceProvider.GetRequiredService<IViewManager>())
    {
        this.InitializeComponent();
        this.InitializeOptions();
    }

    private void InitializeOptions()
    {
        var registeredOptions = this.optionsProvider.GetRegisteredOptionsTypes();
        foreach(var registeredType in registeredOptions)
        {
            if (!IsOptionsVisible(registeredType))
            {
                continue;
            }

            this.Options.Add(new OptionSection
            {
                Name = GetOptionsName(registeredType),
                Tooltip = GetOptionsToolTip(registeredType),
                Type = registeredType
            });
        }
    }

    private static string GetOptionsToolTip(Type type)
    {
        var name = GetOptionsName(type);
        return $"{name} settings";
    }

    private static string GetOptionsName(Type type)
    {
        if (type.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(OptionsNameAttribute)) is not OptionsNameAttribute optionsNameAttribute)
        {
            return type.Name;
        }

        if (optionsNameAttribute.Name!.IsNullOrWhiteSpace())
        {
            return type.Name;
        }

        return optionsNameAttribute.Name!;
    }

    private static bool IsOptionsVisible(Type optionType)
    {
        if (optionType.GetCustomAttribute<OptionsIgnoreAttribute>() is not null)
        {
            return false;
        }

        return true;
    }

    private void MenuButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not MenuButton button)
        {
            return;
        }

        if (button.DataContext is not OptionSection optionSection)
        {
            return;
        }

        this.viewManager.ShowView<OptionSectionView>(optionSection);
    }
}
