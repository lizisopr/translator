using System;

namespace translator.Exceptions
{
    public class InvalidCredentialsException : LinguaException
    {
        public InvalidCredentialsException() : base("Invalid username or password.")
        {
        }
    }
}
