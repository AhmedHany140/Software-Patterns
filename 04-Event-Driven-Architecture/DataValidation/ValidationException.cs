using System;

namespace OrderingSystem.DataValidation;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
