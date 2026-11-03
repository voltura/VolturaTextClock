using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VolturaTextClock.Forms.Controls;

internal sealed class ClockActionButton : Button
{
    private readonly string action;
    private bool hover;
    internal ClockActionButton(string action)
    {
        this.action = action;
        AccessibleName = UiText.Get(action);
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Color.FromArgb(23, 28, 35);
        Cursor = Cursors.Hand;
        TabStop = true;
        MouseEnter += (_, _) => { hover = true; Invalidate(); };
        MouseLeave += (_, _) => { hover = false; Invalidate(); };
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(hover || Focused ? Color.FromArgb(53, 61, 72) : BackColor);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.ScaleTransform(Width / 36f, Height / 36f);
        using var pen = new Pen(hover || Focused ? Color.FromArgb(255, 225, 166) : Color.FromArgb(221, 225, 230), 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        var g = e.Graphics;
        switch (action)
        {
            case "Options":
                for (int y = 12; y <= 24; y += 6) g.DrawLine(pen, 11, y, 25, y);
                break;
            case "Close": g.DrawLine(pen, 12, 12, 24, 24); g.DrawLine(pen, 24, 12, 12, 24); break;
            case "Minimize": g.DrawLine(pen, 11, 23, 25, 23); break;
            case "Pin":
                g.DrawLines(pen, new[] { new Point(15, 10), new Point(25, 20), new Point(22, 20), new Point(18, 24), new Point(12, 18), new Point(16, 14), new Point(15, 10) });
                g.DrawLine(pen, 15, 21, 10, 26); break;
            default:
                for (int x = 12; x <= 24; x += 6) g.DrawLine(pen, x, 10, x, 26);
                g.DrawLine(pen, 9, 15, 15, 15); g.DrawLine(pen, 15, 22, 21, 22); g.DrawLine(pen, 21, 14, 27, 14); break;
        }
        if (Focused) { using var focus = new Pen(Color.FromArgb(255, 225, 166), 1); g.DrawRectangle(focus, 2, 2, 31, 31); }
    }
}
