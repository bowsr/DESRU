namespace DESpeedrunUtil {
    partial class SettingsPage {
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
            settingsAACheckbox.Size = new Size(158, 22);
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
            settingsUNCheckbox.Size = new Size(273, 22);
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
            settingsAutoContinueCheckbox.Size = new Size(242, 22);
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
            settingsCloseButton.Location = new Point(232, 507);
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
            titleSeparator.Size = new Size(370, 2);
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
            settingsResetRunHotkeyField.Location = new Point(48, 287);
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
            settingsResetBindDescription.Location = new Point(48, 319);
            settingsResetBindDescription.Multiline = true;
            settingsResetBindDescription.Name = "settingsResetBindDescription";
            settingsResetBindDescription.ReadOnly = true;
            settingsResetBindDescription.Size = new Size(315, 182);
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
            settingsResetBindCheckbox.Size = new Size(341, 22);
            settingsResetBindCheckbox.TabIndex = 37;
            settingsResetBindCheckbox.Tag = "autocontinue";
            settingsResetBindCheckbox.Text = "Enable Reset Run / Kill Script Hotkey (Double Tap)";
            settingsResetBindCheckbox.UseVisualStyleBackColor = true;
            settingsResetBindCheckbox.CheckedChanged += ResetRunCheckbox_CheckChanged;
            // 
            // SettingsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(  35,   35,   35);
            ClientSize = new Size(394, 547);
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
    }
}