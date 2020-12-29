
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
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.closeBtn = new System.Windows.Forms.Button();
            this.minimizeBtn = new System.Windows.Forms.Button();
            this.optionsPicBox = new System.Windows.Forms.PictureBox();
            this.useTip = new System.Windows.Forms.ToolTip(this.components);
            this.settingsPicBox = new System.Windows.Forms.PictureBox();
            this.pinPicBox = new System.Windows.Forms.PictureBox();
            this.clockPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.optionsPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // clockTimer
            // 
            this.clockTimer.Interval = 1;
            this.clockTimer.Tick += new System.EventHandler(this.ClockTimer_Tick);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.BackColor = System.Drawing.Color.Gray;
            this.closeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.closeBtn.ForeColor = System.Drawing.Color.White;
            this.closeBtn.Location = new System.Drawing.Point(373, 440);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(64, 40);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "&Close";
            this.useTip.SetToolTip(this.closeBtn, "Press to close the application");
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Visible = false;
            this.closeBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // minimizeBtn
            // 
            this.minimizeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizeBtn.BackColor = System.Drawing.Color.Gray;
            this.minimizeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.minimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeBtn.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.minimizeBtn.ForeColor = System.Drawing.Color.White;
            this.minimizeBtn.Location = new System.Drawing.Point(304, 440);
            this.minimizeBtn.Name = "minimizeBtn";
            this.minimizeBtn.Size = new System.Drawing.Size(64, 40);
            this.minimizeBtn.TabIndex = 2;
            this.minimizeBtn.Text = "&Hide";
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
            this.optionsPicBox.Image = ((System.Drawing.Image)(resources.GetObject("optionsPicBox.Image")));
            this.optionsPicBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("optionsPicBox.InitialImage")));
            this.optionsPicBox.Location = new System.Drawing.Point(440, 440);
            this.optionsPicBox.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
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
            this.settingsPicBox.Image = ((System.Drawing.Image)(resources.GetObject("settingsPicBox.Image")));
            this.settingsPicBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("settingsPicBox.InitialImage")));
            this.settingsPicBox.Location = new System.Drawing.Point(180, 440);
            this.settingsPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.settingsPicBox.Name = "settingsPicBox";
            this.settingsPicBox.Size = new System.Drawing.Size(40, 40);
            this.settingsPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.settingsPicBox.TabIndex = 6;
            this.settingsPicBox.TabStop = false;
            this.useTip.SetToolTip(this.settingsPicBox, "Press to show settings");
            this.settingsPicBox.Visible = false;
            this.settingsPicBox.Click += new System.EventHandler(this.SettingsPicBox_Click);
            // 
            // pinPicBox
            // 
            this.pinPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pinPicBox.BackColor = System.Drawing.Color.Transparent;
            this.pinPicBox.Image = ((System.Drawing.Image)(resources.GetObject("pinPicBox.Image")));
            this.pinPicBox.InitialImage = ((System.Drawing.Image)(resources.GetObject("pinPicBox.InitialImage")));
            this.pinPicBox.Location = new System.Drawing.Point(220, 440);
            this.pinPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.pinPicBox.Name = "pinPicBox";
            this.pinPicBox.Size = new System.Drawing.Size(40, 40);
            this.pinPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pinPicBox.TabIndex = 10;
            this.pinPicBox.TabStop = false;
            this.useTip.SetToolTip(this.pinPicBox, "Press to toggle if the clock always should be displayed on top");
            this.pinPicBox.Visible = false;
            this.pinPicBox.Click += new System.EventHandler(this.PinPicBox_Click);
            // 
            // clockPictureBox
            // 
            this.clockPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.clockPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clockPictureBox.Location = new System.Drawing.Point(0, 0);
            this.clockPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.clockPictureBox.Name = "clockPictureBox";
            this.clockPictureBox.Size = new System.Drawing.Size(480, 480);
            this.clockPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.clockPictureBox.TabIndex = 9;
            this.clockPictureBox.TabStop = false;
            // 
            // VolturaTextClockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(480, 480);
            this.ControlBox = false;
            this.Controls.Add(this.pinPicBox);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.optionsPicBox);
            this.Controls.Add(this.minimizeBtn);
            this.Controls.Add(this.settingsPicBox);
            this.Controls.Add(this.clockPictureBox);
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
            ((System.ComponentModel.ISupportInitialize)(this.pinPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button minimizeBtn;
        private System.Windows.Forms.PictureBox optionsPicBox;
        private System.Windows.Forms.PictureBox settingsPicBox;
        private System.Windows.Forms.ToolTip useTip;
        private System.Windows.Forms.PictureBox clockPictureBox;
        private System.Windows.Forms.PictureBox pinPicBox;
    }
}

