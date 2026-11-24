using Daybreak.Shared.Models.Options;
using Daybreak.Shared.Validators;
using Daybreak.Views.Components;
using Daybreak.Views.Components.Options;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using NSubstitute;
using TypeDescriptor = System.ComponentModel.TypeDescriptor;

namespace Daybreak.Tests.Views;

#pragma warning disable BL0006 // Exercise rendered component callbacks without a browser.

[TestClass]
public sealed class OptionInputTests
{
    [TestMethod]
    public async Task NumberInput_SavesAnIntegerWithoutChangingItsType()
    {
        var option = new OptionValue(1);
        using var services = CreateServices();
        await using var renderer = new TestRenderer(services);

        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = await renderer.RenderAsync<NumberOption>(option.Parameters);
            var input = renderer.FindComponent<FluentNumberInput<double>>(id);

            await input.ValueChanged.InvokeAsync(3.0);

            option.Value.Should().BeOfType<int>().Which.Should().Be(3);
            option.ChangeCount.Should().Be(1);
        });
    }

    [TestMethod]
    public async Task RangedNumberInput_SavesValidValuesAndRejectsInvalidValues()
    {
        var validator = new ClampedValidator<float>(0.0f, 1.0f);
        var option = new OptionValue(0.5f, validator);
        option.Parameters[nameof(NumberRangeOption.Validator)] = validator;
        using var services = CreateServices();
        await using var renderer = new TestRenderer(services);

        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = await renderer.RenderAsync<NumberRangeOption>(option.Parameters);
            var input = renderer.FindComponent<FluentNumberInput<double>>(id);

            await input.ValueChanged.InvokeAsync(0.75);
            await input.ValueChanged.InvokeAsync(2.0);

            option.Value.Should().BeOfType<float>().Which.Should().Be(0.75f);
            option.ChangeCount.Should().Be(1);
        });
    }

    [TestMethod]
    public async Task StringInput_SavesTextAndAllowsClearingIt()
    {
        var option = new OptionValue("original");
        using var services = CreateServices();
        await using var renderer = new TestRenderer(services);

        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = await renderer.RenderAsync<StringOption>(option.Parameters);
            var input = renderer.FindComponent<FluentTextInput>(id);

            await input.ValueChanged.InvokeAsync("updated");
            option.Value.Should().Be("updated");
            await input.ValueChanged.InvokeAsync(null);

            option.Value.Should().Be(string.Empty);
            option.ChangeCount.Should().Be(2);
        });
    }

    [TestMethod]
    public async Task SelectInput_SavesTheEnumValueAndIgnoresUnknownOptions()
    {
        var option = new OptionValue(DayOfWeek.Monday);
        option.Parameters[nameof(MultiSelectOption.PossibleValues)] =
            new List<object> { DayOfWeek.Monday, DayOfWeek.Tuesday };
        using var services = CreateServices();
        await using var renderer = new TestRenderer(services);

        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = await renderer.RenderAsync<MultiSelectOption>(option.Parameters);
            var input = renderer.FindComponent<FluentSelect<string, string>>(id);

            await input.ValueChanged.InvokeAsync("Tuesday");
            await input.ValueChanged.InvokeAsync("Unknown");

            option.Value.Should().Be(DayOfWeek.Tuesday);
            option.ChangeCount.Should().Be(1);
        });
    }

    [TestMethod]
    public async Task SearchInput_RendersItsTemplateAndDebouncesValueChanges()
    {
        var searches = new List<string>();
        var searchCompleted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var services = CreateServices();
        await using var renderer = new TestRenderer(services);

        await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var id = await renderer.RenderAsync<DebouncedTextField>(
                new Dictionary<string, object?>
                {
                    [nameof(DebouncedTextField.OnSearch)] = (Action<string>)(value =>
                    {
                        searches.Add(value);
                        searchCompleted.TrySetResult();
                    }),
                });
            var input = renderer.FindComponent<FluentTextInput>(id);

            renderer.FindComponent<FluentIcon<Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size16.Search>>(id)
                .Should().NotBeNull();
            await input.ValueChanged.InvokeAsync("first");
            await input.ValueChanged.InvokeAsync("latest");
            searchCompleted.Task.IsCompleted.Should().BeFalse();
        });

        await searchCompleted.Task.WaitAsync(TimeSpan.FromSeconds(5));
        searches.Should().Equal("latest");
    }

    private static ServiceProvider CreateServices()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(Substitute.For<IJSRuntime>());
        services.AddFluentUIComponents();
        return services.BuildServiceProvider();
    }

    private sealed class OptionValue
    {
        public object Value { get; private set; }
        public int ChangeCount { get; private set; }
        public Dictionary<string, object?> Parameters { get; }

        public OptionValue(object value, IValidator? validator = null)
        {
            this.Value = value;
            var property = new OptionProperty
            {
                Name = "Value",
                Description = string.Empty,
                Validator = validator ?? new AllGoesValidator(),
                ValuesFactory = null,
                Getter = _ => this.Value,
                Setter = (_, updated) => this.Value = updated!,
                Converter = TypeDescriptor.GetConverter(value.GetType()),
                Type = value.GetType(),
                IsSynchronized = false,
                IsVisible = true,
            };
            this.Parameters = new Dictionary<string, object?>
            {
                ["Name"] = property.Name,
                ["Description"] = property.Description,
                ["Property"] = property,
                ["Instance"] = new OptionInstance
                {
                    Reference = this,
                    Type = new OptionType
                    {
                        Name = "Test",
                        Description = string.Empty,
                        Type = typeof(OptionValue),
                        Properties = [property],
                        IsSynchronized = false,
                        IsVisible = true,
                    },
                },
                ["ValueChanged"] = (Action)(() => this.ChangeCount++),
            };
        }
    }

    private sealed class TestRenderer(IServiceProvider services)
        : Renderer(services, services.GetRequiredService<ILoggerFactory>())
    {
        public override Dispatcher Dispatcher { get; } = Dispatcher.CreateDefault();

        public async Task<int> RenderAsync<T>(Dictionary<string, object?> parameters)
            where T : IComponent
        {
            var id = this.AssignRootComponentId(this.InstantiateComponent(typeof(T)));
            await this.RenderRootComponentAsync(id, ParameterView.FromDictionary(parameters));
            return id;
        }

        public T FindComponent<T>(int id) where T : IComponent
        {
            var frames = this.GetCurrentRenderTreeFrames(id);
            foreach (var frame in frames.Array.Take(frames.Count))
            {
                if (frame.FrameType != RenderTreeFrameType.Component)
                {
                    continue;
                }

                if (frame.Component is T component)
                {
                    return component;
                }

                try
                {
                    return this.FindComponent<T>(frame.ComponentId);
                }
                catch (InvalidOperationException)
                {
                }
            }

            throw new InvalidOperationException($"No rendered {typeof(T).Name} component");
        }

        protected override Task UpdateDisplayAsync(in RenderBatch renderBatch) => Task.CompletedTask;

        protected override void HandleException(Exception exception) => throw exception;
    }
}
