using Newtonsoft.Json;
using VolturaTextClock.Updates;
using System.Collections.Generic;
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
        private readonly UpdateService updater = new(System.Reflection.Assembly.GetEntryAssembly() == typeof(SettingsForm).Assembly
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Voltura", "TextClock")
            : Path.Combine(Path.GetDirectoryName(ConfigurationFile), "update-test-cache"));
        private TextClockTheme theme;
        private PictureBox preview;
        private CheckBox startWithWindows, startMinimized, alwaysOnTop, automaticUpdates, glow, flicker;
        private ComboBox language, background;
        private Label backgroundPath;
        private Button activeFont, inactiveFont;
        private bool initializing = true;
        private readonly System.Windows.Forms.Timer previewTimer = new() { Interval = 125 };
        private Image previewBackground;

        public SettingsForm()
        {
            theme = ClockAppearance.LoadTheme();
            InitializeComponent();
            previewTimer.Tick += (_, _) => RenderPreview();
            VisibleChanged += (_, _) => UpdatePreviewAnimation();
            updater.Changed += UpdateUi;
            Shown += async (_, _) => { try { await updater.RestorePendingAsync(); } catch (OperationCanceledException) { } };
            FormClosed += async (_, _) => { updater.Changed -= UpdateUi; await updater.DisposeAsync(); };
            initializing = false;
            UpdateUi();
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (initializing || preview.Width <= 0 || preview.Height <= 0) return;
            previewBackground?.Dispose();
            previewBackground = ClockAppearance.LoadBackground(theme);
            RenderPreview();
            UpdatePreviewAnimation();
            activeFont.Text = $"{theme.ActiveFont}, {theme.ActiveFontSize:0} px" + (theme.ActiveBold ? ", " + UiText.Get("Bold") : "") + (theme.ActiveItalic ? ", " + UiText.Get("Italic") : "");
            inactiveFont.Text = $"{theme.InactiveFont}, {theme.InactiveFontSize:0} px" + (theme.InactiveBold ? ", " + UiText.Get("Bold") : "") + (theme.InactiveItalic ? ", " + UiText.Get("Italic") : "");
        }

        private void UpdatePreviewAnimation() => previewTimer.Enabled = !IsDisposed && Visible && theme.Flicker;

        private void RenderPreview()
        {
            if (IsDisposed || preview.Width <= 0 || preview.Height <= 0) return;
            var now = DateTime.Now;
            int side = Math.Max(1, Math.Min(preview.ClientSize.Width, preview.ClientSize.Height));
            var image = TextClock.Render(theme, new Size(side, side), now, previewBackground, TextClock.GetIntensity(theme, now));
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
            using var dialog = new OpenFileDialog { Title = UiText.Get("ChooseImage"), Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp", CheckFileExists = true };
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
            { MessageBox.Show(this, UiText.Get("BadImage"), UiText.Get("Background"), MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void SaveSettings()
        {
            try
            {
                StartWithWindows.Active = startWithWindows.Checked;
                SettingsStore.Save(ConfigurationFile, new Dictionary<string, object>
                {
                    ["autoStart"] = startWithWindows.Checked, ["startMinimized"] = startMinimized.Checked,
                    ["alwaysOnTop"] = alwaysOnTop.Checked, ["automaticUpdateCheck"] = automaticUpdates.Checked,
                    ["interfaceLanguage"] = (string)interfaceLanguage.SelectedItem,
                    ["clockTheme"] = JsonConvert.SerializeObject(theme)
                });
                UiText.Language = (string)interfaceLanguage.SelectedItem;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException or System.Security.SecurityException)
            {
                Log.Error = error;
                MessageBox.Show(this, UiText.Get("SaveFailed"), UiText.Get("Settings"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void CheckForUpdates(object sender, EventArgs e)
        {
            if (!updater.Eligible) { AppLinks.Open(UpdateChecker.ReleasesUrl); return; }
            await updater.CheckAsync();
        }

        private void UpdateUi()
        {
            if (IsDisposed || Disposing) return;
            if (InvokeRequired) { if (IsHandleCreated) BeginInvoke(new Action(UpdateUi)); return; }
            var state = updater.State;
            updateStatus.Text = UiText.Get(state.Status.ToString());
            checkUpdate.Enabled = !state.Busy;
            downloadUpdate.Visible = state.Status == UpdateStatus.Available;
            installUpdate.Visible = state.Ready;
            cancelUpdate.Visible = state.Busy && state.Status != UpdateStatus.Installing;
        }
    }
}
