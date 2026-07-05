using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CrossworldsModManager
{
#pragma warning disable CA1416
    public class DynamicThemeMenuRenderer : ToolStripProfessionalRenderer
    {
        public DynamicThemeMenuRenderer(ProfessionalColorTable colorTable) : base(colorTable)
        {
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is MenuStrip)
            {
                using (var brush = new SolidBrush(ThemeManager.CurrentTheme.MenuBackColor))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
            else if (e.ToolStrip is ToolStripDropDown)
            {
                using (var brush = new SolidBrush(ThemeManager.CurrentTheme.ControlBackColor))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
            {
                using (var pen = new Pen(ThemeManager.CurrentTheme.BorderColor))
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1));
            }
            else
            {
                base.OnRenderToolStripBorder(e);
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item is ToolStripMenuItem item)
            {
                if (item.Owner is MenuStrip)
                {
                    if (item.Selected || item.Pressed)
                    {
                        var bg = Color.FromArgb(80, ThemeManager.CurrentTheme.AccentColor);
                        using (var brush = new SolidBrush(bg))
                            e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
                    }
                }
                else if (item.Owner is ToolStripDropDown)
                {
                    if (item.Selected || item.Pressed)
                    {
                        using (var brush = new SolidBrush(ThemeManager.CurrentTheme.ButtonBackColor))
                            e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
                        using (var pen = new Pen(ThemeManager.CurrentTheme.AccentColor))
                            e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1));
                    }
                }
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? ThemeManager.CurrentTheme.MenuForeColor : Color.Gray;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            using (var brush = new SolidBrush(ThemeManager.CurrentTheme.ControlBackColor))
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = e.ImageRectangle;
            var center = new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);

            using (var pen = new Pen(ThemeManager.CurrentTheme.MenuForeColor, 2))
            {
                g.DrawLines(pen, new PointF[]
                {
                    new PointF(center.X - 4.5f, center.Y - 1.5f),
                    new PointF(center.X - 1.5f, center.Y + 3.5f),
                    new PointF(center.X + 4.5f, center.Y - 4.5f)
                });
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var y = e.Item.Height / 2;
            using (var pen = new Pen(ThemeManager.CurrentTheme.BorderColor))
                e.Graphics.DrawLine(pen, 4, y, e.Item.Width - 4, y);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = ThemeManager.CurrentTheme.MenuForeColor;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            using (var brush = new SolidBrush(ThemeManager.CurrentTheme.BorderColor))
                e.Graphics.FillRectangle(brush, e.GripBounds);
        }
    }
#pragma warning restore CA1416
}
