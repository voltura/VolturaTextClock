using System.ComponentModel;
using System.Windows.Forms;
using VolturaTextClock.Forms.Controls;

namespace VolturaTextClock
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.Control.set_Text(System.String)")]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkStartWithWindows = new System.Windows.Forms.CheckBox();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.lblSettingsTitle = new System.Windows.Forms.Label();
            this.minimizePanelFrame = new System.Windows.Forms.Panel();
            this.minimizePanel = new System.Windows.Forms.Panel();
            this.titleIcon = new System.Windows.Forms.PictureBox();
            this.settingsPanel.SuspendLayout();
            this.minimizePanelFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.settingsPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("settingsPanel.BackgroundImage")));
            this.settingsPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.settingsPanel.Controls.Add(this.btnSave);
            this.settingsPanel.Controls.Add(this.chkStartWithWindows);
            this.settingsPanel.Controls.Add(this.chkAlwaysOnTop);
            this.settingsPanel.Controls.Add(this.chkStartMinimized);
            this.settingsPanel.Location = new System.Drawing.Point(0, 64);
            this.settingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(891, 474);
            this.settingsPanel.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSave.Location = new System.Drawing.Point(657, 368);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(215, 84);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Close";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // chkStartWithWindows
            // 
            this.chkStartWithWindows.AutoSize = true;
            this.chkStartWithWindows.BackColor = System.Drawing.Color.Black;
            this.chkStartWithWindows.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkStartWithWindows.BackgroundImage")));
            this.chkStartWithWindows.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkStartWithWindows.Checked = true;
            this.chkStartWithWindows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartWithWindows.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.chkStartWithWindows.FlatAppearance.BorderSize = 2;
            this.chkStartWithWindows.FlatAppearance.CheckedBackColor = System.Drawing.Color.DeepSkyBlue;
            this.chkStartWithWindows.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.chkStartWithWindows.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.chkStartWithWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkStartWithWindows.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkStartWithWindows.Location = new System.Drawing.Point(21, 20);
            this.chkStartWithWindows.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.chkStartWithWindows.Name = "chkStartWithWindows";
            this.chkStartWithWindows.Size = new System.Drawing.Size(172, 42);
            this.chkStartWithWindows.TabIndex = 0;
            this.chkStartWithWindows.Text = "Auto start";
            this.chkStartWithWindows.UseVisualStyleBackColor = false;
            this.chkStartWithWindows.Visible = false;
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.BackColor = System.Drawing.Color.Black;
            this.chkAlwaysOnTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkAlwaysOnTop.BackgroundImage")));
            this.chkAlwaysOnTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkAlwaysOnTop.Checked = true;
            this.chkAlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAlwaysOnTop.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.chkAlwaysOnTop.FlatAppearance.BorderSize = 2;
            this.chkAlwaysOnTop.FlatAppearance.CheckedBackColor = System.Drawing.Color.DeepSkyBlue;
            this.chkAlwaysOnTop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.chkAlwaysOnTop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.chkAlwaysOnTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAlwaysOnTop.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(21, 152);
            this.chkAlwaysOnTop.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(225, 42);
            this.chkAlwaysOnTop.TabIndex = 3;
            this.chkAlwaysOnTop.Text = "Always on top";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = false;
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.BackColor = System.Drawing.Color.Black;
            this.chkStartMinimized.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkStartMinimized.BackgroundImage")));
            this.chkStartMinimized.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkStartMinimized.Checked = true;
            this.chkStartMinimized.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartMinimized.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.chkStartMinimized.FlatAppearance.BorderSize = 2;
            this.chkStartMinimized.FlatAppearance.CheckedBackColor = System.Drawing.Color.DeepSkyBlue;
            this.chkStartMinimized.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.chkStartMinimized.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.chkStartMinimized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkStartMinimized.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkStartMinimized.Location = new System.Drawing.Point(21, 86);
            this.chkStartMinimized.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(239, 42);
            this.chkStartMinimized.TabIndex = 2;
            this.chkStartMinimized.Text = "Start in Taskbar";
            this.chkStartMinimized.UseVisualStyleBackColor = false;
            this.chkStartMinimized.Visible = false;
            // 
            // lblSettingsTitle
            // 
            this.lblSettingsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSettingsTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSettingsTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSettingsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSettingsTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblSettingsTitle.Name = "lblSettingsTitle";
            this.lblSettingsTitle.Padding = new System.Windows.Forms.Padding(52, 0, 0, 0);
            this.lblSettingsTitle.Size = new System.Drawing.Size(891, 64);
            this.lblSettingsTitle.TabIndex = 1;
            this.lblSettingsTitle.Text = "Settings";
            this.lblSettingsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSettingsTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsTitle_MouseDown);
            this.lblSettingsTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SettingsTitle_MouseMove);
            // 
            // minimizePanelFrame
            // 
            this.minimizePanelFrame.Controls.Add(this.minimizePanel);
            this.minimizePanelFrame.Location = new System.Drawing.Point(829, 0);
            this.minimizePanelFrame.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.minimizePanelFrame.Name = "minimizePanelFrame";
            this.minimizePanelFrame.Size = new System.Drawing.Size(59, 64);
            this.minimizePanelFrame.TabIndex = 3;
            this.minimizePanelFrame.Click += new System.EventHandler(this.MinimizePanelFrame_Click);
            this.minimizePanelFrame.MouseEnter += new System.EventHandler(this.MinimizePanel_MouseEnter);
            this.minimizePanelFrame.MouseLeave += new System.EventHandler(this.MinimizePanel_MouseLeave);
            // 
            // minimizePanel
            // 
            this.minimizePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizePanel.BackColor = System.Drawing.Color.White;
            this.minimizePanel.Location = new System.Drawing.Point(12, 14);
            this.minimizePanel.Margin = new System.Windows.Forms.Padding(0);
            this.minimizePanel.Name = "minimizePanel";
            this.minimizePanel.Size = new System.Drawing.Size(40, 16);
            this.minimizePanel.TabIndex = 3;
            this.minimizePanel.Click += new System.EventHandler(this.MinimizePanel_Click);
            this.minimizePanel.MouseEnter += new System.EventHandler(this.MinimizePanel_MouseEnter);
            // 
            // titleIcon
            // 
            this.titleIcon.BackColor = System.Drawing.Color.Transparent;
            this.titleIcon.Image = ((System.Drawing.Image)(resources.GetObject("titleIcon.Image")));
            this.titleIcon.Location = new System.Drawing.Point(7, 0);
            this.titleIcon.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.titleIcon.Name = "titleIcon";
            this.titleIcon.Size = new System.Drawing.Size(52, 64);
            this.titleIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.titleIcon.TabIndex = 4;
            this.titleIcon.TabStop = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(891, 538);
            this.ControlBox = false;
            this.Controls.Add(this.titleIcon);
            this.Controls.Add(this.minimizePanelFrame);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.lblSettingsTitle);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.minimizePanelFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CheckBox chkStartWithWindows;
        private CheckBox chkStartMinimized;
        private CheckBox chkAlwaysOnTop;
        private Button btnSave;
        private Label lblSettingsTitle;
        private Panel settingsPanel;
        private Panel minimizePanelFrame;
        private Panel minimizePanel;
        private PictureBox titleIcon;
    }
}