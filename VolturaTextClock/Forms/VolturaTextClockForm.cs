using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using VolturaTextClock.Library;
using static VolturaTextClock.Program;

namespace VolturaTextClock
{
    public partial class VolturaTextClockForm : Form
    {
        private TextClockTheme theme;
        private Image clockBackground;

        public VolturaTextClockForm()
        {
            InitializeComponent();
            AutoSize = false;
            ShowInTaskbar = true;
            MinimizeBox = true;
            clockPicBox.MinimumSize = Size.Empty;
            clockPicBox.Dock = DockStyle.Fill;
            clockPicBox.MoveOtherWithMouse(this);
            clockPicBox.SendToBack();
            foreach (var picture in new[] { optionsPicBox, settingsPicBox, minimizePicBox, closePicBox })
            { picture.BackgroundImageLayout = ImageLayout.Zoom; var target = picture; ImageOverlay.InitializeImage(ref target); }
            DpiChanged += (_, _) => { if (IsHandleCreated) BeginInvoke(new Action(() => RenderClock())); };
            Resize += (_, _) => { if (theme != null) RenderClock(); };
            VisibleChanged += (_, _) => { if (theme != null) RenderClock(); };
            FormClosed += (_, _) => { clockTimer.Stop(); clockBackground?.Dispose(); clockPicBox.Image?.Dispose(); };
            if (AppConfig.GetValue("startMinimized", false)) WindowState = FormWindowState.Minimized;
        }

        private void ApplySettings()
        {
            theme = ClockAppearance.LoadTheme();
            clockBackground?.Dispose();
            clockBackground = ClockAppearance.LoadBackground(theme);
            TopMost = AppConfig.GetValue("alwaysOnTop", false);
            pinPicBox.Image = TopMost ? Properties.Resources.unpin : Properties.Resources.pin;
            RenderClock();
        }

        private void RenderClock()
        {
            if (IsDisposed || theme == null || !Visible || WindowState == FormWindowState.Minimized || clockPicBox.Width <= 0 || clockPicBox.Height <= 0)
            { clockTimer.Stop(); return; }
            var now = DateTime.Now;
            float brightness = theme.Flicker ? .88f + .12f * (float)Math.Sin(now.TimeOfDay.TotalSeconds * 4) : 1;
            var image = TextClock.Render(theme, clockPicBox.ClientSize, now, clockBackground, brightness);
            var previous = clockPicBox.Image;
            clockPicBox.Image = image;
            previous?.Dispose();
            clockTimer.Interval = theme.Flicker ? 125 : 60000 - now.Second * 1000 - now.Millisecond;
            clockTimer.Start();
        }

        private void VolturaTextClockForm_Load(object sender, EventArgs e)
        {
            string[] coordinates = AppConfig.GetValue("mainFormLocation", "40,40").Split(',');
            if (coordinates.Length == 2 && int.TryParse(coordinates[0], out int x) && int.TryParse(coordinates[1], out int y))
            {
                var proposed = new Rectangle(x, y, Width, Height);
                if (Array.Exists(Screen.AllScreens, screen => screen.WorkingArea.IntersectsWith(proposed))) Location = proposed.Location;
            }
            ApplySettings();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!AppConfig.GetValue("automaticUpdateCheck", false)) return;
            try
            {
                var version = await UpdateChecker.GetNewerVersionAsync(typeof(VolturaTextClockForm).Assembly.GetName().Version);
                if (version != null && !IsDisposed && MessageBox.Show(this, $"VolturaTextClock {version} is available. Open the download page?", "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(UpdateChecker.ReleasesUrl) { UseShellExecute = true });
            }
            catch (Exception ex) { Log.Error = ex; }
        }

        private void ShowSettingsForm()
        {
            clockTimer.Stop();
            using var settings = new SettingsForm();
            bool pinned = TopMost;
            TopMost = false;
            settings.ShowDialog(this);
            TopMost = pinned;
            ApplySettings();
        }

        private void ToggleButtons() => settingsPicBox.Visible = pinPicBox.Visible = closePicBox.Visible = minimizePicBox.Visible = !minimizePicBox.Visible;
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            RenderClock();
        }
        private void OptionsPicBox_Click(object sender, EventArgs e) => ToggleButtons();
        private void SettingsPicBox_Click(object sender, EventArgs e) => ShowSettingsForm();
        private void PictureBox_MouseLeaveOrEnter(object sender, EventArgs e) => ImageOverlay.SwitchImage((PictureBox)sender);
        private void PinPicBox_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            pinPicBox.Image = TopMost ? Properties.Resources.unpin : Properties.Resources.pin;
            AppConfig.AddOrUpdateAppSetting("alwaysOnTop", TopMost);
            ToggleButtons();
        }
        private void MinimizePicBox_Click(object sender, EventArgs e) { ToggleButtons(); WindowState = FormWindowState.Minimized; }
        private void ClosePicBox_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to close the application?", "Close application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close();
        }
        private void VolturaTextClockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var position = WindowState == FormWindowState.Normal ? Location : RestoreBounds.Location;
            AppConfig.AddOrUpdateAppSetting("mainFormLocation", $"{position.X},{position.Y}");
        }
        protected override CreateParams CreateParams
        {
            get { var parameters = base.CreateParams; parameters.ClassStyle |= 0x20000; return parameters; }
        }
    }
}
