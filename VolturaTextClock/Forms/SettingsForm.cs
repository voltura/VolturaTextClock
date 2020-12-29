#region Using statements

using VolturaTextClock.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
using static VolturaTextClock.Program;

#endregion

namespace VolturaTextClock
{
    /// <summary>
    ///     Settings form
    /// </summary>
    public partial class SettingsForm : Form
    {
        #region Protected class properties

        /// <summary>
        ///     Mouse location offset used form form movement
        /// </summary>
        protected Point Offset { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        ///     Settings form constructor
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
            InitializeFormFromSettings();
        }

        #endregion

        #region Private methods

        private void InitializeFormFromSettings()
        {
            SetControlTextsFromResources();
            SetValuesFromSettings();
            AddFieldDataBindings();
        }

        private void SetValuesFromSettings()
        {
            TickCheckboxesBasedOnUserSettings();
        }

        private void TickCheckboxesBasedOnUserSettings()
        {
            chkStartWithWindows.Checked = AppConfig.GetValue<bool>("autoStart", false);
            chkAlwaysOnTop.Checked = AppConfig.GetValue<bool>("alwaysOnTop", false);
            chkStartMinimized.Checked = AppConfig.GetValue<bool>("startMinimized", false);
        }

        private void SetControlTextsFromResources()
        {
            SetCheckboxTextsFromResources();
            SetLabelTextsFromResources();
            SetButtonTextsFromResources();
            Text = Resources.Settings;
        }

        private void SetButtonTextsFromResources()
        {
            btnSave.Text = Resources.SaveButtonTitle;
        }

        private void SetLabelTextsFromResources()
        {
            lblSettingsTitle.Text = Resources.Settings;
        }

        private void SetCheckboxTextsFromResources()
        {
            chkStartWithWindows.Text = Resources.StartWithWindowsText;
            chkAlwaysOnTop.Text = Resources.AlwaysOnTop;
            chkStartMinimized.Text = Resources.StartMinimized;
        }

        private void AddFieldDataBindings()
        {
            chkStartWithWindows.Checked = AppConfig.GetValue<bool>("autoStart", false);
            chkAlwaysOnTop.Checked = AppConfig.GetValue<bool>("alwaysOnTop", false);
            chkStartMinimized.Checked = AppConfig.GetValue<bool>("startMinimized", false);
        }

        private void SaveSettings()
        {
            UnfocusMinimizeIcon();
            AppConfig.AddOrUpdateAppSetting<bool>("autoStart", chkStartWithWindows.Checked);
            StartWithWindows.Active = chkStartWithWindows.Checked;
            AppConfig.AddOrUpdateAppSetting<bool>("alwaysOnTop", chkAlwaysOnTop.Checked);
            AppConfig.AddOrUpdateAppSetting<bool>("startMinimized", chkStartMinimized.Checked);
        }

        private void FocusMinimizeIcon()
        {
            minimizePanel.BackColor = Color.LightGray;
        }

        private void MoveForm(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Top = Cursor.Position.Y - Offset.Y;
            Left = Cursor.Position.X - Offset.X;
        }

        private void UpdateOffset(MouseEventArgs e)
        {
            Offset = new Point(e.X, e.Y);
        }

        private void UnfocusMinimizeIcon()
        {
            minimizePanel.BackColor = Color.White;
        }

        #endregion

        #region Events handling

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsTitle_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateOffset(e);
        }

        private void SettingsTitle_MouseMove(object sender, MouseEventArgs e)
        {
            MoveForm(e);
        }

        private void MinimizePanel_MouseEnter(object sender, EventArgs e)
        {
            FocusMinimizeIcon();
        }

        private void MinimizePanel_MouseLeave(object sender, EventArgs e)
        {
            UnfocusMinimizeIcon();
        }

        private void MinimizePanel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MinimizePanelFrame_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            InitializeFormFromSettings();
        }

        #endregion
    }
}