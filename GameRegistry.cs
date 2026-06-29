using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrossworldsModManager
{
    public static class GameRegistry
    {
        public const string SteamAppId = "2486820";

        public static Dictionary<string, (string Path, string? AppName)> FindGameInstallations()
        {
            if (PlatformUtils.IsLinux)
                return FindLinuxInstallations();
            return FindWindowsInstallations();
        }

        private static Dictionary<string, (string Path, string? AppName)> FindWindowsInstallations()
        {
            var installations = new Dictionary<string, (string Path, string? AppName)>();
            var steamPath = FindSteamPathWindows();
            if (!string.IsNullOrEmpty(steamPath))
                installations["Steam"] = (steamPath, null);
            return installations;
        }

        private static Dictionary<string, (string Path, string? AppName)> FindLinuxInstallations()
        {
            var installations = new Dictionary<string, (string Path, string? AppName)>();
            var steamPath = FindSteamPathLinux();
            if (!string.IsNullOrEmpty(steamPath))
                installations["Steam"] = (steamPath, null);
            return installations;
        }

        private static string? FindSteamPathWindows()
        {
            try
            {
                using var steamKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam");
                var steamInstallPath = steamKey?.GetValue("InstallPath") as string;
                if (string.IsNullOrEmpty(steamInstallPath)) return null;

                return FindGameInSteamLibraries(steamInstallPath);
            }
            catch
            {
                return null;
            }
        }

        private static string? FindSteamPathLinux()
        {
            try
            {
                var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var steamInstallPath =
                    Directory.Exists(Path.Combine(userProfile, ".local", "share", "Steam"))
                        ? Path.Combine(userProfile, ".local", "share", "Steam")
                        : Path.Combine(userProfile, ".steam", "steam");

                return FindGameInSteamLibraries(steamInstallPath);
            }
            catch
            {
                return null;
            }
        }

        private static string? FindGameInSteamLibraries(string steamInstallPath)
        {
            var libraryFoldersFile = Path.Combine(steamInstallPath, "steamapps", "libraryfolders.vdf");
            if (!File.Exists(libraryFoldersFile)) return null;

            var libraryPaths = new List<string> { steamInstallPath };
            var vdfContent = File.ReadAllText(libraryFoldersFile);
            var matches = Regex.Matches(vdfContent, @"""path""\s+""(.+?)""");
            libraryPaths.AddRange(matches.Cast<Match>().Select(m => m.Groups[1].Value.Replace(@"\\", @"\")));

            foreach (var libraryPath in libraryPaths.Distinct())
            {
                var appManifestPath = Path.Combine(libraryPath, "steamapps", $"appmanifest_{SteamAppId}.acf");
                if (File.Exists(appManifestPath))
                {
                    var manifestContent = File.ReadAllText(appManifestPath);
                    var match = Regex.Match(manifestContent, @"""installdir""\s+""(.+?)""");
                    if (match.Success)
                        return Path.Combine(libraryPath, "steamapps", "common", match.Groups[1].Value);
                }
            }

            return null;
        }
    }
}
