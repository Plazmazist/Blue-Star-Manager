using System.Drawing;
using System.Windows.Forms;

namespace CrossworldsModManager
{
    public class DynamicThemeColorTable : ProfessionalColorTable
    {
        // Button states
        public override Color ButtonPressedBorder => ThemeManager.CurrentTheme.AccentColor;
        public override Color ButtonPressedGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ButtonPressedGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ButtonPressedGradientMiddle => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ButtonPressedHighlight => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ButtonPressedHighlightBorder => ThemeManager.CurrentTheme.AccentColor;
        public override Color ButtonSelectedBorder => ThemeManager.CurrentTheme.AccentColor;
        public override Color ButtonSelectedGradientBegin => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonSelectedGradientEnd => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonSelectedGradientMiddle => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonSelectedHighlight => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonSelectedHighlightBorder => ThemeManager.CurrentTheme.AccentColor;

        // Checked items (checkboxes in toolstrip buttons)
        public override Color ButtonCheckedGradientBegin => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonCheckedGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ButtonCheckedGradientMiddle => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonCheckedHighlight => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color ButtonCheckedHighlightBorder => ThemeManager.CurrentTheme.AccentColor;

        // Check marks in dropdown menus
        public override Color CheckBackground => ThemeManager.CurrentTheme.AccentColor;
        public override Color CheckPressedBackground => ThemeManager.CurrentTheme.AccentColor;
        public override Color CheckSelectedBackground => ThemeManager.CurrentTheme.AccentColor;

        // Grip (reorder handle)
        public override Color GripDark => ThemeManager.CurrentTheme.BorderColor;
        public override Color GripLight => ThemeManager.CurrentTheme.BackColor;

        // Image margin (icon area in dropdowns)
        public override Color ImageMarginGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ImageMarginGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ImageMarginGradientMiddle => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ImageMarginRevealedGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ImageMarginRevealedGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ImageMarginRevealedGradientMiddle => ThemeManager.CurrentTheme.ControlBackColor;

        // Menu borders
        public override Color MenuBorder => ThemeManager.CurrentTheme.BorderColor;
        public override Color MenuItemBorder => ThemeManager.CurrentTheme.AccentColor;
        public override Color MenuItemPressedGradientBegin => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color MenuItemPressedGradientEnd => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color MenuItemPressedGradientMiddle => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color MenuItemSelected => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color MenuItemSelectedGradientBegin => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color MenuItemSelectedGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;

        // MenuStrip gradient
        public override Color MenuStripGradientBegin => ThemeManager.CurrentTheme.MenuBackColor;
        public override Color MenuStripGradientEnd => ThemeManager.CurrentTheme.MenuBackColor;

        // Overflow button (the chevron when toolstrip overflows)
        public override Color OverflowButtonGradientBegin => ThemeManager.CurrentTheme.ButtonBackColor;
        public override Color OverflowButtonGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color OverflowButtonGradientMiddle => ThemeManager.CurrentTheme.ButtonBackColor;

        // Rafting container (used when dragging toolstrips)
        public override Color RaftingContainerGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color RaftingContainerGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;

        // Separators
        public override Color SeparatorDark => ThemeManager.CurrentTheme.BorderColor;
        public override Color SeparatorLight => ThemeManager.CurrentTheme.ButtonBackColor;

        // StatusStrip gradient
        public override Color StatusStripGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color StatusStripGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;

        // ToolStrip gradients
        public override Color ToolStripBorder => ThemeManager.CurrentTheme.BorderColor;
        public override Color ToolStripContentPanelGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ToolStripContentPanelGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ToolStripDropDownBackground => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ToolStripGradientBegin => ThemeManager.CurrentTheme.MenuBackColor;
        public override Color ToolStripGradientEnd => ThemeManager.CurrentTheme.MenuBackColor;
        public override Color ToolStripGradientMiddle => ThemeManager.CurrentTheme.MenuBackColor;
        public override Color ToolStripPanelGradientBegin => ThemeManager.CurrentTheme.ControlBackColor;
        public override Color ToolStripPanelGradientEnd => ThemeManager.CurrentTheme.ControlBackColor;
    }
}
