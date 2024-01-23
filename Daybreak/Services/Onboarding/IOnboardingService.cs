using Daybreak.Models.Onboarding;

namespace Daybreak.Services.Onboarding;

public interface IOnboardingService
{
    LauncherOnboardingStage CheckOnboardingStage();
}
