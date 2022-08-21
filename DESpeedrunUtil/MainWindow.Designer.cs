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
            this.autorunMacroCheckbox = new System.Windows.Forms.CheckBox();
            this.macroUpKeyLabel = new System.Windows.Forms.Label();
            this.macroDownKeyLabel = new System.Windows.Forms.Label();
            this.macroStatus = new System.Windows.Forms.Label();
            this.macroTitle = new System.Windows.Forms.Label();
            this.gameVersion = new System.Windows.Forms.Label();
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
            this.versionChangedLabel = new System.Windows.Forms.Label();
            this.refreshVersionsButton = new System.Windows.Forms.Button();
            this.changeVersionButton = new System.Windows.Forms.Button();
            this.firewallToggleButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.meathookToggleButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hotkeyField0
            // 
            this.hotkeyField0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.hotkeyField0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField0.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField0.ForeColor = System.Drawing.Color.LightGray;
            this.hotkeyField0.Location = new System.Drawing.Point(111, 89);
            this.hotkeyField0.Name = "hotkeyField0";
            this.hotkeyField0.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField0.Size = new System.Drawing.Size(134, 23);
            this.hotkeyField0.TabIndex = 2;
            this.hotkeyField0.Tag = "macroDown";
            // 
            // hotkeyField1
            // 
            this.hotkeyField1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.hotkeyField1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField1.ForeColor = System.Drawing.Color.LightGray;
            this.hotkeyField1.Location = new System.Drawing.Point(111, 64);
            this.hotkeyField1.Name = "hotkeyField1";
            this.hotkeyField1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField1.Size = new System.Drawing.Size(134, 23);
            this.hotkeyField1.TabIndex = 3;
            this.hotkeyField1.Tag = "macroUp";
            // 
            // autorunMacroCheckbox
            // 
            this.autorunMacroCheckbox.AutoSize = true;
            this.autorunMacroCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.autorunMacroCheckbox.ForeColor = System.Drawing.Color.LightGray;
            this.autorunMacroCheckbox.Location = new System.Drawing.Point(12, 42);
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
            this.macroUpKeyLabel.ForeColor = System.Drawing.Color.LightGray;
            this.macroUpKeyLabel.Location = new System.Drawing.Point(33, 68);
            this.macroUpKeyLabel.Name = "macroUpKeyLabel";
            this.macroUpKeyLabel.Size = new System.Drawing.Size(72, 15);
            this.macroUpKeyLabel.TabIndex = 7;
            this.macroUpKeyLabel.Text = "Upscroll Key";
            // 
            // macroDownKeyLabel
            // 
            this.macroDownKeyLabel.AutoSize = true;
            this.macroDownKeyLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroDownKeyLabel.ForeColor = System.Drawing.Color.LightGray;
            this.macroDownKeyLabel.Location = new System.Drawing.Point(17, 93);
            this.macroDownKeyLabel.Name = "macroDownKeyLabel";
            this.macroDownKeyLabel.Size = new System.Drawing.Size(88, 15);
            this.macroDownKeyLabel.TabIndex = 6;
            this.macroDownKeyLabel.Text = "Downscroll Key";
            // 
            // macroStatus
            // 
            this.macroStatus.AutoSize = true;
            this.macroStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroStatus.ForeColor = System.Drawing.Color.LightGray;
            this.macroStatus.Location = new System.Drawing.Point(529, 24);
            this.macroStatus.Name = "macroStatus";
            this.macroStatus.Size = new System.Drawing.Size(51, 15);
            this.macroStatus.TabIndex = 5;
            this.macroStatus.Text = "Stopped";
            // 
            // macroTitle
            // 
            this.macroTitle.AutoSize = true;
            this.macroTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.macroTitle.ForeColor = System.Drawing.Color.LightGray;
            this.macroTitle.Location = new System.Drawing.Point(12, 9);
            this.macroTitle.Name = "macroTitle";
            this.macroTitle.Size = new System.Drawing.Size(171, 30);
            this.macroTitle.TabIndex = 5;
            this.macroTitle.Text = "Freescroll Macro";
            // 
            // gameVersion
            // 
            this.gameVersion.AutoSize = true;
            this.gameVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameVersion.ForeColor = System.Drawing.Color.LightGray;
            this.gameVersion.Location = new System.Drawing.Point(490, 60);
            this.gameVersion.Name = "gameVersion";
            this.gameVersion.Size = new System.Drawing.Size(99, 21);
            this.gameVersion.TabIndex = 6;
            this.gameVersion.Text = "Not Running";
            // 
            // fpsLabel2
            // 
            this.fpsLabel2.AutoSize = true;
            this.fpsLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel2.ForeColor = System.Drawing.Color.LightGray;
            this.fpsLabel2.Location = new System.Drawing.Point(247, 245);
            this.fpsLabel2.Name = "fpsLabel2";
            this.fpsLabel2.Size = new System.Drawing.Size(26, 15);
            this.fpsLabel2.TabIndex = 19;
            this.fpsLabel2.Text = "FPS";
            // 
            // fpsLabel1
            // 
            this.fpsLabel1.AutoSize = true;
            this.fpsLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel1.ForeColor = System.Drawing.Color.LightGray;
            this.fpsLabel1.Location = new System.Drawing.Point(247, 221);
            this.fpsLabel1.Name = "fpsLabel1";
            this.fpsLabel1.Size = new System.Drawing.Size(26, 15);
            this.fpsLabel1.TabIndex = 18;
            this.fpsLabel1.Text = "FPS";
            // 
            // fpsLabel0
            // 
            this.fpsLabel0.AutoSize = true;
            this.fpsLabel0.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel0.ForeColor = System.Drawing.Color.LightGray;
            this.fpsLabel0.Location = new System.Drawing.Point(247, 197);
            this.fpsLabel0.Name = "fpsLabel0";
            this.fpsLabel0.Size = new System.Drawing.Size(26, 15);
            this.fpsLabel0.TabIndex = 17;
            this.fpsLabel0.Text = "FPS";
            // 
            // enableHotkeysCheckbox
            // 
            this.enableHotkeysCheckbox.AutoSize = true;
            this.enableHotkeysCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enableHotkeysCheckbox.ForeColor = System.Drawing.Color.LightGray;
            this.enableHotkeysCheckbox.Location = new System.Drawing.Point(12, 169);
            this.enableHotkeysCheckbox.Name = "enableHotkeysCheckbox";
            this.enableHotkeysCheckbox.Size = new System.Drawing.Size(107, 19);
            this.enableHotkeysCheckbox.TabIndex = 9;
            this.enableHotkeysCheckbox.Text = "Enable Hotkeys";
            this.enableHotkeysCheckbox.UseVisualStyleBackColor = true;
            // 
            // fpsInput2
            // 
            this.fpsInput2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.fpsInput2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsInput2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput2.ForeColor = System.Drawing.Color.LightGray;
            this.fpsInput2.Location = new System.Drawing.Point(217, 241);
            this.fpsInput2.MaxLength = 3;
            this.fpsInput2.Name = "fpsInput2";
            this.fpsInput2.Size = new System.Drawing.Size(29, 23);
            this.fpsInput2.TabIndex = 16;
            this.fpsInput2.Tag = "fpscap2";
            // 
            // fpsInput1
            // 
            this.fpsInput1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.fpsInput1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsInput1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput1.ForeColor = System.Drawing.Color.LightGray;
            this.fpsInput1.Location = new System.Drawing.Point(217, 217);
            this.fpsInput1.MaxLength = 3;
            this.fpsInput1.Name = "fpsInput1";
            this.fpsInput1.Size = new System.Drawing.Size(29, 23);
            this.fpsInput1.TabIndex = 15;
            this.fpsInput1.Tag = "fpscap1";
            // 
            // fpsInput0
            // 
            this.fpsInput0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.fpsInput0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsInput0.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput0.ForeColor = System.Drawing.Color.LightGray;
            this.fpsInput0.Location = new System.Drawing.Point(217, 193);
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
            this.fpsKey2Label.ForeColor = System.Drawing.Color.LightGray;
            this.fpsKey2Label.Location = new System.Drawing.Point(22, 245);
            this.fpsKey2Label.Name = "fpsKey2Label";
            this.fpsKey2Label.Size = new System.Drawing.Size(83, 15);
            this.fpsKey2Label.TabIndex = 13;
            this.fpsKey2Label.Text = "FPS Hotkey #3";
            // 
            // hotkeyField4
            // 
            this.hotkeyField4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.hotkeyField4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField4.ForeColor = System.Drawing.Color.LightGray;
            this.hotkeyField4.Location = new System.Drawing.Point(111, 241);
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
            this.fpsKey1Label.ForeColor = System.Drawing.Color.LightGray;
            this.fpsKey1Label.Location = new System.Drawing.Point(22, 221);
            this.fpsKey1Label.Name = "fpsKey1Label";
            this.fpsKey1Label.Size = new System.Drawing.Size(83, 15);
            this.fpsKey1Label.TabIndex = 11;
            this.fpsKey1Label.Text = "FPS Hotkey #2";
            // 
            // hotkeyField2
            // 
            this.hotkeyField2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.hotkeyField2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField2.ForeColor = System.Drawing.Color.LightGray;
            this.hotkeyField2.Location = new System.Drawing.Point(111, 193);
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
            this.fpsKey0Label.ForeColor = System.Drawing.Color.LightGray;
            this.fpsKey0Label.Location = new System.Drawing.Point(22, 197);
            this.fpsKey0Label.Name = "fpsKey0Label";
            this.fpsKey0Label.Size = new System.Drawing.Size(83, 15);
            this.fpsKey0Label.TabIndex = 10;
            this.fpsKey0Label.Text = "FPS Hotkey #1";
            // 
            // hotkeyField3
            // 
            this.hotkeyField3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.hotkeyField3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField3.ForeColor = System.Drawing.Color.LightGray;
            this.hotkeyField3.Location = new System.Drawing.Point(111, 217);
            this.hotkeyField3.Name = "hotkeyField3";
            this.hotkeyField3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField3.Size = new System.Drawing.Size(100, 23);
            this.hotkeyField3.TabIndex = 9;
            this.hotkeyField3.Tag = "fps1";
            // 
            // versionDropDownSelector
            // 
            this.versionDropDownSelector.BackColor = System.Drawing.SystemColors.Control;
            this.versionDropDownSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionDropDownSelector.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.versionDropDownSelector.FormattingEnabled = true;
            this.versionDropDownSelector.Location = new System.Drawing.Point(116, 328);
            this.versionDropDownSelector.Name = "versionDropDownSelector";
            this.versionDropDownSelector.Size = new System.Drawing.Size(92, 23);
            this.versionDropDownSelector.TabIndex = 9;
            // 
            // versionChangedLabel
            // 
            this.versionChangedLabel.AutoSize = true;
            this.versionChangedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.versionChangedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.versionChangedLabel.Location = new System.Drawing.Point(214, 332);
            this.versionChangedLabel.Name = "versionChangedLabel";
            this.versionChangedLabel.Size = new System.Drawing.Size(102, 15);
            this.versionChangedLabel.TabIndex = 9;
            this.versionChangedLabel.Text = "Version Swapped";
            // 
            // refreshVersionsButton
            // 
            this.refreshVersionsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.refreshVersionsButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.refreshVersionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshVersionsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.refreshVersionsButton.ForeColor = System.Drawing.Color.LightGray;
            this.refreshVersionsButton.Location = new System.Drawing.Point(11, 360);
            this.refreshVersionsButton.Name = "refreshVersionsButton";
            this.refreshVersionsButton.Size = new System.Drawing.Size(99, 29);
            this.refreshVersionsButton.TabIndex = 11;
            this.refreshVersionsButton.Text = "Refresh List";
            this.refreshVersionsButton.UseVisualStyleBackColor = false;
            // 
            // changeVersionButton
            // 
            this.changeVersionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.changeVersionButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.changeVersionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changeVersionButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.changeVersionButton.ForeColor = System.Drawing.Color.LightGray;
            this.changeVersionButton.Location = new System.Drawing.Point(11, 325);
            this.changeVersionButton.Name = "changeVersionButton";
            this.changeVersionButton.Size = new System.Drawing.Size(99, 29);
            this.changeVersionButton.TabIndex = 10;
            this.changeVersionButton.Text = "Change Version";
            this.changeVersionButton.UseVisualStyleBackColor = false;
            // 
            // firewallToggleButton
            // 
            this.firewallToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.firewallToggleButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.firewallToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.firewallToggleButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.firewallToggleButton.ForeColor = System.Drawing.Color.LightGray;
            this.firewallToggleButton.Location = new System.Drawing.Point(11, 441);
            this.firewallToggleButton.Name = "firewallToggleButton";
            this.firewallToggleButton.Size = new System.Drawing.Size(197, 29);
            this.firewallToggleButton.TabIndex = 0;
            this.firewallToggleButton.Text = "fwToggleButton";
            this.firewallToggleButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(452, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(12, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 30);
            this.label2.TabIndex = 21;
            this.label2.Text = "FPS Cap Toggles";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(12, 288);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 30);
            this.label3.TabIndex = 22;
            this.label3.Text = "Version Swapping";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(12, 408);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(227, 30);
            this.label4.TabIndex = 23;
            this.label4.Text = "Miscellaneous Options";
            // 
            // meathookToggleButton
            // 
            this.meathookToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.meathookToggleButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.meathookToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.meathookToggleButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.meathookToggleButton.ForeColor = System.Drawing.Color.LightGray;
            this.meathookToggleButton.Location = new System.Drawing.Point(11, 476);
            this.meathookToggleButton.Name = "meathookToggleButton";
            this.meathookToggleButton.Size = new System.Drawing.Size(197, 29);
            this.meathookToggleButton.TabIndex = 24;
            this.meathookToggleButton.Text = "mhToggleButton";
            this.meathookToggleButton.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(214, 483);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 15);
            this.label5.TabIndex = 25;
            this.label5.Text = "RESTART GAME";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(639, 764);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.meathookToggleButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.autorunMacroCheckbox);
            this.Controls.Add(this.fpsLabel2);
            this.Controls.Add(this.macroUpKeyLabel);
            this.Controls.Add(this.versionChangedLabel);
            this.Controls.Add(this.macroDownKeyLabel);
            this.Controls.Add(this.fpsLabel1);
            this.Controls.Add(this.macroStatus);
            this.Controls.Add(this.firewallToggleButton);
            this.Controls.Add(this.fpsLabel0);
            this.Controls.Add(this.hotkeyField0);
            this.Controls.Add(this.refreshVersionsButton);
            this.Controls.Add(this.hotkeyField1);
            this.Controls.Add(this.enableHotkeysCheckbox);
            this.Controls.Add(this.changeVersionButton);
            this.Controls.Add(this.fpsInput2);
            this.Controls.Add(this.fpsInput1);
            this.Controls.Add(this.versionDropDownSelector);
            this.Controls.Add(this.fpsInput0);
            this.Controls.Add(this.gameVersion);
            this.Controls.Add(this.fpsKey2Label);
            this.Controls.Add(this.macroTitle);
            this.Controls.Add(this.hotkeyField4);
            this.Controls.Add(this.fpsKey1Label);
            this.Controls.Add(this.hotkeyField2);
            this.Controls.Add(this.hotkeyField3);
            this.Controls.Add(this.fpsKey0Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DOOM Eternal Speedrun Utility";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label hotkeyField0;
        private Label hotkeyField1;
        private Label macroStatus;
        private Label macroUpKeyLabel;
        private Label macroDownKeyLabel;
        private Label macroTitle;
        private Label gameVersion;
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
        private Button refreshVersionsButton;
        private Button changeVersionButton;
        private Label versionChangedLabel;
        private Button firewallToggleButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button meathookToggleButton;
        private Label label5;
    }
}