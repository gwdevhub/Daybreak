using Daybreak.Models;
using System.Threading.Tasks;

namespace Daybreak.Services.Onboarding;

public interface IOnboardingService
{
    Task<OnboardingStage> CheckOnboardingStage();
}
