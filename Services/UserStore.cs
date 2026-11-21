using System;
using System.Collections.Generic;
using System.Text;
using translator.Exceptions;
using translator.Models;
using System.Linq;

namespace translator.Services
{
    public class UserStore
    {
        private readonly string path;
        private readonly List<User> users = new();

        public UserStore(string path)
        {
            this.path = path;
            foreach (string raw in FileStorage.ReadLines(path))
            {
                string line = raw.Trim();
                if (line.Length == 0)
                {
                    continue;
                }
                User? user = User.FromLine(line);
                if (user != null)
                {
                    users.Add(user);
                }
            }
        }

        public User? Find(string username)
        {
            return users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
        }

        public User Authenticate(string username, string password)
        {
            User? user = Find(username);
            if (user == null || !user.CheckPassword(password))
            {
                throw new InvalidCredentialsException();
            }
            return user;
        }

        public User Create(string username, string password, Lang learning)
        {
            if (Find(username) != null)
            {
                throw new UserAlreadyExistsException(username);
            }
            User user = new User(username, User.Hash(username, password), Lang.Ka, learning, Texts.Get("nick0", learning));
            users.Add(user);
            Save();
            return user;
        }

        public void ResetProgress(User user)
        {
            user.Level = 1;
            user.Points = 0;
            user.ReadBooks.Clear();
            user.Nickname = Texts.Get("nick0", user.Learning);
            Save();
        }

        public void Delete(User user)
        {
            users.Remove(user);
            Save();
        }

        public void Save()
        {
            FileStorage.WriteLines(path, users.Select(u => u.ToLine()));
        }
    }
}
