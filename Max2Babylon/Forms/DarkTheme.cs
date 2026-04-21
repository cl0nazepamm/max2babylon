using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Max2Babylon
{
    /// <summary>
    /// Win11 Fluent-inspired dark theme for WinForms. Call <see cref="Apply"/> from a form's
    /// constructor after InitializeComponent(). Dark titlebar, rounded corners, and Mica backdrop
    /// are applied via DWM (no-op on Windows versions that don't support them).
    /// </summary>
    internal static class DarkTheme
    {
        public static readonly Color Background  = Color.FromArgb(32, 32, 32);
        public static readonly Color Surface     = Color.FromArgb(43, 43, 43);
        public static readonly Color SurfaceHi   = Color.FromArgb(54, 54, 54);
        public static readonly Color Border      = Color.FromArgb(64, 64, 64);
        public static readonly Color Text        = Color.FromArgb(240, 240, 240);
        public static readonly Color TextMuted   = Color.FromArgb(160, 160, 160);
        public static readonly Color Accent      = Color.FromArgb(0, 120, 212);
        public static readonly Color AccentHover = Color.FromArgb(26, 140, 232);
        public static readonly Color AccentDown  = Color.FromArgb(0, 95, 184);

        private static bool _appModeInitialized;

        public static void Apply(Form form)
        {
            if (form == null) return;
            InitAppMode();

            form.BackColor = Background;
            form.ForeColor = Text;
            form.Font = new Font("Segoe UI", 9f);

            EventHandler handler = null;
            handler = (s, e) => { ApplyWindowChrome(form); form.HandleCreated -= handler; };
            form.HandleCreated += handler;
            if (form.IsHandleCreated) ApplyWindowChrome(form);

            ApplyRecursive(form);
        }

        public static void ApplyRecursive(Control container)
        {
            foreach (Control c in container.Controls)
            {
                StyleControl(c);
                if (c.HasChildren) ApplyRecursive(c);
            }
        }

        public static void StylePrimary(Button b)
        {
            if (b == null) return;
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = Accent;
            b.ForeColor = Color.White;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = AccentHover;
            b.FlatAppearance.MouseDownBackColor = AccentDown;
            b.Font = new Font("Segoe UI Semibold", 9f);
            b.UseVisualStyleBackColor = false;
        }

        public static void StyleControl(Control c)
        {
            switch (c)
            {
                case Button btn:
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.BackColor = SurfaceHi;
                    btn.ForeColor = Text;
                    btn.FlatAppearance.BorderColor = Border;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(48, 48, 48);
                    btn.UseVisualStyleBackColor = false;
                    break;

                case CheckBox cb:
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.ForeColor = Text;
                    cb.BackColor = Color.Transparent;
                    cb.FlatAppearance.BorderColor = Border;
                    cb.FlatAppearance.CheckedBackColor = Accent;
                    break;

                case RadioButton rb:
                    rb.FlatStyle = FlatStyle.Flat;
                    rb.ForeColor = Text;
                    rb.BackColor = Color.Transparent;
                    rb.FlatAppearance.BorderColor = Border;
                    rb.FlatAppearance.CheckedBackColor = Accent;
                    break;

                case ComboBox co:
                    co.FlatStyle = FlatStyle.Flat;
                    co.BackColor = Surface;
                    co.ForeColor = Text;
                    break;

                case NumericUpDown nud:
                    nud.BackColor = Surface;
                    nud.ForeColor = Text;
                    nud.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case RichTextBox rtb:
                    rtb.BackColor = Surface;
                    rtb.ForeColor = Text;
                    rtb.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case TextBox tb:
                    tb.BackColor = Surface;
                    tb.ForeColor = Text;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case TreeView tv:
                    tv.BackColor = Surface;
                    tv.ForeColor = Text;
                    tv.BorderStyle = BorderStyle.None;
                    tv.LineColor = Border;
                    break;

                case ListBox lb:
                    lb.BackColor = Surface;
                    lb.ForeColor = Text;
                    lb.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ListView lv:
                    lv.BackColor = Surface;
                    lv.ForeColor = Text;
                    lv.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case LinkLabel ll:
                    ll.BackColor = Color.Transparent;
                    ll.LinkColor = Accent;
                    ll.ActiveLinkColor = AccentHover;
                    ll.VisitedLinkColor = Accent;
                    ll.ForeColor = Text;
                    break;

                case Label l:
                    l.ForeColor = Text;
                    l.BackColor = Color.Transparent;
                    break;

                case GroupBox gb:
                    gb.ForeColor = TextMuted;
                    gb.BackColor = Color.Transparent;
                    gb.Paint -= PaintGroupBox;
                    gb.Paint += PaintGroupBox;
                    break;

                case TabControl tc:
                    tc.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tc.SizeMode = TabSizeMode.Fixed;
                    tc.ItemSize = new Size(120, 28);
                    tc.Padding = new Point(12, 4);
                    tc.DrawItem -= DrawTabItem;
                    tc.DrawItem += DrawTabItem;
                    tc.BackColor = Background;
                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.BackColor = Background;
                        tp.ForeColor = Text;
                    }
                    break;

                case ProgressBar pb:
                    pb.ForeColor = Accent;
                    pb.BackColor = Surface;
                    break;

                case SplitContainer sc:
                    sc.BackColor = Border;
                    sc.Panel1.BackColor = Background;
                    sc.Panel2.BackColor = Background;
                    break;

                case StatusStrip ss:
                    ss.BackColor = SurfaceHi;
                    ss.ForeColor = Text;
                    break;

                case ToolStrip ts:
                    ts.BackColor = SurfaceHi;
                    ts.ForeColor = Text;
                    break;

                case Panel p:
                    p.ForeColor = Text;
                    if (p.BackColor == SystemColors.Control)
                        p.BackColor = Background;
                    break;

                default:
                    c.ForeColor = Text;
                    break;
            }
        }

        private static void PaintGroupBox(object sender, PaintEventArgs e)
        {
            var gb = (GroupBox)sender;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var client = gb.ClientRectangle;
            var headerH = string.IsNullOrEmpty(gb.Text) ? 0 : (int)(gb.Font.GetHeight(g) / 2f);

            var border = new Rectangle(client.X, client.Y + headerH, client.Width - 1, client.Height - headerH - 1);
            using (var path = RoundedRect(border, 6))
            using (var pen = new Pen(Border))
                g.DrawPath(pen, path);

            if (!string.IsNullOrEmpty(gb.Text))
            {
                var textSize = g.MeasureString(gb.Text, gb.Font);
                var textRect = new RectangleF(client.X + 10, client.Y, textSize.Width + 8, textSize.Height);
                var parentColor = gb.Parent != null ? gb.Parent.BackColor : Background;
                using (var bg = new SolidBrush(parentColor))
                    g.FillRectangle(bg, textRect);
                using (var br = new SolidBrush(TextMuted))
                    g.DrawString(gb.Text, gb.Font, br, client.X + 14, client.Y);
            }
        }

        private static void DrawTabItem(object sender, DrawItemEventArgs e)
        {
            var tc = (TabControl)sender;
            if (e.Index < 0 || e.Index >= tc.TabPages.Count) return;

            var page = tc.TabPages[e.Index];
            var tabRect = tc.GetTabRect(e.Index);
            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var g = e.Graphics;

            using (var bg = new SolidBrush(selected ? SurfaceHi : Background))
                g.FillRectangle(bg, tabRect);

            if (selected)
            {
                var underline = new Rectangle(tabRect.X + 10, tabRect.Bottom - 3, tabRect.Width - 20, 2);
                using (var br = new SolidBrush(Accent))
                    g.FillRectangle(br, underline);
            }

            var fmt = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };
            using (var br = new SolidBrush(selected ? Text : TextMuted))
                g.DrawString(page.Text, tc.Font, br, tabRect, fmt);
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        #region DWM window chrome

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE  = 20;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWA_SYSTEMBACKDROP_TYPE      = 38;
        private const int DWMWCP_ROUND                   = 2;
        private const int DWMSBT_MAINWINDOW              = 2;

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private static void ApplyWindowChrome(Form form)
        {
            var hwnd = form.Handle;
            int value;

            value = 1;
            TrySetAttr(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref value);

            value = DWMWCP_ROUND;
            TrySetAttr(hwnd, DWMWA_WINDOW_CORNER_PREFERENCE, ref value);

            value = DWMSBT_MAINWINDOW;
            TrySetAttr(hwnd, DWMWA_SYSTEMBACKDROP_TYPE, ref value);
        }

        private static void TrySetAttr(IntPtr hwnd, int attr, ref int value)
        {
            try { DwmSetWindowAttribute(hwnd, attr, ref value, sizeof(int)); }
            catch { /* older Windows / missing dwmapi attribute — ignore */ }
        }

        #endregion

        private static void InitAppMode()
        {
            if (_appModeInitialized) return;
            _appModeInitialized = true;
#if MAX2027
            try { Application.SetColorMode(SystemColorMode.Dark); } catch { }
#endif
        }
    }
}
