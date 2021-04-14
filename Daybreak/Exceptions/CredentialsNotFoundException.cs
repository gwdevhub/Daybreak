using System;
using System.Runtime.Serialization;

namespace Daybreak.Exceptions
{
    public sealed class CredentialsNotFoundException : Exception
    {
        public CredentialsNotFoundException()
        {
        }

        public CredentialsNotFoundException(string message) : base(message)
        {
        }

        public CredentialsNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CredentialsNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
