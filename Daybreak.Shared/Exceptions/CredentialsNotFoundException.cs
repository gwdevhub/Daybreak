using System;

namespace Daybreak.Shared.Exceptions;

public sealed class CredentialsNotFoundException : Exception
{
    public CredentialsNotFoundException()
    {
    }

    public CredentialsNotFoundException(string message) : base(message)
    {
    }


    public CredentialsNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
