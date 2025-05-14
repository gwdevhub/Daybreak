using Daybreak.Configuration.Options;
using Daybreak.Shared.Services.Sounds;
using NAudio.Wave;
using System.Configuration;
using System.Core.Extensions;
using System.IO;
using System.Reflection;

namespace Daybreak.Services.Sounds;

internal sealed class SoundService : ISoundService
{
    private const string CloseNotification = "NotifyClose";
    private const string ErrorNotification = "NotifyError";
    private const string InformationNotification = "NotifyInformation";

    private readonly ILiveOptions<SoundOptions> liveOptions;
    private readonly Assembly containingAssembly;

    public SoundService(
        ILiveOptions<SoundOptions> options)
    {
        this.liveOptions = options.ThrowIfNull();
        this.containingAssembly = this.GetType().Assembly;
    }

    public void PlayNotifyClose()
    {
        var maybeResourceStream = GetResourceStream(this.containingAssembly, CloseNotification);
        if (maybeResourceStream is null)
        {
            return;
        }

        this.PlayResourceStream(maybeResourceStream);
    }

    public void PlayNotifyError()
    {
        var maybeResourceStream = GetResourceStream(this.containingAssembly, ErrorNotification);
        if (maybeResourceStream is null)
        {
            return;
        }

        this.PlayResourceStream(maybeResourceStream);
    }

    public void PlayNotifyInformation()
    {
        var maybeResourceStream = GetResourceStream(this.containingAssembly, InformationNotification);
        if (maybeResourceStream is null)
        {
            return;
        }

        this.PlayResourceStream(maybeResourceStream);
    }

    private void PlayResourceStream(Stream stream)
    {
        if (!this.liveOptions.Value.Enabled)
        {
            return;
        }

        var waveStream = new WaveFileReader(stream);
        var waveOut = new WaveOutEvent();
        waveOut.Init(waveStream);
        waveOut.Play();
        waveOut.PlaybackStopped += (_, _) =>
        {
            waveOut.Dispose();
            waveStream.Dispose();
        };
    }

    private static Stream? GetResourceStream(Assembly assembly, string resourceName)
    {
        return assembly.GetManifestResourceStream($"Daybreak.Resources.{resourceName}.wav");
    }
}
