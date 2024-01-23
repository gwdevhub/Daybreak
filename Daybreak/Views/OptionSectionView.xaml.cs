using Daybreak.Attributes;
using Daybreak.Controls.Buttons;
using Daybreak.Controls.Options;
using Daybreak.Models.Options;
using Daybreak.Services.Navigation;
using Daybreak.Services.Options;
using Daybreak.Validators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using System.Windows.Input;

namespace Daybreak.Views;
/// <summary>
/// Interaction logic for OptionSectionView.xaml
/// </summary>
public partial class OptionSectionView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly IOptionsProvider optionsProvider;

    private object currentOptions = new();

    [GenerateDependencyProperty]
    private string title = string.Empty;

    public ObservableCollection<OptionEntry> OptionEntries { get; } = [];

    public OptionSectionView(
        IViewManager viewManager,
        IOptionsProvider optionsProvider)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.optionsProvider = optionsProvider.ThrowIfNull();

        this.InitializeComponent();

        this.DataContextChanged += this.OptionSectionView_DataContextChanged;
    }

    private void OptionSectionView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.Property != DataContextProperty)
        {
            return;
        }

        if (e.NewValue is not OptionSection optionSection)
        {
            return;
        }

        this.OptionEntries.Clear();
        this.GetAndSetCurrentValue(optionSection.Type!);
        this.Title = optionSection.Name;
        foreach(var propertyInfo in this.currentOptions.GetType().GetProperties())
        {
            if (!IsVisibleOption(propertyInfo))
            {
                continue;
            }

            var propertyType = propertyInfo.PropertyType;
            (var name, var description) = this.GetNameAndDescription(propertyInfo);
            (var validator, var template) = GetValidatorAndTemplate(propertyInfo);
            (var hasCustomSetter, var action, var customSetterViewType) = GetCustomSetter(propertyInfo);
            var converter = TypeDescriptor.GetConverter(propertyType);
            var getter = new Func<object>(() => propertyInfo.GetValue(this.currentOptions)!);
            var setter = new Action<object>((value) =>
            {
                if (propertyType == value.GetType())
                {
                    propertyInfo.SetValue(this.currentOptions, value);
                }
                else
                {
                    propertyInfo.SetValue(this.currentOptions, converter.ConvertFrom(value));
                }
            });
            var optionProperty = new OptionProperty(name, description, validator ?? new AllGoesValidator(), setter, getter, converter, propertyType);
            if (template is null ||
                validator is null)
            {
                continue;
            }

            template.DataContext = optionProperty;
            var optionHeadingDesiredSize = template.Height > 0 ?
                template.Height :
                template.DesiredSize.Height > 0 ?
                    template.DesiredSize.Height :
                    0;
            var heading = new OptionHeading { Title = name, DesiredHeight = optionHeadingDesiredSize, Description = description };
            var optionSetter = new OptionSetter { HasCustomSetter = hasCustomSetter, CustomSetterAction = action, CustomSetterViewType = customSetterViewType };
            this.OptionEntries.Add(new OptionEntry { Heading = heading, Template = template, Setter = optionSetter });
        }
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        this.optionsProvider.SaveRegisteredOptions(this.currentOptions);
        this.viewManager.ShowView<LauncherView>();
    }

    private void GetAndSetCurrentValue(Type optionType)
    {
        var maybeValue = this.optionsProvider.GetRegisteredOptions().FirstOrDefault(o => o.GetType() == optionType);
        if (maybeValue is null)
        {
            return;
        }

        this.currentOptions = maybeValue;
    }

    private (string Name, string Description) GetNameAndDescription(PropertyInfo propertyInfo)
    {
        string? name;
        var description = string.Empty;
        if (propertyInfo.GetCustomAttribute<OptionNameAttribute>() is not OptionNameAttribute optionNameAttribute)
        {
            name = propertyInfo.Name;
        }
        else
        {
            name = optionNameAttribute.Name;
            description = optionNameAttribute.Description;
        }

        if (description.IsNullOrWhiteSpace())
        {
            description = GetDefaultDescription(name, this.Title);
        }

        return (name, description);
    }

    private static (bool HasCustomSetter, string? CustomSetterAction, Type? CustomSetterViewType) GetCustomSetter(PropertyInfo propertyInfo)
    {
        if (propertyInfo.GetCustomAttributes()
                .FirstOrDefault(a =>
                {
                    var attributeType = a.GetType();
                    if (!attributeType.IsGenericType)
                    {
                        return false;
                    }

                    return a.GetType().GetGenericTypeDefinition() == typeof(OptionSetterView<>);
                }) is not object customSetterViewAttribute)
        {
            return (false, default, default);
        }

        var action = customSetterViewAttribute.GetType().GetProperty(nameof(OptionSetterView<UserControl>.Action))?
            .GetValue(customSetterViewAttribute)?.As<string>();
        var viewType = customSetterViewAttribute.GetType().GetGenericArguments().FirstOrDefault();

        return (viewType is not null,
            action,
            viewType);
    }

    private static bool IsVisibleOption(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetCustomAttribute<OptionIgnoreAttribute>() is null;
    }

    private static (IValidator? Validator, UserControl? Template) GetValidatorAndTemplate(PropertyInfo propertyInfo)
    {
        // If property has a custom validator, return that validator
        if (propertyInfo.GetCustomAttributes()
                .FirstOrDefault(a =>
                {
                    var attributeType = a.GetType();
                    if (!attributeType.IsGenericType)
                    {
                        return false;
                    }

                    return a.GetType().GetGenericTypeDefinition() == typeof(OptionCustomValidatorAttribute<>);
                }) is object customValidatorAttribute)
        {
            var validatorPropertyInfo = customValidatorAttribute.GetType().GetProperty(nameof(OptionCustomValidatorAttribute<AllGoesValidator>.Validator));
            return (validatorPropertyInfo?.GetValue(customValidatorAttribute) as IValidator, new StringOptionTemplate());
        }

        if (propertyInfo.PropertyType.IsPrimitive ||
            propertyInfo.PropertyType == typeof(string))
        {
            return GetPrimitiveValidator(propertyInfo);
        }
        else if (propertyInfo.PropertyType.IsEnum)
        {
            return (Activator.CreateInstance(typeof(EnumValidator<>).MakeGenericType(propertyInfo.PropertyType)) as IValidator, new EnumOptionTemplate());
        }

        return default;
    }

    private static (IValidator? Validator, UserControl? Template) GetPrimitiveValidator(PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType == typeof(byte) ||
            propertyInfo.PropertyType == typeof(byte?))
        {
            var validator = GetClampedValidator(propertyInfo, byte.MinValue, byte.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(sbyte) ||
                propertyInfo.PropertyType == typeof(sbyte?))
        {
            var validator = GetClampedValidator(propertyInfo, sbyte.MinValue, sbyte.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(short) ||
                propertyInfo.PropertyType == typeof(short?))
        {
            var validator = GetClampedValidator(propertyInfo, short.MinValue, short.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(ushort) ||
                propertyInfo.PropertyType == typeof(ushort?))
        {
            var validator = GetClampedValidator(propertyInfo, ushort.MinValue, ushort.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(int) ||
                propertyInfo.PropertyType == typeof(int?))
        {
            var validator = GetClampedValidator(propertyInfo, int.MinValue, int.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(uint) ||
                propertyInfo.PropertyType == typeof(uint?))
        {
            var validator = GetClampedValidator(propertyInfo, uint.MinValue, uint.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(long) ||
                propertyInfo.PropertyType == typeof(long?))
        {
            var validator = GetClampedValidator(propertyInfo, long.MinValue, long.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(ulong) ||
                propertyInfo.PropertyType == typeof(ulong?))
        {
            var validator = GetClampedValidator(propertyInfo, ulong.MinValue, ulong.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(float) ||
                propertyInfo.PropertyType == typeof(float?))
        {
            var validator = GetClampedValidator(propertyInfo, float.MinValue, float.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(double) ||
                propertyInfo.PropertyType == typeof(double?))
        {
            var validator = GetClampedValidator(propertyInfo, double.MinValue, double.MaxValue, out var clampDetails);
            UserControl template = clampDetails.IsRange ?
                new PrimitiveRangeOptionTemplate(clampDetails.Min, clampDetails.Max) :
                new StringOptionTemplate();
            return (validator, template);
        }
        else if (propertyInfo.PropertyType == typeof(bool) ||
                propertyInfo.PropertyType == typeof(bool?))
        {
            return (new BooleanValidator(), new BooleanOptionTemplate());
        }
        else if (propertyInfo.PropertyType == typeof(string))
        {
            return (new AllGoesValidator(), new StringOptionTemplate());
        }

        return default;
    }

    private static IValidator? GetClampedValidator<T>(PropertyInfo propertyInfo, T defaultMinValue, T defaultMaxValue, out (bool IsRange, T Min, T Max) clampDetails)
        where T : IComparable<T>
    {
        var maybeOptionRangeAttribute = propertyInfo.GetCustomAttribute<OptionRangeAttribute<T>>();
        var minValue = maybeOptionRangeAttribute is not null ?
            maybeOptionRangeAttribute.MinValue :
            defaultMinValue;

        var maxValue = maybeOptionRangeAttribute is not null ?
            maybeOptionRangeAttribute.MaxValue :
            defaultMaxValue;

        clampDetails = maybeOptionRangeAttribute is not null ?
            (true, minValue, maxValue) :
            (false, minValue, maxValue);

        return new ClampedValidator<T>(minValue, maxValue);
    }

    private static string GetDefaultDescription(string optionName, string optionSectionName)
    {
        return $"{optionName} property of {optionSectionName}";
    }

    private void CustomSetterButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not HighlightButton highlightButton ||
            highlightButton.DataContext is not OptionSetter optionSetter)
        {
            return;
        }

        if (!optionSetter.HasCustomSetter)
        {
            return;
        }

        this.viewManager.ShowView(optionSetter.CustomSetterViewType!);
    }

    private void HelpButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not HelpButton helpButton)
        {
            return;
        }

        helpButton.ToolTip.As<ToolTip>()!.IsOpen = true;
        helpButton.ToolTip.As<ToolTip>()!.Content = helpButton.DataContext.As<OptionEntry>()?.Heading?.Description;
    }

    private void HelpButton_MouseLeave(object sender, MouseEventArgs e)
    {
        if (sender is not HelpButton helpButton)
        {
            return;
        }

        if (helpButton.ToolTip.As<ToolTip>()!.IsOpen)
        {
            helpButton.ToolTip.As<ToolTip>()!.IsOpen = false;
        }
    }
}
