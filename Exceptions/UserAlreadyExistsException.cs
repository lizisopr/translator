using System;
using System.Collections.Generic;
using System.Text;

namespace translator.Exceptions
{
    public class UserAlreadyExistsException : LinguaException
    {
        public string Username { get; }

        public UserAlreadyExistsException(string username)
        : base($"User \"{username}\" already exists.")
        {
            Username = username;
        }
    }
}
