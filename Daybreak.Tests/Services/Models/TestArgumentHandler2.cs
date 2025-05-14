using Daybreak.Shared.Services.ApplicationArguments.ArgumentHandling;
using FluentAssertions;

namespace Daybreak.Tests.Services.Models;
public sealed class TestArgumentHandler2 : IArgumentHandler
{
    public string Identifier => "-c";
    public int ExpectedArgumentCount => 5;
    public bool Called { get; private set; }

    public void HandleArguments(string[] args)
    {
        args.Should().BeEquivalentTo(["uments", "he", "re", "and", "there"]);
        this.Called = true;
    }
}
