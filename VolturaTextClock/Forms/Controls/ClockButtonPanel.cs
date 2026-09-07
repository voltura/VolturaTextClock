using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VolturaTextClock.Forms.Controls
{
    internal sealed class ClockButtonPanel : Panel
    {
        public ClockButtonPanel()
        {
            DoubleBuffered = true;
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Width < 2 || Height < 2) return;
            float radius = System.Math.Min(8 * DeviceDpi / 96f, System.Math.Min(Width, Height) / 2f);
            using var outline = new GraphicsPath();
            outline.AddArc(.5f, .5f, radius * 2, radius * 2, 180, 90);
            outline.AddLine(radius + .5f, .5f, Width - .5f, .5f);
            outline.AddLine(Width - .5f, .5f, Width - .5f, Height - .5f);
            outline.AddLine(Width - .5f, Height - .5f, .5f, Height - .5f);
            outline.CloseFigure();
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(Brushes.Black, outline);
            using var border = new Pen(Color.FromArgb(85, 85, 85));
            e.Graphics.DrawPath(border, outline);
        }
    }
}
