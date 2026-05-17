using Daybreak.Configuration.Options;
using Daybreak.Services.Credentials;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Credentials;
using Daybreak.Shared.Services.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace Daybreak.Tests.Services.Credentials;

[TestClass]
public sealed class CredentialManagerTests
{
    private readonly ICredentialProtector protector = Substitute.For<ICredentialProtector>();
    private readonly IOptionsProvider optionsProvider = Substitute.For<IOptionsProvider>();
    private readonly IOptionsMonitor<CredentialManagerOptions> liveOptions = Substitute.For<IOptionsMonitor<CredentialManagerOptions>>();
    private readonly CredentialManagerOptions options = new();
    private readonly CredentialManager manager;

    public CredentialManagerTests()
    {
        this.liveOptions.CurrentValue.Returns(this.options);
        // Reversible "encryption" — easy to assert and reverse for round-trip tests.
        this.protector.Protect(Arg.Any<byte[]>()).Returns(call => Reverse(call.Arg<byte[]>()));
        this.protector.Unprotect(Arg.Any<byte[]>()).Returns(call => Reverse(call.Arg<byte[]>()));
        this.manager = new CredentialManager(
            this.protector,
            this.optionsProvider,
            this.liveOptions,
            Substitute.For<ILogger<CredentialManager>>());
    }

    private static byte[] Reverse(byte[] bytes)
    {
        var copy = (byte[])bytes.Clone();
        Array.Reverse(copy);
        return copy;
    }

    [TestMethod]
    public void GetCredentialList_EmptyOptions_ReturnsEmpty()
    {
        this.manager.GetCredentialList().Should().BeEmpty();
    }

    [TestMethod]
    public void GetCredentialList_NullCollection_ReturnsEmpty()
    {
        this.options.ProtectedLoginCredentials = null!;

        this.manager.GetCredentialList().Should().BeEmpty();
    }

    [TestMethod]
    public void StoreCredentials_RoundTripsThroughProtector()
    {
        var creds = new LoginCredentials { Identifier = "id-1", Username = "alice", Password = "s3cret" };

        this.manager.StoreCredentials([creds]);
        var roundTripped = this.manager.GetCredentialList();

        roundTripped.Should().ContainSingle().Which.Should().BeEquivalentTo(creds);
        this.optionsProvider.Received(1).SaveOption(this.options);
        this.options.ProtectedLoginCredentials.Should().ContainSingle()
            .Which.Identifier.Should().Be("id-1");
    }

    [TestMethod]
    public void StoreCredentials_ProtectsUsernameAndPassword_AsBase64ReversedBytes()
    {
        var creds = new LoginCredentials { Identifier = "id-1", Username = "alice", Password = "pw" };

        this.manager.StoreCredentials([creds]);

        var stored = this.options.ProtectedLoginCredentials.Single();
        var expectedUsername = Convert.ToBase64String(Reverse(System.Text.Encoding.UTF8.GetBytes("alice")));
        var expectedPassword = Convert.ToBase64String(Reverse(System.Text.Encoding.UTF8.GetBytes("pw")));
        stored.ProtectedUsername.Should().Be(expectedUsername);
        stored.ProtectedPassword.Should().Be(expectedPassword);
    }

    [TestMethod]
    public void GetCredentialList_UnprotectThrows_DropsThatEntry()
    {
        // Cause the underlying conversion to fail by setting an invalid base64 string for one entry.
        this.options.ProtectedLoginCredentials =
        [
            new ProtectedLoginCredentials { Identifier = "bad", ProtectedUsername = "!!not-base64!!", ProtectedPassword = "!!not-base64!!" },
            new ProtectedLoginCredentials { Identifier = "good", ProtectedUsername = Convert.ToBase64String(Reverse(System.Text.Encoding.UTF8.GetBytes("user"))), ProtectedPassword = Convert.ToBase64String(Reverse(System.Text.Encoding.UTF8.GetBytes("pass"))) }
        ];

        var result = this.manager.GetCredentialList();

        result.Should().ContainSingle().Which.Identifier.Should().Be("good");
    }

    [TestMethod]
    public void TryGetCredentialsByIdentifier_Found_ReturnsTrueAndCredentials()
    {
        this.manager.StoreCredentials([new LoginCredentials { Identifier = "id-1", Username = "a", Password = "b" }]);

        var ok = this.manager.TryGetCredentialsByIdentifier("id-1", out var found);

        ok.Should().BeTrue();
        found!.Identifier.Should().Be("id-1");
    }

    [TestMethod]
    public void TryGetCredentialsByIdentifier_Missing_ReturnsFalseAndNull()
    {
        var ok = this.manager.TryGetCredentialsByIdentifier("missing", out var found);

        ok.Should().BeFalse();
        found.Should().BeNull();
    }

    [TestMethod]
    public void CreateUniqueCredentials_HasUniqueGuidIdentifierAndEmptyStrings()
    {
        var a = this.manager.CreateUniqueCredentials();
        var b = this.manager.CreateUniqueCredentials();

        a.Identifier.Should().NotBeNullOrEmpty();
        Guid.TryParse(a.Identifier, out _).Should().BeTrue();
        a.Identifier.Should().NotBe(b.Identifier);
        a.Username.Should().BeEmpty();
        a.Password.Should().BeEmpty();
    }
}
