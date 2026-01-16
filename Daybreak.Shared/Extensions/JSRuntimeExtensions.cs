using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Daybreak.Shared.Models;

namespace Daybreak.Shared.Extensions;

public static class JSRuntimeExtensions
{
    public static async Task<ClientBoundingRect> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element, CancellationToken cancellationToken = default)
    {
        return await jsRuntime.InvokeAsync<ClientBoundingRect>("getElementBoundingRect", cancellationToken, element);
    }

    public static async Task HoverDelayStart<T>(this IJSRuntime jsRuntime, DotNetObjectReference<T> element, string callback, CancellationToken cancellationToken = default)
        where T : class
    {
        await jsRuntime.InvokeVoidAsync("hoverDelay.start", cancellationToken, element, callback);
    }

    public static async Task HoverDelayStop(this IJSRuntime jsRuntime, CancellationToken cancellationToken = default)
    {
        await jsRuntime.InvokeVoidAsync("hoverDelay.stop", cancellationToken);
    }
}
