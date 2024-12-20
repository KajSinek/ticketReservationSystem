﻿namespace TRS.CoreApi.Exceptions;

public class UserExceptions : Exception
{
    public UserExceptions() { }

    public UserExceptions(string message)
        : base(message) { }

    public UserExceptions(string message, Exception innerException)
        : base(message, innerException) { }
}
