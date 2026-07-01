using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NativeFileDialogs.Net;

namespace CrossworldsModManager
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            var platformType = SettingsManager.Settings.PreferredLaunchPlatform;
            cmbPlatformType.SelectedItem = (platformType == "Custom") ? "Custom" : "Steam";

            if (platformType == "Custom")
            {
                txtGameDir.Text = SettingsManager.Settings.CustomUnionDirectory;
            }
            else if (!string.IsNullOrEmpty(SettingsManager.Settings.GameDirectory) && !string.IsNullOrEmpty(SettingsManager.Settings.GameExecutableName))
            {
                txtGameDir.Text = Path.Combine(SettingsManager.Settings.GameDirectory, SettingsManager.Settings.GameExecutableName);
            }
            else
            {
                txtGameDir.Text = SettingsManager.Settings.GameDirectory;
            }

            txtModsDir.Text = SettingsManager.Settings.ModsDirectory;
            chkSortEnabled.Checked = SettingsManager.Settings.SortEnabledModsToTop;
            chkAutoClean.Checked = SettingsManager.Settings.AutoCleanTemporaryFiles;
            chkCheckForGames.Checked = SettingsManager.Settings.CheckForGamesOnStartup;
            chkAutoCloseLog.Checked = SettingsManager.Settings.AutoCloseLogOnSuccess;
            chkDeveloperMode.Checked = SettingsManager.Settings.DeveloperModeEnabled;
            var doNotBackupChk = this.Controls.Find("chkDoNotBackup", true);
            if (doNotBackupChk.Length > 0 && doNotBackupChk[0] is CheckBox chkBackup)
            {
                chkBackup.Checked = SettingsManager.Settings.DoNotBackupModsAutomatically;
            }
            var doNotConfirmChk = this.Controls.Find("chkDoNotConfirmEnableDisable", true);
            if (doNotConfirmChk.Length > 0 && doNotConfirmChk[0] is CheckBox chkConfirm)
            {
                chkConfirm.Checked = SettingsManager.Settings.DoNotConfirmEnableDisable;
            }
            var doNotWarnChk = this.Controls.Find("chkDoNotWarnUnsavedChanges", true);
            if (doNotWarnChk.Length > 0 && doNotWarnChk[0] is CheckBox chkWarn)
            {
                chkWarn.Checked = SettingsManager.Settings.DoNotWarnUnsavedChanges;
            }

            var cmbThemeControl = this.Controls.Find("cmbTheme", true);
            if (cmbThemeControl.Length > 0 && cmbThemeControl[0] is ComboBox cmbTheme)
            {
                cmbTheme.Items.Clear();
                cmbTheme.Items.AddRange(ThemeManager.GetAvailableThemes().ToArray());
                cmbTheme.SelectedItem = SettingsManager.Settings.SelectedTheme;
                UpdateCustomizeButtonVisibility();
            }

            ThemeManager.ApplyTheme(this);
        }

        private void btnBrowseGameDir_Click(object sender, EventArgs e)
        {
            bool isCustom = cmbPlatformType.SelectedItem?.ToString() == "Custom";

            if (isCustom)
            {
                CustomMessageBox.Show(
                    "Please select the 'UNION' folder inside your game installation directory.\n\n" +
                    "This is typically located at:\n" +
                    "  [Game Root]/UNION\n\n" +
                    "The app will automatically detect the executable and other paths from there.",
                    "Select UNION Folder",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                NfdStatus result = Nfd.PickFolder(out string? dirPath);

                if (result == NfdStatus.Ok && dirPath != null)
                {
                    var dirName = new DirectoryInfo(dirPath).Name;
                    if (!dirName.Equals("UNION", StringComparison.OrdinalIgnoreCase))
                    {
                        CustomMessageBox.Show(
                            "The selected folder must be named 'UNION'.\n\n" +
                            "Please select the UNION folder inside your game installation.",
                            "Invalid Selection",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                    txtGameDir.Text = dirPath;
                }
            }
            else
            {
                Dictionary<string, string> filters = new Dictionary<string, string>
                {
                    {"Game Executable", "exe"}
                };
                NfdStatus result = Nfd.OpenDialog(out string? fileName, filters);

                if (result == NfdStatus.Ok)
                {
                    if (fileName == null) return;
                    txtGameDir.Text = fileName;
                }
            }
        }

        private void btnBrowseModsDir_Click(object sender, EventArgs e)
        {
            NfdStatus result = Nfd.PickFolder(out string? dirPath);

            while (true)
            {
                if (result == NfdStatus.Ok)
                {
                    if (dirPath == null) continue;
                    var dirName = new DirectoryInfo(dirPath).Name;
                    if (dirName.Equals("~mods", StringComparison.OrdinalIgnoreCase))
                    {
                        CustomMessageBox.Show("You cannot select the game's '~mods' folder as your mod storage directory.\n\nThis folder is used by the manager to install mods. Please select a different folder to store your source mods.", "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }
                    txtModsDir.Text = dirPath;
                }
                break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isCustom = cmbPlatformType.SelectedItem?.ToString() == "Custom";
            SettingsManager.Settings.PreferredLaunchPlatform = isCustom ? "Custom" : "Steam";

            if (isCustom)
            {
                SettingsManager.Settings.CustomUnionDirectory = txtGameDir.Text;
                SettingsManager.Settings.GameDirectory = null;
                SettingsManager.Settings.GameExecutableName = null;
            }
            else
            {
                SettingsManager.Settings.CustomUnionDirectory = null;
                string inputPath = txtGameDir.Text;
                if (File.Exists(inputPath))
                {
                    SettingsManager.Settings.GameDirectory = Path.GetDirectoryName(inputPath);
                    SettingsManager.Settings.GameExecutableName = Path.GetFileName(inputPath);
                }
                else
                {
                    SettingsManager.Settings.GameDirectory = inputPath;
                    if (string.IsNullOrEmpty(SettingsManager.Settings.GameExecutableName))
                        SettingsManager.Settings.GameExecutableName = "SonicRacingCrossWorlds.exe";
                }
            }

            SettingsManager.Settings.ModsDirectory = txtModsDir.Text;
            SettingsManager.Settings.SortEnabledModsToTop = chkSortEnabled.Checked;
            SettingsManager.Settings.AutoCleanTemporaryFiles = chkAutoClean.Checked;
            SettingsManager.Settings.CheckForGamesOnStartup = chkCheckForGames.Checked;
            SettingsManager.Settings.AutoCloseLogOnSuccess = chkAutoCloseLog.Checked;
            SettingsManager.Settings.DeveloperModeEnabled = chkDeveloperMode.Checked;
            var doNotBackupChk2 = this.Controls.Find("chkDoNotBackup", true);
            if (doNotBackupChk2.Length > 0 && doNotBackupChk2[0] is CheckBox chkBackup)
            {
                SettingsManager.Settings.DoNotBackupModsAutomatically = chkBackup.Checked;
            }
            var doNotConfirmChk2 = this.Controls.Find("chkDoNotConfirmEnableDisable", true);
            if (doNotConfirmChk2.Length > 0 && doNotConfirmChk2[0] is CheckBox chkConfirm)
            {
                SettingsManager.Settings.DoNotConfirmEnableDisable = chkConfirm.Checked;
            }
            var doNotWarnChk2 = this.Controls.Find("chkDoNotWarnUnsavedChanges", true);
            if (doNotWarnChk2.Length > 0 && doNotWarnChk2[0] is CheckBox chkWarn)
            {
                SettingsManager.Settings.DoNotWarnUnsavedChanges = chkWarn.Checked;
            }

            var cmbThemeControl = this.Controls.Find("cmbTheme", true);
            if (cmbThemeControl.Length > 0 && cmbThemeControl[0] is ComboBox cmbTheme && cmbTheme.SelectedItem != null)
            {
                SettingsManager.Settings.SelectedTheme = cmbTheme.SelectedItem.ToString() ?? "Default";
            }
            SettingsManager.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCustomizeButtonVisibility();
        }

        private void UpdateCustomizeButtonVisibility()
        {
            var cmbThemeControl = this.Controls.Find("cmbTheme", true);
            var btnCustomizeControl = this.Controls.Find("btnCustomizeTheme", true);
            if (cmbThemeControl.Length > 0 && cmbThemeControl[0] is ComboBox cmbTheme && 
                btnCustomizeControl.Length > 0 && btnCustomizeControl[0] is Button btnCustomize)
            {
                btnCustomize.Visible = (cmbTheme.SelectedItem?.ToString() == "Custom");
            }
        }

        private void btnCustomizeTheme_Click(object sender, EventArgs e)
        {
            using (var editor = new ThemeEditorForm(SettingsManager.Settings.CustomTheme))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    SettingsManager.Settings.CustomTheme = editor.ResultTheme;
                    ThemeManager.ReloadCustomTheme(SettingsManager.Settings.CustomTheme);
                    ThemeManager.ApplyTheme(this);
                }
                else
                {
                    ThemeManager.ReloadCustomTheme(SettingsManager.Settings.CustomTheme);
                    ThemeManager.ApplyTheme(this);
                }
            }
        }
    }
}
