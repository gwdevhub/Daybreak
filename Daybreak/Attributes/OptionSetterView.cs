using System;
using System.Windows.Controls;

namespace Daybreak.Attributes;

/// <summary>
/// Mark a custom view that is supposed to set the value of the option.
/// In the option view, a button will be displayed that will redirect to the custom view.
/// The custom view must be registered in the <see cref="Services.Navigation.IViewManager"/>
/// </summary>
/// <typeparam name="T">Type of view. Has to be registered in the <see cref="Services.Navigation.IViewManager"./></typeparam>
[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionSetterView<T> : Attribute
    where T : UserControl
{
    public string Action { get; set; } = "Set";
}
