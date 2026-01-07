namespace Daybreak.Services.Injection.Models;

internal sealed record InjectionResult(int ExitCode, string Output, string Error)
{
}
