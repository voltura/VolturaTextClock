using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VolturaTextClock.Library;
using static VolturaTextClock.Program;

namespace VolturaTextClock
{
    public partial class SettingsForm : Form
    {
        private TextClockTheme theme;
        private PictureBox preview;
        private CheckBox startWithWindows, startMinimized, alwaysOnTop, automaticUpdates, glow, flicker;
        private ComboBox language, background;
        private Label backgroundPath;
        private Button activeFont, inactiveFont;
        private bool initializing = true;
        private readonly Timer previewTimer = new() { Interval = 125 };
        private Image previewBackground;

        public SettingsForm()
        {
            theme = ClockAppearance.LoadTheme();
            InitializeComponent();
            previewTimer.Tick += (_, _) => RenderPreview();
            VisibleChanged += (_, _) => UpdatePreviewAnimation();
            initializing = false;
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (initializing || preview.Width <= 0 || preview.Height <= 0) return;
            previewBackground?.Dispose();
            previewBackground = ClockAppearance.LoadBackground(theme);
            RenderPreview();
            UpdatePreviewAnimation();
            activeFont.Text = $"{theme.ActiveFont}, {theme.ActiveFontSize:0} px" + (theme.ActiveBold ? ", bold" : "") + (theme.ActiveItalic ? ", italic" : "");
            inactiveFont.Text = $"{theme.InactiveFont}, {theme.InactiveFontSize:0} px" + (theme.InactiveBold ? ", bold" : "") + (theme.InactiveItalic ? ", italic" : "");
        }

        private void UpdatePreviewAnimation() => previewTimer.Enabled = !IsDisposed && Visible && theme.Flicker;

        private void RenderPreview()
        {
            if (IsDisposed || preview.Width <= 0 || preview.Height <= 0) return;
            var now = DateTime.Now;
            var image = TextClock.Render(theme, preview.ClientSize, now, previewBackground, TextClock.GetIntensity(theme, now));
            var previous = preview.Image;
            preview.Image = image;
            previous?.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                previewTimer.Dispose();
                previewBackground?.Dispose();
                preview?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ChooseFont(bool active)
        {
            using var dialog = new FontDialog
            {
                Font = new Font(active ? theme.ActiveFont : theme.InactiveFont, active ? theme.ActiveFontSize : theme.InactiveFontSize,
                    ((active ? theme.ActiveBold : theme.InactiveBold) ? FontStyle.Bold : FontStyle.Regular) |
                    ((active ? theme.ActiveItalic : theme.InactiveItalic) ? FontStyle.Italic : FontStyle.Regular), GraphicsUnit.Pixel),
                MinSize = 6, MaxSize = 27, ShowEffects = false
            };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            float pixels = dialog.Font.SizeInPoints * 96 / 72;
            if (active) { theme.ActiveFont = dialog.Font.Name; theme.ActiveFontSize = pixels; theme.ActiveBold = dialog.Font.Bold; theme.ActiveItalic = dialog.Font.Italic; }
            else { theme.InactiveFont = dialog.Font.Name; theme.InactiveFontSize = pixels; theme.InactiveBold = dialog.Font.Bold; theme.InactiveItalic = dialog.Font.Italic; }
            RefreshPreview();
        }

        private void ChooseColor(string kind)
        {
            string current = kind == "Active" ? theme.ActiveColor : kind == "Inactive" ? theme.InactiveColor : theme.GlowColor;
            using var dialog = new ColorDialog { Color = ColorTranslator.FromHtml(current), FullOpen = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            string color = ColorTranslator.ToHtml(dialog.Color);
            if (kind == "Active") theme.ActiveColor = color;
            else if (kind == "Inactive") theme.InactiveColor = color;
            else theme.GlowColor = color;
            RefreshPreview();
        }

        private void BrowseBackground()
        {
            using var dialog = new OpenFileDialog { Title = "Choose clock background", Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp", CheckFileExists = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                using var image = Image.FromFile(dialog.FileName);
                theme.BackgroundImagePath = dialog.FileName;
                background.SelectedItem = "Custom";
                theme.BackgroundTemplate = "Custom";
                backgroundPath.Text = Path.GetFileName(dialog.FileName);
                RefreshPreview();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is IOException || ex is OutOfMemoryException || ex is UnauthorizedAccessException)
            { MessageBox.Show(this, "This image could not be opened. Choose a PNG, JPEG or bitmap image.", "Background image", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void SaveSettings()
        {
            AppConfig.AddOrUpdateAppSetting("autoStart", startWithWindows.Checked);
            StartWithWindows.Active = startWithWindows.Checked;
            AppConfig.AddOrUpdateAppSetting("startMinimized", startMinimized.Checked);
            AppConfig.AddOrUpdateAppSetting("alwaysOnTop", alwaysOnTop.Checked);
            AppConfig.AddOrUpdateAppSetting("automaticUpdateCheck", automaticUpdates.Checked);
            AppConfig.AddOrUpdateAppSetting("clockTheme", JsonConvert.SerializeObject(theme));
            DialogResult = DialogResult.OK;
            Close();
        }

        private async void CheckForUpdates(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.Enabled = false;
            try
            {
                var version = await UpdateChecker.GetNewerVersionAsync(typeof(SettingsForm).Assembly.GetName().Version);
                if (IsDisposed) return;
                if (version == null) MessageBox.Show(this, "You have the latest release.", "Check for updates");
                else if (MessageBox.Show(this, $"Version {version} is available. Open the download page?", "Update available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(UpdateChecker.ReleasesUrl) { UseShellExecute = true });
            }
            catch (Exception)
            { if (!IsDisposed) MessageBox.Show(this, "Updates could not be checked. Please try again later.", "Check for updates"); }
            finally { if (!IsDisposed) button.Enabled = true; }
        }
    }
}
