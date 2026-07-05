using System;
using System.Drawing;
using System.Windows.Forms;

namespace CrossworldsModManager
{
    public static class UiHelpers
    {
        public static Button CreateFlatButton(string text, int width = 80, Color? backColor = null, EventHandler? onClick = null)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(width, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = backColor ?? Color.FromArgb(63, 63, 70),
                UseVisualStyleBackColor = false,
                Font = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont
            };
            btn.FlatAppearance.BorderSize = 0;
            if (onClick != null)
                btn.Click += onClick;
            return btn;
        }

        public static Button CreateAccentButton(string text, int width = 80, EventHandler? onClick = null)
        {
            return CreateFlatButton(text, width, Color.FromArgb(0, 122, 204), onClick);
        }

        public static void ApplyButtonTheme(Button btn, bool isAccent = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = isAccent ? Color.FromArgb(0, 122, 204) : Color.FromArgb(63, 63, 70);
            btn.UseVisualStyleBackColor = false;
            btn.Font = SystemFonts.MessageBoxFont ?? SystemFonts.DefaultFont;
        }

        public static void SetDarkTheme(this Control c)
        {
            if (c is TextBox || c is RichTextBox || c is ListBox || c is ComboBox || c is CheckedListBox)
            {
                c.BackColor = Color.FromArgb(30, 30, 30);
                c.ForeColor = Color.White;
            }
        }

        public static ToolStripMenuItem CreateMenuItem(string text, EventHandler onClick)
        {
            return new ToolStripMenuItem(text)
            {
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48)
            };
        }
    }
}
