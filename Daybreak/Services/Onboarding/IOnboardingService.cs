using Daybreak.Models.Onboarding;
using System.Threading.Tasks;

namespace Daybreak.Services.Onboarding;

public interface IOnboardingService
{
    Task<LauncherOnboardingStage> CheckOnboardingStage();
}
