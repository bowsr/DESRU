﻿namespace DESpeedrunUtil {
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
            settingsAACheckbox.CheckedChanged += Checkbox_CheckedChanged;
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
            settingsUNCheckbox.CheckedChanged += Checkbox_CheckedChanged;
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
            settingsAutoContinueCheckbox.CheckedChanged += Checkbox_CheckedChanged;
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
            settingsCloseButton.Location = new Point(232, 220);
            settingsCloseButton.Name = "settingsCloseButton";
            settingsCloseButton.Size = new Size(150, 28);
            settingsCloseButton.TabIndex = 8;
            settingsCloseButton.Text = "close";
            settingsCloseButton.UseVisualStyleBackColor = false;
            settingsCloseButton.Click += CloseButton_Click;
            // 
            // SettingsPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(  35,   35,   35);
            ClientSize = new Size(394, 260);
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
            Text = "More Settings";
            TopMost = true;
            FormClosing += SettingsPage_FormClosing;
            Load += SettingsPage_Load;
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
    }
}