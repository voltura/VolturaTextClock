
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
            this.optionsPicBox = new System.Windows.Forms.PictureBox();
            this.useTip = new System.Windows.Forms.ToolTip(this.components);
            this.settingsPicBox = new System.Windows.Forms.PictureBox();
            this.pinPicBox = new System.Windows.Forms.PictureBox();
            this.minimizePicBox = new System.Windows.Forms.PictureBox();
            this.clockPicBox = new System.Windows.Forms.PictureBox();
            this.closePicBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.optionsPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizePicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closePicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // clockTimer
            // 
            this.clockTimer.Interval = 1;
            this.clockTimer.Tick += new System.EventHandler(this.ClockTimer_Tick);
            // 
            // optionsPicBox
            // 
            this.optionsPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPicBox.BackColor = System.Drawing.Color.Transparent;
            this.optionsPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.optionsPicBox.Image = ((System.Drawing.Image)(resources.GetObject("optionsPicBox.Image")));
            this.optionsPicBox.InitialImage = null;
            this.optionsPicBox.Location = new System.Drawing.Point(440, 440);
            this.optionsPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.optionsPicBox.Name = "optionsPicBox";
            this.optionsPicBox.Size = new System.Drawing.Size(40, 40);
            this.optionsPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.optionsPicBox.TabIndex = 4;
            this.optionsPicBox.TabStop = false;
            this.useTip.SetToolTip(this.optionsPicBox, "Press to toogle buttons");
            this.optionsPicBox.Click += new System.EventHandler(this.OptionsPicBox_Click);
            this.optionsPicBox.MouseEnter += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            this.optionsPicBox.MouseLeave += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            // 
            // settingsPicBox
            // 
            this.settingsPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPicBox.BackColor = System.Drawing.Color.Transparent;
            this.settingsPicBox.Image = ((System.Drawing.Image)(resources.GetObject("settingsPicBox.Image")));
            this.settingsPicBox.InitialImage = null;
            this.settingsPicBox.Location = new System.Drawing.Point(243, 440);
            this.settingsPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.settingsPicBox.Name = "settingsPicBox";
            this.settingsPicBox.Size = new System.Drawing.Size(40, 40);
            this.settingsPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.settingsPicBox.TabIndex = 6;
            this.settingsPicBox.TabStop = false;
            this.useTip.SetToolTip(this.settingsPicBox, "Press to show settings");
            this.settingsPicBox.Visible = false;
            this.settingsPicBox.Click += new System.EventHandler(this.SettingsPicBox_Click);
            this.settingsPicBox.MouseEnter += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            this.settingsPicBox.MouseLeave += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            // 
            // pinPicBox
            // 
            this.pinPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pinPicBox.BackColor = System.Drawing.Color.Transparent;
            this.pinPicBox.Image = ((System.Drawing.Image)(resources.GetObject("pinPicBox.Image")));
            this.pinPicBox.InitialImage = null;
            this.pinPicBox.Location = new System.Drawing.Point(292, 440);
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
            // minimizePicBox
            // 
            this.minimizePicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizePicBox.BackColor = System.Drawing.Color.Transparent;
            this.minimizePicBox.Image = ((System.Drawing.Image)(resources.GetObject("minimizePicBox.Image")));
            this.minimizePicBox.InitialImage = null;
            this.minimizePicBox.Location = new System.Drawing.Point(342, 440);
            this.minimizePicBox.Margin = new System.Windows.Forms.Padding(0);
            this.minimizePicBox.Name = "minimizePicBox";
            this.minimizePicBox.Size = new System.Drawing.Size(40, 40);
            this.minimizePicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizePicBox.TabIndex = 11;
            this.minimizePicBox.TabStop = false;
            this.useTip.SetToolTip(this.minimizePicBox, "Press to minimize clock (still running in taskbar)");
            this.minimizePicBox.Visible = false;
            this.minimizePicBox.Click += new System.EventHandler(this.MinimizePicBox_Click);
            this.minimizePicBox.MouseEnter += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            this.minimizePicBox.MouseLeave += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            // 
            // clockPicBox
            // 
            this.clockPicBox.BackColor = System.Drawing.Color.Transparent;
           // this.clockPicBox.Image = ((System.Drawing.Image)(resources.GetObject("clockPicBox.Image")));
            this.clockPicBox.Location = new System.Drawing.Point(0, 0);
            this.clockPicBox.Margin = new System.Windows.Forms.Padding(0);
            this.clockPicBox.MinimumSize = new System.Drawing.Size(480, 480);
            this.clockPicBox.Name = "clockPicBox";
            this.clockPicBox.Size = new System.Drawing.Size(480, 480);
            this.clockPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.clockPicBox.TabIndex = 9;
            this.clockPicBox.TabStop = false;
            this.useTip.SetToolTip(this.clockPicBox, "Press and hold + move mouse to move clock");
            // 
            // closePicBox
            // 
            this.closePicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closePicBox.BackColor = System.Drawing.Color.Transparent;
            this.closePicBox.Image = ((System.Drawing.Image)(resources.GetObject("closePicBox.Image")));
            this.closePicBox.InitialImage = null;
            this.closePicBox.Location = new System.Drawing.Point(391, 440);
            this.closePicBox.Margin = new System.Windows.Forms.Padding(0);
            this.closePicBox.Name = "closePicBox";
            this.closePicBox.Size = new System.Drawing.Size(40, 40);
            this.closePicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.closePicBox.TabIndex = 12;
            this.closePicBox.TabStop = false;
            this.useTip.SetToolTip(this.closePicBox, "Press to close clock");
            this.closePicBox.Visible = false;
            this.closePicBox.Click += new System.EventHandler(this.ClosePicBox_Click);
            this.closePicBox.MouseEnter += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            this.closePicBox.MouseLeave += new System.EventHandler(this.PictureBox_MouseLeaveOrEnter);
            // 
            // VolturaTextClockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(480, 480);
            this.ControlBox = false;
            this.Controls.Add(this.closePicBox);
            this.Controls.Add(this.minimizePicBox);
            this.Controls.Add(this.pinPicBox);
            this.Controls.Add(this.optionsPicBox);
            this.Controls.Add(this.settingsPicBox);
            this.Controls.Add(this.clockPicBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VolturaTextClockForm";
            this.Text = "VolturaTextClock";
            this.TransparencyKey = System.Drawing.Color.Yellow;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VolturaTextClockForm_FormClosing);
            this.Load += new System.EventHandler(this.VolturaTextClockForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.optionsPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingsPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizePicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clockPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closePicBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.PictureBox optionsPicBox;
        private System.Windows.Forms.PictureBox settingsPicBox;
        private System.Windows.Forms.ToolTip useTip;
        private System.Windows.Forms.PictureBox clockPicBox;
        private System.Windows.Forms.PictureBox pinPicBox;
        private System.Windows.Forms.PictureBox minimizePicBox;
        private System.Windows.Forms.PictureBox closePicBox;
    }
}

