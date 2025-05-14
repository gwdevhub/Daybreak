using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using FluentAssertions;

namespace Daybreak.Tests.Services.Models;
public sealed class TestArgumentHandler : IArgumentHandler
{
    public string Identifier => "-r";
    public int ExpectedArgumentCount => 3;
    public bool Called { get; private set; }

    public void HandleArguments(string[] args)
    {
        args.Should().BeEquivalentTo(["so", "me", "arg"]);
        this.Called = true;
    }
}
