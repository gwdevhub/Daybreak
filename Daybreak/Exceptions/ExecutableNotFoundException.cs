using System;

namespace Daybreak.Exceptions;

public sealed class ExecutableNotFoundException : Exception
{
    public ExecutableNotFoundException()
    {
    }

    public ExecutableNotFoundException(string message) : base(message)
    {
    }


    public ExecutableNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
