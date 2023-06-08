using System.Media;

namespace Daybreak.Services.Sounds;

public interface ISoundService
{
    void PlayNotifyInformation();
    void PlayNotifyError();
    void PlayNotifyClose();
}
