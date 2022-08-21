namespace DESpeedrunUtil {
    partial class MainWindow {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.hotkeyField0 = new System.Windows.Forms.Label();
            this.hotkeyField1 = new System.Windows.Forms.Label();
            this.macroGroupBox = new System.Windows.Forms.GroupBox();
            this.autorunMacroCheckbox = new System.Windows.Forms.CheckBox();
            this.macroUpKeyLabel = new System.Windows.Forms.Label();
            this.macroDownKeyLabel = new System.Windows.Forms.Label();
            this.macroStatus = new System.Windows.Forms.Label();
            this.macroStatusLabel = new System.Windows.Forms.Label();
            this.gameVersionLabel = new System.Windows.Forms.Label();
            this.gameVersion = new System.Windows.Forms.Label();
            this.fpsGroupBox = new System.Windows.Forms.GroupBox();
            this.fpsLabel2 = new System.Windows.Forms.Label();
            this.fpsLabel1 = new System.Windows.Forms.Label();
            this.fpsLabel0 = new System.Windows.Forms.Label();
            this.enableHotkeysCheckbox = new System.Windows.Forms.CheckBox();
            this.fpsInput2 = new System.Windows.Forms.TextBox();
            this.fpsInput1 = new System.Windows.Forms.TextBox();
            this.fpsInput0 = new System.Windows.Forms.TextBox();
            this.fpsKey2Label = new System.Windows.Forms.Label();
            this.hotkeyField4 = new System.Windows.Forms.Label();
            this.fpsKey1Label = new System.Windows.Forms.Label();
            this.hotkeyField2 = new System.Windows.Forms.Label();
            this.fpsKey0Label = new System.Windows.Forms.Label();
            this.hotkeyField3 = new System.Windows.Forms.Label();
            this.versionDropDownSelector = new System.Windows.Forms.ComboBox();
            this.versionGroupBox = new System.Windows.Forms.GroupBox();
            this.versionChangedLabel = new System.Windows.Forms.Label();
            this.refreshVersionsButton = new System.Windows.Forms.Button();
            this.changeVersionButton = new System.Windows.Forms.Button();
            this.macroGroupBox.SuspendLayout();
            this.fpsGroupBox.SuspendLayout();
            this.versionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // hotkeyField0
            // 
            this.hotkeyField0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField0.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField0.Location = new System.Drawing.Point(105, 70);
            this.hotkeyField0.Name = "hotkeyField0";
            this.hotkeyField0.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField0.Size = new System.Drawing.Size(100, 23);
            this.hotkeyField0.TabIndex = 2;
            this.hotkeyField0.Tag = "macroDown";
            // 
            // hotkeyField1
            // 
            this.hotkeyField1.BackColor = System.Drawing.SystemColors.Control;
            this.hotkeyField1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField1.Location = new System.Drawing.Point(105, 46);
            this.hotkeyField1.Name = "hotkeyField1";
            this.hotkeyField1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField1.Size = new System.Drawing.Size(100, 23);
            this.hotkeyField1.TabIndex = 3;
            this.hotkeyField1.Tag = "macroUp";
            // 
            // macroGroupBox
            // 
            this.macroGroupBox.Controls.Add(this.autorunMacroCheckbox);
            this.macroGroupBox.Controls.Add(this.macroUpKeyLabel);
            this.macroGroupBox.Controls.Add(this.macroDownKeyLabel);
            this.macroGroupBox.Controls.Add(this.macroStatus);
            this.macroGroupBox.Controls.Add(this.macroStatusLabel);
            this.macroGroupBox.Controls.Add(this.hotkeyField0);
            this.macroGroupBox.Controls.Add(this.hotkeyField1);
            this.macroGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroGroupBox.Location = new System.Drawing.Point(12, 159);
            this.macroGroupBox.Name = "macroGroupBox";
            this.macroGroupBox.Size = new System.Drawing.Size(305, 102);
            this.macroGroupBox.TabIndex = 4;
            this.macroGroupBox.TabStop = false;
            this.macroGroupBox.Text = "Freescroll Macro";
            // 
            // autorunMacroCheckbox
            // 
            this.autorunMacroCheckbox.AutoSize = true;
            this.autorunMacroCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.autorunMacroCheckbox.Location = new System.Drawing.Point(11, 24);
            this.autorunMacroCheckbox.Name = "autorunMacroCheckbox";
            this.autorunMacroCheckbox.Size = new System.Drawing.Size(98, 19);
            this.autorunMacroCheckbox.TabIndex = 8;
            this.autorunMacroCheckbox.Text = "Enable Macro";
            this.autorunMacroCheckbox.UseVisualStyleBackColor = true;
            // 
            // macroUpKeyLabel
            // 
            this.macroUpKeyLabel.AutoSize = true;
            this.macroUpKeyLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroUpKeyLabel.Location = new System.Drawing.Point(27, 50);
            this.macroUpKeyLabel.Name = "macroUpKeyLabel";
            this.macroUpKeyLabel.Size = new System.Drawing.Size(72, 15);
            this.macroUpKeyLabel.TabIndex = 7;
            this.macroUpKeyLabel.Text = "Upscroll Key";
            // 
            // macroDownKeyLabel
            // 
            this.macroDownKeyLabel.AutoSize = true;
            this.macroDownKeyLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroDownKeyLabel.Location = new System.Drawing.Point(11, 74);
            this.macroDownKeyLabel.Name = "macroDownKeyLabel";
            this.macroDownKeyLabel.Size = new System.Drawing.Size(88, 15);
            this.macroDownKeyLabel.TabIndex = 6;
            this.macroDownKeyLabel.Text = "Downscroll Key";
            // 
            // macroStatus
            // 
            this.macroStatus.AutoSize = true;
            this.macroStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroStatus.Location = new System.Drawing.Point(237, 25);
            this.macroStatus.Name = "macroStatus";
            this.macroStatus.Size = new System.Drawing.Size(51, 15);
            this.macroStatus.TabIndex = 5;
            this.macroStatus.Text = "Stopped";
            // 
            // macroStatusLabel
            // 
            this.macroStatusLabel.AutoSize = true;
            this.macroStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroStatusLabel.Location = new System.Drawing.Point(197, 25);
            this.macroStatusLabel.Name = "macroStatusLabel";
            this.macroStatusLabel.Size = new System.Drawing.Size(42, 15);
            this.macroStatusLabel.TabIndex = 4;
            this.macroStatusLabel.Text = "Status:";
            // 
            // gameVersionLabel
            // 
            this.gameVersionLabel.AutoSize = true;
            this.gameVersionLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameVersionLabel.Location = new System.Drawing.Point(12, 9);
            this.gameVersionLabel.Name = "gameVersionLabel";
            this.gameVersionLabel.Size = new System.Drawing.Size(110, 21);
            this.gameVersionLabel.TabIndex = 5;
            this.gameVersionLabel.Text = "Game Version:";
            // 
            // gameVersion
            // 
            this.gameVersion.AutoSize = true;
            this.gameVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameVersion.Location = new System.Drawing.Point(117, 9);
            this.gameVersion.Name = "gameVersion";
            this.gameVersion.Size = new System.Drawing.Size(99, 21);
            this.gameVersion.TabIndex = 6;
            this.gameVersion.Text = "Not Running";
            // 
            // fpsGroupBox
            // 
            this.fpsGroupBox.Controls.Add(this.fpsLabel2);
            this.fpsGroupBox.Controls.Add(this.fpsLabel1);
            this.fpsGroupBox.Controls.Add(this.fpsLabel0);
            this.fpsGroupBox.Controls.Add(this.enableHotkeysCheckbox);
            this.fpsGroupBox.Controls.Add(this.fpsInput2);
            this.fpsGroupBox.Controls.Add(this.fpsInput1);
            this.fpsGroupBox.Controls.Add(this.fpsInput0);
            this.fpsGroupBox.Controls.Add(this.fpsKey2Label);
            this.fpsGroupBox.Controls.Add(this.hotkeyField4);
            this.fpsGroupBox.Controls.Add(this.fpsKey1Label);
            this.fpsGroupBox.Controls.Add(this.hotkeyField2);
            this.fpsGroupBox.Controls.Add(this.fpsKey0Label);
            this.fpsGroupBox.Controls.Add(this.hotkeyField3);
            this.fpsGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsGroupBox.Location = new System.Drawing.Point(12, 265);
            this.fpsGroupBox.Name = "fpsGroupBox";
            this.fpsGroupBox.Size = new System.Drawing.Size(305, 126);
            this.fpsGroupBox.TabIndex = 8;
            this.fpsGroupBox.TabStop = false;
            this.fpsGroupBox.Text = "FPS Cap Toggles";
            // 
            // fpsLabel2
            // 
            this.fpsLabel2.AutoSize = true;
            this.fpsLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel2.Location = new System.Drawing.Point(240, 98);
            this.fpsLabel2.Name = "fpsLabel2";
            this.fpsLabel2.Size = new System.Drawing.Size(26, 15);
            this.fpsLabel2.TabIndex = 19;
            this.fpsLabel2.Text = "FPS";
            // 
            // fpsLabel1
            // 
            this.fpsLabel1.AutoSize = true;
            this.fpsLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel1.Location = new System.Drawing.Point(240, 74);
            this.fpsLabel1.Name = "fpsLabel1";
            this.fpsLabel1.Size = new System.Drawing.Size(26, 15);
            this.fpsLabel1.TabIndex = 18;
            this.fpsLabel1.Text = "FPS";
            // 
            // fpsLabel0
            // 
            this.fpsLabel0.AutoSize = true;
            this.fpsLabel0.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel0.Location = new System.Drawing.Point(240, 50);
            this.fpsLabel0.Name = "fpsLabel0";
            this.fpsLabel0.Size = new System.Drawing.Size(26, 15);
            this.fpsLabel0.TabIndex = 17;
            this.fpsLabel0.Text = "FPS";
            // 
            // enableHotkeysCheckbox
            // 
            this.enableHotkeysCheckbox.AutoSize = true;
            this.enableHotkeysCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enableHotkeysCheckbox.Location = new System.Drawing.Point(15, 24);
            this.enableHotkeysCheckbox.Name = "enableHotkeysCheckbox";
            this.enableHotkeysCheckbox.Size = new System.Drawing.Size(107, 19);
            this.enableHotkeysCheckbox.TabIndex = 9;
            this.enableHotkeysCheckbox.Text = "Enable Hotkeys";
            this.enableHotkeysCheckbox.UseVisualStyleBackColor = true;
            // 
            // fpsInput2
            // 
            this.fpsInput2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput2.Location = new System.Drawing.Point(210, 95);
            this.fpsInput2.MaxLength = 3;
            this.fpsInput2.Name = "fpsInput2";
            this.fpsInput2.Size = new System.Drawing.Size(29, 23);
            this.fpsInput2.TabIndex = 16;
            this.fpsInput2.Tag = "fpscap2";
            // 
            // fpsInput1
            // 
            this.fpsInput1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput1.Location = new System.Drawing.Point(210, 70);
            this.fpsInput1.MaxLength = 3;
            this.fpsInput1.Name = "fpsInput1";
            this.fpsInput1.Size = new System.Drawing.Size(29, 23);
            this.fpsInput1.TabIndex = 15;
            this.fpsInput1.Tag = "fpscap1";
            // 
            // fpsInput0
            // 
            this.fpsInput0.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput0.Location = new System.Drawing.Point(210, 46);
            this.fpsInput0.MaxLength = 3;
            this.fpsInput0.Name = "fpsInput0";
            this.fpsInput0.Size = new System.Drawing.Size(29, 23);
            this.fpsInput0.TabIndex = 14;
            this.fpsInput0.Tag = "fpscap0";
            // 
            // fpsKey2Label
            // 
            this.fpsKey2Label.AutoSize = true;
            this.fpsKey2Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsKey2Label.Location = new System.Drawing.Point(15, 98);
            this.fpsKey2Label.Name = "fpsKey2Label";
            this.fpsKey2Label.Size = new System.Drawing.Size(83, 15);
            this.fpsKey2Label.TabIndex = 13;
            this.fpsKey2Label.Text = "FPS Hotkey #3";
            // 
            // hotkeyField4
            // 
            this.hotkeyField4.BackColor = System.Drawing.SystemColors.Control;
            this.hotkeyField4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField4.Location = new System.Drawing.Point(104, 94);
            this.hotkeyField4.Name = "hotkeyField4";
            this.hotkeyField4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField4.Size = new System.Drawing.Size(100, 23);
            this.hotkeyField4.TabIndex = 12;
            this.hotkeyField4.Tag = "fps2";
            // 
            // fpsKey1Label
            // 
            this.fpsKey1Label.AutoSize = true;
            this.fpsKey1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsKey1Label.Location = new System.Drawing.Point(15, 74);
            this.fpsKey1Label.Name = "fpsKey1Label";
            this.fpsKey1Label.Size = new System.Drawing.Size(83, 15);
            this.fpsKey1Label.TabIndex = 11;
            this.fpsKey1Label.Text = "FPS Hotkey #2";
            // 
            // hotkeyField2
            // 
            this.hotkeyField2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField2.Location = new System.Drawing.Point(104, 46);
            this.hotkeyField2.Name = "hotkeyField2";
            this.hotkeyField2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField2.Size = new System.Drawing.Size(100, 23);
            this.hotkeyField2.TabIndex = 8;
            this.hotkeyField2.Tag = "fps0";
            // 
            // fpsKey0Label
            // 
            this.fpsKey0Label.AutoSize = true;
            this.fpsKey0Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsKey0Label.Location = new System.Drawing.Point(15, 50);
            this.fpsKey0Label.Name = "fpsKey0Label";
            this.fpsKey0Label.Size = new System.Drawing.Size(83, 15);
            this.fpsKey0Label.TabIndex = 10;
            this.fpsKey0Label.Text = "FPS Hotkey #1";
            // 
            // hotkeyField3
            // 
            this.hotkeyField3.BackColor = System.Drawing.SystemColors.Control;
            this.hotkeyField3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField3.Location = new System.Drawing.Point(104, 70);
            this.hotkeyField3.Name = "hotkeyField3";
            this.hotkeyField3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField3.Size = new System.Drawing.Size(100, 23);
            this.hotkeyField3.TabIndex = 9;
            this.hotkeyField3.Tag = "fps1";
            // 
            // versionDropDownSelector
            // 
            this.versionDropDownSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionDropDownSelector.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.versionDropDownSelector.FormattingEnabled = true;
            this.versionDropDownSelector.Location = new System.Drawing.Point(104, 24);
            this.versionDropDownSelector.Name = "versionDropDownSelector";
            this.versionDropDownSelector.Size = new System.Drawing.Size(92, 23);
            this.versionDropDownSelector.TabIndex = 9;
            // 
            // versionGroupBox
            // 
            this.versionGroupBox.Controls.Add(this.versionChangedLabel);
            this.versionGroupBox.Controls.Add(this.refreshVersionsButton);
            this.versionGroupBox.Controls.Add(this.changeVersionButton);
            this.versionGroupBox.Controls.Add(this.versionDropDownSelector);
            this.versionGroupBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.versionGroupBox.Location = new System.Drawing.Point(12, 397);
            this.versionGroupBox.Name = "versionGroupBox";
            this.versionGroupBox.Size = new System.Drawing.Size(305, 126);
            this.versionGroupBox.TabIndex = 20;
            this.versionGroupBox.TabStop = false;
            this.versionGroupBox.Text = "Version Switching";
            // 
            // versionChangedLabel
            // 
            this.versionChangedLabel.AutoSize = true;
            this.versionChangedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.versionChangedLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.versionChangedLabel.Location = new System.Drawing.Point(6, 28);
            this.versionChangedLabel.Name = "versionChangedLabel";
            this.versionChangedLabel.Size = new System.Drawing.Size(96, 15);
            this.versionChangedLabel.TabIndex = 9;
            this.versionChangedLabel.Text = "Version Swapped";
            // 
            // refreshVersionsButton
            // 
            this.refreshVersionsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.refreshVersionsButton.Location = new System.Drawing.Point(202, 53);
            this.refreshVersionsButton.Name = "refreshVersionsButton";
            this.refreshVersionsButton.Size = new System.Drawing.Size(97, 23);
            this.refreshVersionsButton.TabIndex = 11;
            this.refreshVersionsButton.Text = "Refresh List";
            this.refreshVersionsButton.UseVisualStyleBackColor = true;
            // 
            // changeVersionButton
            // 
            this.changeVersionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.changeVersionButton.Location = new System.Drawing.Point(202, 24);
            this.changeVersionButton.Name = "changeVersionButton";
            this.changeVersionButton.Size = new System.Drawing.Size(97, 23);
            this.changeVersionButton.TabIndex = 10;
            this.changeVersionButton.Text = "Change Version";
            this.changeVersionButton.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 764);
            this.Controls.Add(this.versionGroupBox);
            this.Controls.Add(this.fpsGroupBox);
            this.Controls.Add(this.gameVersion);
            this.Controls.Add(this.gameVersionLabel);
            this.Controls.Add(this.macroGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DOOM Eternal Speedrun Utility";
            this.macroGroupBox.ResumeLayout(false);
            this.macroGroupBox.PerformLayout();
            this.fpsGroupBox.ResumeLayout(false);
            this.fpsGroupBox.PerformLayout();
            this.versionGroupBox.ResumeLayout(false);
            this.versionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label hotkeyField0;
        private Label hotkeyField1;
        private GroupBox macroGroupBox;
        private Label macroStatus;
        private Label macroStatusLabel;
        private Label macroUpKeyLabel;
        private Label macroDownKeyLabel;
        private Label gameVersionLabel;
        private Label gameVersion;
        private GroupBox fpsGroupBox;
        private Label fpsKey2Label;
        private Label hotkeyField4;
        private Label fpsKey1Label;
        private Label hotkeyField2;
        private Label fpsKey0Label;
        private Label hotkeyField3;
        private TextBox fpsInput0;
        private TextBox fpsInput2;
        private TextBox fpsInput1;
        private CheckBox autorunMacroCheckbox;
        private CheckBox enableHotkeysCheckbox;
        private Label fpsLabel2;
        private Label fpsLabel1;
        private Label fpsLabel0;
        private ComboBox versionDropDownSelector;
        private GroupBox versionGroupBox;
        private Button refreshVersionsButton;
        private Button changeVersionButton;
        private Label versionChangedLabel;
    }
}