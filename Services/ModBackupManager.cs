using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossworldsModManager
{
    public static class ModBackupManager
    {
        public static void BackupMods(string sourceModDirectory)
        {
            var progressForm = new ProgressForm("Backing up Mods");
            progressForm.Shown += (sender, e) =>
                Task.Run(() => CopyDirectoryContents(sourceModDirectory, PlatformUtils.GetModBackupDir(), "Backing up", "Backup", progressForm));
            progressForm.ShowDialog();
        }

        public static void RestoreModsFromBackup(string destinationModDirectory)
        {
            var progressForm = new ProgressForm("Restoring Mods");
            progressForm.Shown += (sender, e) =>
                Task.Run(() => CopyDirectoryContents(PlatformUtils.GetModBackupDir(), destinationModDirectory, "Restoring", "Restore", progressForm));
            progressForm.ShowDialog();
        }

        private static void CopyDirectoryContents(string sourceDir, string destDir, string actionLabel, string actionName, ProgressForm form)
        {
            try
            {
                if (!Directory.Exists(sourceDir))
                {
                    form.ShowCompletion($"{actionName} failed: Source directory not found.");
                    return;
                }

                Directory.CreateDirectory(destDir);

                var files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
                int totalFiles = files.Length;
                int processed = 0;
                var token = form.TokenSource?.Token ?? CancellationToken.None;

                foreach (string file in files)
                {
                    if (token.IsCancellationRequested)
                    {
                        form.ShowCompletion($"{actionName} skipped.");
                        return;
                    }

                    var relativePath = Path.GetRelativePath(sourceDir, file);
                    if (Path.IsPathRooted(relativePath))
                        relativePath = Path.GetFileName(file);

                    string destFile = Path.Combine(destDir, relativePath);
                    string? destDirPath = Path.GetDirectoryName(destFile);

                    if (destDirPath != null && !Directory.Exists(destDirPath))
                        Directory.CreateDirectory(destDirPath);

                    File.Copy(file, destFile, true);

                    processed++;
                    int percentage = (int)((processed / (float)totalFiles) * 100);
                    form.UpdateStatus($"{actionLabel}: {Path.GetFileName(file)}");
                    form.UpdateProgress(percentage);
                }

                form.ShowCompletion($"{actionName} completed successfully!");
            }
            catch (Exception ex)
            {
                form.ShowCompletion($"Error: {ex.Message}");
            }
        }
    }
}
