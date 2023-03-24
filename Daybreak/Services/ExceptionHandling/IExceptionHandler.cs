using System;

namespace Daybreak.Services.ExceptionHandling;

public interface IExceptionHandler
{
    bool HandleException(Exception exception);
}
