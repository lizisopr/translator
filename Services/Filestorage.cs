using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace translator.Services
{
    public static class FileStorage
    {
        public static string DataDir => Path.Combine(AppContext.BaseDirectory, "Data");

        public static string[] ReadLines(string path)
        {
            if (!File.Exists(path))
            {
                return Array.Empty<string>();
            }
            return File.ReadAllLines(path, Encoding.UTF8);
        }

        public static void WriteLines(string path, IEnumerable<string> lines)
        {
            string? directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllLines(path, lines, new UTF8Encoding(false));
        }
    }
}
