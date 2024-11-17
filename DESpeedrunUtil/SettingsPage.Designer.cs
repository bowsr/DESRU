namespace DESpeedrunUtil
{
    partial class SettingsPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPage));
            settingsCVARLabel = new Label();
            settingsAACheckbox = new CheckBox();
            settingsUNCheckbox = new CheckBox();
            settingsAutoContinueCheckbox = new CheckBox();
            settingsAADescription = new TextBox();
            settingsUNDescription = new TextBox();
            settingsAutoContinueDescription = new TextBox();
            settingsCloseButton = new Button();
            titleSeparator = new Panel();
            settingsHotkeyLabel = new Label();
            settingsResetRunHotkeyField = new Label();
            settingsResetBindDescription = new TextBox();
            settingsResetBindCheckbox = new CheckBox();
            settingsFontSlider = new TrackBar();
            settingsFontSizeDescription = new TextBox();
            settingsFontSizeCheckbox = new CheckBox();
            settingsFontSliderText = new TextBox();
            settingsConsoleHotkeyDescription = new TextBox();
            settingsConsoleHotkeyCheckbox = new CheckBox();
            settingsManualAltTabCheckbox = new CheckBox();
            settingsResetRunConsoleKey = new Label();
            settingsDevConsoleKeyDescription = new TextBox();
            ((System.ComponentModel.ISupportInitialize) settingsFontSlider).BeginInit();
            SuspendLayout();
            // 
            // settingsCVARLabel
            // 
            settingsCVARLabel.AutoSize = true;
            settingsCVARLabel.Font = new Font("Eternal UI 2", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            settingsCVARLabel.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsCVARLabel.Location = new Point(12, 9);
            settingsCVARLabel.Name = "settingsCVARLabel";
            settingsCVARLabel.Size = new Size(98, 33);
            settingsCVARLabel.TabIndex = 0;
            settingsCVARLabel.Text = "CVARS";
            // 
            // settingsAACheckbox
            // 
            settingsAACheckbox.AutoSize = true;
            settingsAACheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsAACheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsAACheckbox.Location = new Point(27, 45);
            settingsAACheckbox.Name = "settingsAACheckbox";
            settingsAACheckbox.Size = new Size(148, 22);
            settingsAACheckbox.TabIndex = 1;
            settingsAACheckbox.Tag = "antialiasing";
            settingsAACheckbox.Text = "Disable Anti-Aliasing";
            settingsAACheckbox.UseVisualStyleBackColor = true;
            settingsAACheckbox.CheckedChanged += CVARCheckbox_CheckedChanged;
            // 
            // settingsUNCheckbox
            // 
            settingsUNCheckbox.AutoSize = true;
            settingsUNCheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsUNCheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsUNCheckbox.Location = new Point(27, 98);
            settingsUNCheckbox.Name = "settingsUNCheckbox";
            settingsUNCheckbox.Size = new Size(253, 22);
            settingsUNCheckbox.TabIndex = 2;
            settingsUNCheckbox.Tag = "undelay";
            settingsUNCheckbox.Text = "Disable Ultra-Nightmare Quitout Delay";
            settingsUNCheckbox.UseVisualStyleBackColor = true;
            settingsUNCheckbox.CheckedChanged += CVARCheckbox_CheckedChanged;
            // 
            // settingsAutoContinueCheckbox
            // 
            settingsAutoContinueCheckbox.AutoSize = true;
            settingsAutoContinueCheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsAutoContinueCheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsAutoContinueCheckbox.Location = new Point(27, 151);
            settingsAutoContinueCheckbox.Name = "settingsAutoContinueCheckbox";
            settingsAutoContinueCheckbox.Size = new Size(224, 22);
            settingsAutoContinueCheckbox.TabIndex = 3;
            settingsAutoContinueCheckbox.Tag = "autocontinue";
            settingsAutoContinueCheckbox.Text = "Auto Continue in Loading Screens";
            settingsAutoContinueCheckbox.UseVisualStyleBackColor = true;
            settingsAutoContinueCheckbox.CheckedChanged += CVARCheckbox_CheckedChanged;
            // 
            // settingsAADescription
            // 
            settingsAADescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsAADescription.BorderStyle = BorderStyle.None;
            settingsAADescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsAADescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsAADescription.Location = new Point(48, 65);
            settingsAADescription.Multiline = true;
            settingsAADescription.Name = "settingsAADescription";
            settingsAADescription.ReadOnly = true;
            settingsAADescription.Size = new Size(315, 32);
            settingsAADescription.TabIndex = 5;
            settingsAADescription.Text = "NOTE: This setting does nothing if you have DLSS enabled.";
            // 
            // settingsUNDescription
            // 
            settingsUNDescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsUNDescription.BorderStyle = BorderStyle.None;
            settingsUNDescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsUNDescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsUNDescription.Location = new Point(48, 118);
            settingsUNDescription.Multiline = true;
            settingsUNDescription.Name = "settingsUNDescription";
            settingsUNDescription.ReadOnly = true;
            settingsUNDescription.Size = new Size(315, 32);
            settingsUNDescription.TabIndex = 6;
            settingsUNDescription.Text = "Removes the delay on some versions before you're able to leave an Ultra-Nightmare run.";
            // 
            // settingsAutoContinueDescription
            // 
            settingsAutoContinueDescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsAutoContinueDescription.BorderStyle = BorderStyle.None;
            settingsAutoContinueDescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsAutoContinueDescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsAutoContinueDescription.Location = new Point(48, 171);
            settingsAutoContinueDescription.Multiline = true;
            settingsAutoContinueDescription.Name = "settingsAutoContinueDescription";
            settingsAutoContinueDescription.ReadOnly = true;
            settingsAutoContinueDescription.Size = new Size(315, 32);
            settingsAutoContinueDescription.TabIndex = 7;
            settingsAutoContinueDescription.Text = "Forces loading screens to continue into the next level without waiting for user input.";
            // 
            // settingsCloseButton
            // 
            settingsCloseButton.BackColor = Color.FromArgb(  70,   70,   70);
            settingsCloseButton.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            settingsCloseButton.FlatStyle = FlatStyle.Flat;
            settingsCloseButton.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            settingsCloseButton.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsCloseButton.Location = new Point(602, 507);
            settingsCloseButton.Name = "settingsCloseButton";
            settingsCloseButton.Size = new Size(150, 28);
            settingsCloseButton.TabIndex = 8;
            settingsCloseButton.Text = "close";
            settingsCloseButton.UseVisualStyleBackColor = false;
            settingsCloseButton.Click += CloseButton_Click;
            // 
            // titleSeparator
            // 
            titleSeparator.BackColor = Color.FromArgb(  85,   85,   85);
            titleSeparator.Location = new Point(12, 215);
            titleSeparator.Name = "titleSeparator";
            titleSeparator.Size = new Size(740, 2);
            titleSeparator.TabIndex = 32;
            // 
            // settingsHotkeyLabel
            // 
            settingsHotkeyLabel.AutoSize = true;
            settingsHotkeyLabel.Font = new Font("Eternal UI 2", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            settingsHotkeyLabel.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsHotkeyLabel.Location = new Point(12, 226);
            settingsHotkeyLabel.Name = "settingsHotkeyLabel";
            settingsHotkeyLabel.Size = new Size(131, 33);
            settingsHotkeyLabel.TabIndex = 33;
            settingsHotkeyLabel.Text = "HOTKEYS";
            // 
            // settingsResetRunHotkeyField
            // 
            settingsResetRunHotkeyField.BackColor = Color.FromArgb(  70,   70,   70);
            settingsResetRunHotkeyField.BorderStyle = BorderStyle.FixedSingle;
            settingsResetRunHotkeyField.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsResetRunHotkeyField.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsResetRunHotkeyField.Location = new Point(48, 312);
            settingsResetRunHotkeyField.Name = "settingsResetRunHotkeyField";
            settingsResetRunHotkeyField.Padding = new Padding(0, 3, 0, 0);
            settingsResetRunHotkeyField.Size = new Size(135, 26);
            settingsResetRunHotkeyField.TabIndex = 34;
            settingsResetRunHotkeyField.Tag = "hkResetBind";
            settingsResetRunHotkeyField.Click += HotkeyAssignment_FieldSelected;
            // 
            // settingsResetBindDescription
            // 
            settingsResetBindDescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsResetBindDescription.BorderStyle = BorderStyle.None;
            settingsResetBindDescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsResetBindDescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsResetBindDescription.Location = new Point(48, 344);
            settingsResetBindDescription.Multiline = true;
            settingsResetBindDescription.Name = "settingsResetBindDescription";
            settingsResetBindDescription.ReadOnly = true;
            settingsResetBindDescription.Size = new Size(315, 161);
            settingsResetBindDescription.TabIndex = 36;
            settingsResetBindDescription.Text = resources.GetString("settingsResetBindDescription.Text");
            // 
            // settingsResetBindCheckbox
            // 
            settingsResetBindCheckbox.AutoSize = true;
            settingsResetBindCheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsResetBindCheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsResetBindCheckbox.Location = new Point(27, 262);
            settingsResetBindCheckbox.Name = "settingsResetBindCheckbox";
            settingsResetBindCheckbox.Size = new Size(319, 22);
            settingsResetBindCheckbox.TabIndex = 37;
            settingsResetBindCheckbox.Tag = "";
            settingsResetBindCheckbox.Text = "Enable Reset Run / Kill Script Hotkey (Double Tap)";
            settingsResetBindCheckbox.UseVisualStyleBackColor = true;
            settingsResetBindCheckbox.CheckedChanged += ResetRunCheckbox_CheckChanged;
            // 
            // settingsFontSlider
            // 
            settingsFontSlider.Location = new Point(408, 82);
            settingsFontSlider.Name = "settingsFontSlider";
            settingsFontSlider.Size = new Size(302, 45);
            settingsFontSlider.TabIndex = 38;
            settingsFontSlider.Value = 10;
            settingsFontSlider.Scroll += FontSlider_Changed;
            // 
            // settingsFontSizeDescription
            // 
            settingsFontSizeDescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsFontSizeDescription.BorderStyle = BorderStyle.None;
            settingsFontSizeDescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsFontSizeDescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsFontSizeDescription.Location = new Point(418, 65);
            settingsFontSizeDescription.Multiline = true;
            settingsFontSizeDescription.Name = "settingsFontSizeDescription";
            settingsFontSizeDescription.ReadOnly = true;
            settingsFontSizeDescription.Size = new Size(341, 77);
            settingsFontSizeDescription.TabIndex = 40;
            settingsFontSizeDescription.Text = "Changes the size of the On-Screen Display text.\r\n\r\n\r\n\r\nThis also changes the size of the in-game dev console's text.";
            // 
            // settingsFontSizeCheckbox
            // 
            settingsFontSizeCheckbox.AutoSize = true;
            settingsFontSizeCheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsFontSizeCheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsFontSizeCheckbox.Location = new Point(397, 45);
            settingsFontSizeCheckbox.Name = "settingsFontSizeCheckbox";
            settingsFontSizeCheckbox.Size = new Size(241, 22);
            settingsFontSizeCheckbox.TabIndex = 39;
            settingsFontSizeCheckbox.Tag = "";
            settingsFontSizeCheckbox.Text = "Change On-Screen Display Font Size";
            settingsFontSizeCheckbox.UseVisualStyleBackColor = true;
            settingsFontSizeCheckbox.CheckedChanged += FontSizeCheckbox_CheckChanged;
            // 
            // settingsFontSliderText
            // 
            settingsFontSliderText.BackColor = Color.FromArgb(  35,   35,   35);
            settingsFontSliderText.BorderStyle = BorderStyle.None;
            settingsFontSliderText.Font = new Font("Eternal UI 2", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            settingsFontSliderText.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsFontSliderText.Location = new Point(709, 77);
            settingsFontSliderText.Multiline = true;
            settingsFontSliderText.Name = "settingsFontSliderText";
            settingsFontSliderText.ReadOnly = true;
            settingsFontSliderText.Size = new Size(50, 34);
            settingsFontSliderText.TabIndex = 41;
            settingsFontSliderText.Text = "10";
            // 
            // settingsConsoleHotkeyDescription
            // 
            settingsConsoleHotkeyDescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsConsoleHotkeyDescription.BorderStyle = BorderStyle.None;
            settingsConsoleHotkeyDescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsConsoleHotkeyDescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsConsoleHotkeyDescription.Location = new Point(418, 171);
            settingsConsoleHotkeyDescription.Multiline = true;
            settingsConsoleHotkeyDescription.Name = "settingsConsoleHotkeyDescription";
            settingsConsoleHotkeyDescription.ReadOnly = true;
            settingsConsoleHotkeyDescription.Size = new Size(315, 32);
            settingsConsoleHotkeyDescription.TabIndex = 43;
            settingsConsoleHotkeyDescription.Text = "Helps prevent accidental dev console openings when pressing other nearby keys.";
            // 
            // settingsConsoleHotkeyCheckbox
            // 
            settingsConsoleHotkeyCheckbox.AutoSize = true;
            settingsConsoleHotkeyCheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsConsoleHotkeyCheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsConsoleHotkeyCheckbox.Location = new Point(397, 151);
            settingsConsoleHotkeyCheckbox.Name = "settingsConsoleHotkeyCheckbox";
            settingsConsoleHotkeyCheckbox.Size = new Size(288, 22);
            settingsConsoleHotkeyCheckbox.TabIndex = 42;
            settingsConsoleHotkeyCheckbox.Tag = "";
            settingsConsoleHotkeyCheckbox.Text = "Change Console Hotkey to CTRL + SHIFT + ~";
            settingsConsoleHotkeyCheckbox.UseVisualStyleBackColor = true;
            settingsConsoleHotkeyCheckbox.CheckedChanged += CVARCheckbox_CheckedChanged;
            // 
            // settingsManualAltTabCheckbox
            // 
            settingsManualAltTabCheckbox.AutoSize = true;
            settingsManualAltTabCheckbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsManualAltTabCheckbox.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsManualAltTabCheckbox.Location = new Point(50, 283);
            settingsManualAltTabCheckbox.Name = "settingsManualAltTabCheckbox";
            settingsManualAltTabCheckbox.Size = new Size(178, 22);
            settingsManualAltTabCheckbox.TabIndex = 44;
            settingsManualAltTabCheckbox.Tag = "";
            settingsManualAltTabCheckbox.Text = "Prevent automatic alt-tab";
            settingsManualAltTabCheckbox.UseVisualStyleBackColor = true;
            settingsManualAltTabCheckbox.CheckedChanged += GenericCheckbox_CheckChanged;
            // 
            // settingsResetRunConsoleKey
            // 
            settingsResetRunConsoleKey.BackColor = Color.FromArgb(  70,   70,   70);
            settingsResetRunConsoleKey.BorderStyle = BorderStyle.FixedSingle;
            settingsResetRunConsoleKey.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            settingsResetRunConsoleKey.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsResetRunConsoleKey.Location = new Point(48, 507);
            settingsResetRunConsoleKey.Name = "settingsResetRunConsoleKey";
            settingsResetRunConsoleKey.Padding = new Padding(0, 3, 0, 0);
            settingsResetRunConsoleKey.Size = new Size(135, 26);
            settingsResetRunConsoleKey.TabIndex = 45;
            settingsResetRunConsoleKey.Tag = "hkConsoleKey";
            settingsResetRunConsoleKey.Click += HotkeyAssignment_FieldSelected;
            // 
            // settingsDevConsoleKeyDesc
            // 
            settingsDevConsoleKeyDescription.BackColor = Color.FromArgb(  35,   35,   35);
            settingsDevConsoleKeyDescription.BorderStyle = BorderStyle.None;
            settingsDevConsoleKeyDescription.Font = new Font("Eternal UI 2", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            settingsDevConsoleKeyDescription.ForeColor = Color.FromArgb(  230,   230,   230);
            settingsDevConsoleKeyDescription.Location = new Point(189, 513);
            settingsDevConsoleKeyDescription.Multiline = true;
            settingsDevConsoleKeyDescription.Name = "settingsDevConsoleKeyDesc";
            settingsDevConsoleKeyDescription.ReadOnly = true;
            settingsDevConsoleKeyDescription.Size = new Size(89, 15);
            settingsDevConsoleKeyDescription.TabIndex = 47;
            settingsDevConsoleKeyDescription.Text = "Dev Console Key";
            // 
            // SettingsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(  35,   35,   35);
            ClientSize = new Size(764, 547);
            Controls.Add(settingsDevConsoleKeyDescription);
            Controls.Add(settingsResetRunConsoleKey);
            Controls.Add(settingsManualAltTabCheckbox);
            Controls.Add(settingsConsoleHotkeyDescription);
            Controls.Add(settingsConsoleHotkeyCheckbox);
            Controls.Add(settingsFontSliderText);
            Controls.Add(settingsFontSlider);
            Controls.Add(settingsFontSizeDescription);
            Controls.Add(settingsFontSizeCheckbox);
            Controls.Add(settingsResetBindCheckbox);
            Controls.Add(settingsResetBindDescription);
            Controls.Add(settingsResetRunHotkeyField);
            Controls.Add(settingsHotkeyLabel);
            Controls.Add(titleSeparator);
            Controls.Add(settingsCloseButton);
            Controls.Add(settingsAutoContinueDescription);
            Controls.Add(settingsUNDescription);
            Controls.Add(settingsAADescription);
            Controls.Add(settingsAutoContinueCheckbox);
            Controls.Add(settingsUNCheckbox);
            Controls.Add(settingsAACheckbox);
            Controls.Add(settingsCVARLabel);
            Icon = (Icon) resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsPage";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "More Settings";
            TopMost = true;
            FormClosing += SettingsPage_FormClosing;
            Load += SettingsPage_Load;
            KeyDown += HotkeyAssignment_KeyDown;
            ((System.ComponentModel.ISupportInitialize) settingsFontSlider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label settingsCVARLabel;
        private CheckBox settingsAACheckbox;
        private CheckBox settingsUNCheckbox;
        private CheckBox settingsAutoContinueCheckbox;
        private TextBox settingsAADescription;
        private TextBox settingsUNDescription;
        private TextBox settingsAutoContinueDescription;
        private Button settingsCloseButton;
        private Panel titleSeparator;
        private Label settingsHotkeyLabel;
        private Label settingsResetRunHotkeyField;
        private TextBox settingsResetBindDescription;
        private CheckBox settingsResetBindCheckbox;
        private TrackBar settingsFontSlider;
        private TextBox settingsFontSizeDescription;
        private CheckBox settingsFontSizeCheckbox;
        private TextBox settingsFontSliderText;
        private TextBox settingsConsoleHotkeyDescription;
        private CheckBox settingsConsoleHotkeyCheckbox;
        private CheckBox settingsManualAltTabCheckbox;
        private Label settingsResetRunConsoleKey;
        private TextBox settingsDevConsoleKeyDescription;
    }
}