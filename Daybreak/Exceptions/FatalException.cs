using System;

namespace Daybreak.Exceptions;

public sealed class FatalException : Exception
{
    public FatalException()
    {
    }

    public FatalException(string message) : base(message)
    {
    }

    public FatalException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
