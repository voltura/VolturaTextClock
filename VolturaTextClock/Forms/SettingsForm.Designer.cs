using System;
using System.Drawing;
using System.Windows.Forms;
using VolturaTextClock.Library;
using static VolturaTextClock.Program;

namespace VolturaTextClock
{
    public partial class SettingsForm
    {
        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "Clock settings";
            AutoScaleDimensions = new SizeF(96, 96);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10);
            ClientSize = new Size(760, 580);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 247, 250);
            var options = new TableLayoutPanel { Dock = DockStyle.Left, Width = 430, Padding = new Padding(20), ColumnCount = 2, AutoScroll = true };
            options.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            options.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(options);
            void Full(Control control)
            {
                int row = options.RowCount++;
                options.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                control.Margin = new Padding(0, 0, 0, 10);
                options.Controls.Add(control, 0, row); options.SetColumnSpan(control, 2);
            }
            void Row(string title, Control control)
            {
                int row = options.RowCount++;
                options.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                options.Controls.Add(new Label { Text = title, AutoSize = true, Margin = new Padding(0, 7, 8, 10) }, 0, row);
                control.Anchor = AnchorStyles.Left | AnchorStyles.Right; control.Margin = new Padding(0, 0, 0, 10); options.Controls.Add(control, 1, row);
            }
            CheckBox Check(string title, bool value) => new CheckBox { Text = title, Checked = value, AutoSize = true };
            startWithWindows = Check("Start with Windows", AppConfig.GetValue("autoStart", false));
            startMinimized = Check("Start minimized in the taskbar", AppConfig.GetValue("startMinimized", false));
            alwaysOnTop = Check("Keep the clock on top", AppConfig.GetValue("alwaysOnTop", false));
            automaticUpdates = Check("Check for updates when the clock starts", AppConfig.GetValue("automaticUpdateCheck", false));
            Full(startWithWindows); Full(startMinimized); Full(alwaysOnTop); Full(automaticUpdates);
            var checkUpdates = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, Text = "Check for updates now", AutoSize = true };
            checkUpdates.Click += CheckForUpdates; Full(checkUpdates);
            language = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            language.Items.AddRange(new object[] { "System", "Swedish", "English" });
            language.SelectedItem = theme.Language.ToString();
            language.SelectedIndexChanged += (_, _) => { theme.Language = Enum.Parse<TextClockTheme.LANGUAGE>((string)language.SelectedItem); RefreshPreview(); };
            Row("Clock language", language);
            activeFont = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, AutoSize = true }; activeFont.Click += (_, _) => ChooseFont(true);
            inactiveFont = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, AutoSize = true }; inactiveFont.Click += (_, _) => ChooseFont(false);
            Row("Active text font", activeFont); Row("Inactive text font", inactiveFont);
            var colors = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, WrapContents = false };
            foreach (string kind in new[] { "Active", "Inactive", "Glow" })
            {
                var button = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, Text = kind, AutoSize = true, MinimumSize = new Size(66, 28) };
                button.Click += (_, _) => ChooseColor(kind); colors.Controls.Add(button);
            }
            Row("Text colors", colors);
            var effects = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
            glow = Check("Glow", theme.Glow); flicker = Check("Flicker", theme.Flicker);
            glow.CheckedChanged += (_, _) => { theme.Glow = glow.Checked; RefreshPreview(); };
            flicker.CheckedChanged += (_, _) => { theme.Flicker = flicker.Checked; RefreshPreview(); };
            effects.Controls.Add(glow); effects.Controls.Add(flicker); Row("Active text effects", effects);
            background = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            background.Items.AddRange(new object[] { "Original", "Brushed", "Rust", "None", "Custom" });
            background.SelectedItem = theme.BackgroundTemplate;
            background.SelectedIndexChanged += (_, _) => { theme.BackgroundTemplate = (string)background.SelectedItem; RefreshPreview(); };
            Row("Background", background);
            var browse = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, Text = "Choose image…", AutoSize = true }; browse.Click += (_, _) => BrowseBackground();
            Row("Custom image", browse);
            backgroundPath = new Label { Text = PathName(theme.BackgroundImagePath), AutoSize = true, MaximumSize = new Size(380, 0) }; Full(backgroundPath);
            var right = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 25, 20, 20) };
            Controls.Add(right); right.BringToFront();
            var heading = new Label { Text = "Preview", Dock = DockStyle.Top, Height = 32 };
            preview = new PictureBox { Location = new Point(10, 65), Size = new Size(280, 280), SizeMode = PictureBoxSizeMode.Zoom };
            right.Controls.Add(heading); right.Controls.Add(preview);
            var note = new Label { Text = "The system language uses Swedish for a Swedish Windows display language, and English otherwise.", Location = new Point(10, 360), Size = new Size(280, 75) };
            right.Controls.Add(note);
            var save = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, Text = "Save", AutoSize = true, Location = new Point(10, 465) };
            save.Click += (_, _) => SaveSettings(); right.Controls.Add(save); AcceptButton = save;
            var cancel = new Button { AutoSizeMode = AutoSizeMode.GrowAndShrink, Text = "Cancel", AutoSize = true, Location = new Point(110, 465), DialogResult = DialogResult.Cancel };
            right.Controls.Add(cancel); CancelButton = cancel;
            FormClosed += (_, _) => preview.Image?.Dispose();
            AutoScaleDimensions = new SizeF(96, 96);
            ResumeLayout(false);
            PerformLayout();
        }
        private static string PathName(string path) => string.IsNullOrWhiteSpace(path) ? "No custom image selected" : System.IO.Path.GetFileName(path);
    }
}
