
using VolturaTextClock.Forms.Controls;

namespace VolturaTextClock
{
    partial class VolturaTextClockForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VolturaTextClockForm));
            this.browser = new System.Windows.Forms.WebBrowser();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.closeBtn = new System.Windows.Forms.Button();
            this.minimizeBtn = new System.Windows.Forms.Button();
            this.optionsPicBox = new System.Windows.Forms.PictureBox();
            this.useTip = new System.Windows.Forms.ToolTip(this.components);
            this.settingsPicBox = new System.Windows.Forms.PictureBox();
            this.pinBtn = new System.Windows.Forms.Button();
            this.panel1 = new VolturaTextClock.Forms.Controls.TransparentPanel();
            ((System.ComponentModel.ISupportInitialize)(this.optionsPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPicBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // browser
            // 
            this.browser.Visible = false;
            this.browser.AllowNavigation = false;
            this.browser.AllowWebBrowserDrop = false;
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.IsWebBrowserContextMenuEnabled = false;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Margin = new System.Windows.Forms.Padding(0);
            this.browser.Name = "browser";
            this.browser.ScriptErrorsSuppressed = true;
            this.browser.ScrollBarsEnabled = false;
            this.browser.Size = new System.Drawing.Size(480, 480);
            this.browser.TabIndex = 0;
            this.browser.WebBrowserShortcutsEnabled = false;
            this.browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);
            this.browser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.Browser_Navigating);
            // 
            // clockTimer
            // 
            this.clockTimer.Enabled = false;
            this.clockTimer.Interval = 1;
            this.clockTimer.Tick += new System.EventHandler(this.ClockTimer_Tick);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.AutoSize = true;
            this.closeBtn.BackColor = System.Drawing.Color.Gray;
            this.closeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(338, 430);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.closeBtn.MinimumSize = new System.Drawing.Size(0, 38);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(47, 38);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "&Close";
            this.closeBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useTip.SetToolTip(this.closeBtn, "Press to close the application");
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Visible = false;
            this.closeBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // minimizeBtn
            // 
            this.minimizeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeBtn.AutoSize = true;
            this.minimizeBtn.BackColor = System.Drawing.Color.Gray;
            this.minimizeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeBtn.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.minimizeBtn.ForeColor = System.Drawing.Color.White;
            this.minimizeBtn.Location = new System.Drawing.Point(288, 430);
            this.minimizeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.minimizeBtn.MinimumSize = new System.Drawing.Size(0, 38);
            this.minimizeBtn.Name = "minimizeBtn";
            this.minimizeBtn.Size = new System.Drawing.Size(46, 38);
            this.minimizeBtn.TabIndex = 2;
            this.minimizeBtn.Text = "&Hide";
            this.minimizeBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useTip.SetToolTip(this.minimizeBtn, "Press to hide clock (still running in taskbar)");
            this.minimizeBtn.UseVisualStyleBackColor = false;
            this.minimizeBtn.Visible = false;
            this.minimizeBtn.Click += new System.EventHandler(this.MinimizeBtn_Click);
            // 
            // optionsPicBox
            // 
            this.optionsPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPicBox.BackColor = System.Drawing.Color.Transparent;
            this.optionsPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.optionsPicBox.Location = new System.Drawing.Point(430, 430);
            this.optionsPicBox.Margin = new System.Windows.Forms.Padding(1);
            this.optionsPicBox.Name = "optionsPicBox";
            this.optionsPicBox.Size = new System.Drawing.Size(40, 40);
            this.optionsPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.optionsPicBox.TabIndex = 4;
            this.optionsPicBox.TabStop = false;
            this.useTip.SetToolTip(this.optionsPicBox, "Press to toogle buttons");
            this.optionsPicBox.Click += new System.EventHandler(this.OptionsPicBox_Click);
            // 
            // settingsPicBox
            // 
            this.settingsPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPicBox.BackColor = System.Drawing.Color.Transparent;
            this.settingsPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.settingsPicBox.Location = new System.Drawing.Point(388, 430);
            this.settingsPicBox.Margin = new System.Windows.Forms.Padding(1);
            this.settingsPicBox.Name = "settingsPicBox";
            this.settingsPicBox.Size = new System.Drawing.Size(40, 40);
            this.settingsPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.settingsPicBox.TabIndex = 6;
            this.settingsPicBox.TabStop = false;
            this.useTip.SetToolTip(this.settingsPicBox, "Press to show settings");
            this.settingsPicBox.Visible = false;
            this.settingsPicBox.Click += new System.EventHandler(this.SettingsPicBox_Click);
            // 
            // pinBtn
            // 
            this.pinBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pinBtn.AutoSize = true;
            this.pinBtn.BackColor = System.Drawing.Color.Gray;
            this.pinBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pinBtn.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.pinBtn.ForeColor = System.Drawing.Color.White;
            this.pinBtn.Location = new System.Drawing.Point(238, 430);
            this.pinBtn.Margin = new System.Windows.Forms.Padding(2);
            this.pinBtn.MinimumSize = new System.Drawing.Size(0, 38);
            this.pinBtn.Name = "pinBtn";
            this.pinBtn.Size = new System.Drawing.Size(46, 38);
            this.pinBtn.TabIndex = 5;
            this.pinBtn.Text = "Pin";
            this.pinBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useTip.SetToolTip(this.pinBtn, "Press to toogle clock always visible on top of other windows");
            this.pinBtn.UseVisualStyleBackColor = false;
            this.pinBtn.Visible = false;
            this.pinBtn.Click += new System.EventHandler(this.PinBtn_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 480);
            this.panel1.TabIndex = 8;
            // 
            // VolturaTextClockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(480, 480);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.browser);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.pinBtn);
            this.Controls.Add(this.optionsPicBox);
            this.Controls.Add(this.minimizeBtn);
            this.Controls.Add(this.settingsPicBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VolturaTextClockForm";
            this.Text = "VolturaTextClock";
            this.TransparencyKey = System.Drawing.Color.DarkGray;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VolturaTextClockForm_FormClosing);
            this.Load += new System.EventHandler(this.VolturaTextClockForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.optionsPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPicBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button minimizeBtn;
        private System.Windows.Forms.PictureBox optionsPicBox;
        private System.Windows.Forms.PictureBox settingsPicBox;
        private System.Windows.Forms.ToolTip useTip;
        private System.Windows.Forms.Button pinBtn;
        private TransparentPanel panel1;
    }
}

