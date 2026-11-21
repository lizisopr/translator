using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace translator.Models
{
    public class User
    {
        public const int MaxLevel = 8;

        private readonly string passwordHash;

        public string Username { get; }
        public Lang Native { get; }
        public Lang Learning { get; }
        public string Nickname { get; set; }
        public int Level { get; set; } = 1;
        public int Points { get; set; }
        public HashSet<int> ReadBooks { get; } = new();

        public bool Completed => Level > MaxLevel;

        public User(string username, string passwordHash, Lang native, Lang learning, string nickname)
        {
            Username = username;
            this.passwordHash = passwordHash;
            Native = native;
            Learning = learning;
            Nickname = nickname;
        }

        public bool CheckPassword(string password)
        {
            return passwordHash == Hash(Username, password);
        }

        public static string Hash(string username, string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(username.ToLowerInvariant() + ":" + password));
            return Convert.ToHexString(bytes);
        }

        public string ToLine()
        {
            string books = string.Join(",", ReadBooks.OrderBy(b => b));
            return string.Join("|", Username, passwordHash, Nickname.Replace('|', ' '), (int)Native, (int)Learning, Level, Points, books);
        }

        public static User? FromLine(string line)
        {
            string[] p = line.Split('|');
            if (p.Length != 7 && p.Length != 8)
            {
                return null;
            }
            if (!int.TryParse(p[3], out int native) || !int.TryParse(p[4], out int learning)
                || !int.TryParse(p[5], out int level) || !int.TryParse(p[6], out int points))
            {
                return null;
            }
            if (!Enum.IsDefined(typeof(Lang), native) || !Enum.IsDefined(typeof(Lang), learning))
            {
                return null;
            }
            User user = new User(p[0], p[1], (Lang)native, (Lang)learning, p[2]);
            user.Level = Math.Clamp(level, 1, MaxLevel + 1);
            user.Points = Math.Max(points, 0);
            if (p.Length == 8)
            {
                foreach (string part in p[7].Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(part, out int bookIndex) && bookIndex >= 0)
                    {
                        user.ReadBooks.Add(bookIndex);
                    }
                }
            }
            return user;
        }
    }
}
