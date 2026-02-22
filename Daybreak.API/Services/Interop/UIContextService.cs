using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Collections.Concurrent;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class UIContextService(ILogger<UIContextService> logger)
{
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void AsyncDecodeStrCallback(void* param, ushort* decodedString);

    // Prevent GC of delegates passed to native code
    private readonly ConcurrentDictionary<AsyncDecodeStrCallback, bool> prevent_GC_callbacks = [];

    private readonly ILogger<UIContextService> logger = logger.ThrowIfNull();

    public unsafe WrappedPointer<T> GetFrameContext<T>(WrappedPointer<Frame> frame)
        where T : unmanaged
    {
        if (frame.IsNull || frame.Pointer->FrameCallbacks.Size == 0)
        {
            return null;
        }

        for (uint i = 0; i < frame.Pointer->FrameCallbacks.Size; i++)
        {
            var callback = frame.Pointer->FrameCallbacks.Buffer[i];
            if (callback.UiCtl_Context != null)
            {
                return new WrappedPointer<T>((T*)callback.UiCtl_Context);
            }
        }

        return null;
    }

    public unsafe WrappedPointer<Frame> GetChildFrame(WrappedPointer<Frame> parent, uint childOffset)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (parent.IsNull)
        {
            scopedLogger.LogError("Parent frame is null");
            return null;
        }

        return GWCA.GW.UI.GetChildFrame(parent, childOffset);
    }

    public WrappedPointer<Frame> GetButtonActionFrame()
    {
        return this.GetChildFrame(this.GetFrameByLabel("Game"), 6);
    }

    public unsafe bool SendFrameUIMessage(WrappedPointer<Frame> frame, UIMessage messageId, void* arg1, void* arg2 = null)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (frame.IsNull)
        {
            scopedLogger.LogError("Frame is null");
            return false;
        }

        return GWCA.GW.UI.SendFrameUIMessage(frame, messageId, arg1, (nint)arg2);
    }

    public unsafe bool SetFrameDisabled(WrappedPointer<Frame> frame, bool disabled)
    {
        return GWCA.GW.UI.SetFrameDisabled(frame, disabled);
    }

    public unsafe bool SetFrameVisible(WrappedPointer<Frame> frame, bool visible)
    {
        return GWCA.GW.UI.SetFrameVisible(frame, visible);
    }

    public unsafe bool KeyDown(ControlAction action, WrappedPointer<Frame> frame)
    {
        return GWCA.GW.UI.Keydown(action, frame);
    }

    public unsafe bool KeyUp(ControlAction action, WrappedPointer<Frame> frame)
    {
        return GWCA.GW.UI.Keyup(action, frame);
    }

    public unsafe WrappedPointer<Frame> GetFrameByLabel(string label)
    {
        fixed (char* labelPtr = label)
        {
            return GWCA.GW.UI.GetFrameByLabel((ushort*)labelPtr);
        }
    }

    public unsafe WrappedPointer<Frame> GetFrameById(uint frameId)
    {
        return GWCA.GW.UI.GetFrameById(frameId);
    }

    public unsafe void SendMessage(UIMessage message, nuint wParam, nuint lParam)
    {
        GWCA.GW.UI.SendUIMessage(message, (void*)wParam, (nint)lParam);
    }

    public unsafe Task<string> AsyncDecodeStringAsync(ushort* encodedString, CancellationToken cancellationToken = default)
    {
        var taskCompletionSource = new TaskCompletionSource<string>();
        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(cancellationToken));
        void callback(void* param, ushort* decodedString)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    taskCompletionSource.TrySetCanceled(cancellationToken);
                    return;
                }

                // Convert the decoded wide string to a managed string
                var result = decodedString != null
                    ? new string((char*)decodedString)
                    : string.Empty;

                taskCompletionSource.TrySetResult(result);
            }
            catch (Exception ex)
            {
                taskCompletionSource.TrySetException(ex);
            }
            finally
            {
                this.prevent_GC_callbacks.TryRemove(callback, out _);
            }
        }

        // CRITICAL: Keep delegate alive until callback executes
        this.prevent_GC_callbacks[callback] = true;

        var funcPtr = Marshal.GetFunctionPointerForDelegate(callback);
        GWCA.GW.UI.AsyncDecodeStr(encodedString, funcPtr);

        return taskCompletionSource.Task;
    }

    public unsafe Task<string> AsyncDecodeStringAsync(string encodedString, CancellationToken cancellationToken = default)
    {
        // Pin the string and convert to ushort*
        fixed (char* encodedPtr = encodedString)
        {
            return this.AsyncDecodeStringAsync((ushort*)encodedPtr, cancellationToken);
        }
    }
}
