using Daybreak.Shared.Models.Onboarding;

namespace Daybreak.Shared.Services.Onboarding;

public interface IOnboardingService
{
    LauncherOnboardingStage CheckOnboardingStage();
}
