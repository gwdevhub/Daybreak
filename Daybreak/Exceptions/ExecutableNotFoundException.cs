using System;
using System.Runtime.Serialization;

namespace Daybreak.Exceptions;

public sealed class ExecutableNotFoundException : Exception
{
    public ExecutableNotFoundException()
    {
    }

    public ExecutableNotFoundException(string message) : base(message)
    {
    }

    public ExecutableNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ExecutableNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
