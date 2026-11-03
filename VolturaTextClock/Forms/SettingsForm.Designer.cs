using System;
using System.Drawing;
using System.Windows.Forms;
using VolturaTextClock.Library;
using static VolturaTextClock.Program;

namespace VolturaTextClock;

public partial class SettingsForm
{
    private ComboBox interfaceLanguage;
    private Label updateStatus;
    private Button checkUpdate, downloadUpdate, installUpdate, cancelUpdate;
    private TabControl tabs;

    private void InitializeComponent()
    {
        SuspendLayout();
        Text = UiText.Get("Settings");
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        AutoScaleDimensions = new SizeF(96, 96);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10);
        ClientSize = new Size(840, 580);
        MinimumSize = new Size(640, 440);
        MaximizeBox = false; MinimizeBox = false; ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(248, 248, 245);
        var shell = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(16) };
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        shell.RowStyles.Add(new RowStyle(SizeType.AutoSize)); Controls.Add(shell);
        tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(16, 8) }; shell.Controls.Add(tabs, 0, 0);
        TableLayoutPanel Page(string key)
        {
            var page = new TabPage(UiText.Get(key)) { BackColor = BackColor, Padding = new Padding(14), AutoScroll = true };
            tabs.TabPages.Add(page);
            var panel = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(4) };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); page.Controls.Add(panel); return panel;
        }
        void Full(TableLayoutPanel panel, Control control)
        {
            int row = panel.RowCount++; panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            control.Margin = new Padding(0, 6, 0, 10); panel.Controls.Add(control, 0, row); panel.SetColumnSpan(control, 2);
        }
        void Row(TableLayoutPanel panel, string title, Control control)
        {
            int row = panel.RowCount++; panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.Controls.Add(new Label { Text = UiText.Get(title), AutoSize = true, MaximumSize = new Size(145, 0), Margin = new Padding(0, 10, 8, 10) }, 0, row);
            control.Margin = new Padding(0, 4, 0, 10); control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            control.AccessibleName = UiText.Get(title); panel.Controls.Add(control, 1, row);
        }
        CheckBox Check(string key, bool value) => new() { Text = UiText.Get(key), Checked = value, AutoSize = true };
        Button Action(string key, EventHandler handler)
        {
            var button = new Button { Text = UiText.Get(key), AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(8, 2, 8, 2), UseVisualStyleBackColor = true };
            button.Click += handler; return button;
        }
        ComboBox Select(string[] values, string selected)
        {
            var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true, Width = 210 };
            box.Items.AddRange(values); box.Format += (_, e) => e.Value = UiText.Get((string)e.ListItem);
            box.SelectedItem = selected; if (box.SelectedIndex < 0) box.SelectedIndex = 0; return box;
        }
        var general = Page("General");
        interfaceLanguage = Select(new[] { "System", "English", "Swedish" }, AppConfig.GetValue("interfaceLanguage", "System"));
        language = Select(new[] { "System", "English", "Swedish" }, theme.Language.ToString());
        language.SelectedIndexChanged += (_, _) => { theme.Language = Enum.Parse<TextClockTheme.LANGUAGE>((string)language.SelectedItem); RefreshPreview(); };
        Row(general, "Interface", interfaceLanguage); Row(general, "ClockLanguage", language);
        Full(general, new Label { Text = UiText.Get("LanguageHint"), AutoSize = true, MaximumSize = new Size(620, 0) });
        Full(general, new Label { Text = UiText.Get("RestartHint"), AutoSize = true, ForeColor = Color.DimGray });
        startWithWindows = Check("Startup", AppConfig.GetValue("autoStart", false));
        startMinimized = Check("Minimized", AppConfig.GetValue("startMinimized", false));
        alwaysOnTop = Check("OnTop", AppConfig.GetValue("alwaysOnTop", true));
        Full(general, startWithWindows); Full(general, startMinimized); Full(general, alwaysOnTop);

        var appearance = Page("Appearance");
        var appearancePage = tabs.TabPages[1];
        var split = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60)); split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        appearancePage.Controls.Remove(appearance);
        var scroller = new Panel { Dock = DockStyle.Fill, AutoScroll = true }; scroller.Controls.Add(appearance);
        split.Controls.Add(scroller, 0, 0); appearancePage.Controls.Add(split);
        activeFont = Action("ActiveFont", (_, _) => ChooseFont(true)); inactiveFont = Action("InactiveFont", (_, _) => ChooseFont(false));
        Row(appearance, "ActiveFont", activeFont); Row(appearance, "InactiveFont", inactiveFont);
        var colors = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        foreach (var kind in new[] { "Active", "Inactive", "Glow" }) colors.Controls.Add(Action(kind, (_, _) => ChooseColor(kind)));
        Row(appearance, "TextColors", colors);
        var effects = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        glow = Check("Glow", theme.Glow); flicker = Check("Flicker", theme.Flicker);
        glow.CheckedChanged += (_, _) => { theme.Glow = glow.Checked; RefreshPreview(); };
        flicker.CheckedChanged += (_, _) => { theme.Flicker = flicker.Checked; RefreshPreview(); };
        effects.Controls.Add(glow); effects.Controls.Add(flicker); Row(appearance, "Effects", effects);
        background = Select(new[] { "Original", "Brushed", "Rust", "None", "Custom" }, theme.BackgroundTemplate);
        background.SelectedIndexChanged += (_, _) => { theme.BackgroundTemplate = (string)background.SelectedItem; RefreshPreview(); };
        Row(appearance, "Background", background); Full(appearance, Action("ChooseImage", (_, _) => BrowseBackground()));
        backgroundPath = new Label { Text = PathName(theme.BackgroundImagePath), AutoSize = true, MaximumSize = new Size(360, 0), ForeColor = Color.DimGray };
        Full(appearance, backgroundPath);
        var previewPanel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 2 };
        previewPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); previewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        previewPanel.Controls.Add(new Label { Text = UiText.Get("Preview"), AutoSize = true, Padding = new Padding(0, 8, 0, 12) }, 0, 0);
        preview = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, MinimumSize = new Size(100, 100) };
        previewPanel.Controls.Add(preview, 0, 1); split.Controls.Add(previewPanel, 1, 0);

        var updates = Page("UpdatesAbout");
        Full(updates, new Label { Text = "Voltura TextClock", Font = new Font(Font.FontFamily, 19, FontStyle.Bold), AutoSize = true });
        Full(updates, new Label { Text = UiText.Get("About") + "  ·  " + Application.ProductVersion.Split('+')[0] + "  ·  © 2020–2026 Voltura AB", AutoSize = true });
        automaticUpdates = Check("Automatic", AppConfig.GetValue("automaticUpdateCheck", false)); Full(updates, automaticUpdates);
        updateStatus = new Label { Text = UiText.Get(updater.State.Status.ToString()), AutoSize = true, MaximumSize = new Size(620, 0) }; Full(updates, updateStatus);
        var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        checkUpdate = Action("Check", CheckForUpdates);
        downloadUpdate = Action("Download", async (_, _) => await updater.CheckAsync(download: true));
        installUpdate = Action("Install", async (_, _) => { if (await updater.InstallAsync()) Application.Exit(); });
        cancelUpdate = Action("Cancel", (_, _) => updater.Cancel());
        actions.Controls.AddRange(new Control[] { checkUpdate, downloadUpdate, installUpdate, cancelUpdate }); Full(updates, actions);
        var links = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        foreach (var link in new[] { ("Website", AppLinks.Website), ("Source", AppLinks.Source), ("Report", AppLinks.Source + "/issues"), ("License", AppLinks.Source + "/blob/master/LICENSE") })
        {
            var item = new LinkLabel { Text = UiText.Get(link.Item1), AutoSize = true, Margin = new Padding(0, 8, 16, 8) };
            item.LinkClicked += (_, _) => AppLinks.Open(link.Item2); links.Controls.Add(item);
        }
        Full(updates, links); Full(updates, new Label { Text = UiText.Get("Privacy"), AutoSize = true, MaximumSize = new Size(620, 0) });
        var footer = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 12, 0, 0) };
        var cancel = Action("Cancel", (_, _) => { DialogResult = DialogResult.Cancel; Close(); });
        var save = Action("Save", (_, _) => SaveSettings()); footer.Controls.Add(cancel); footer.Controls.Add(save); shell.Controls.Add(footer, 0, 1);
        AcceptButton = save; CancelButton = cancel;
        Shown += (_, _) => { var area = Screen.FromControl(this).WorkingArea; if (Height > area.Height - 32) Height = area.Height - 32; if (Width > area.Width - 32) Width = area.Width - 32; };
        ResumeLayout(true);
    }
    private static string PathName(string path) => string.IsNullOrWhiteSpace(path) ? UiText.Get("NoImage") : System.IO.Path.GetFileName(path);
}
