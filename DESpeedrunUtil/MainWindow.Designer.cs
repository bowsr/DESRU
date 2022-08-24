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
            this.hotkeysTitle = new System.Windows.Forms.Label();
            this.gameVersion = new System.Windows.Forms.Label();
            this.fpsLabel2 = new System.Windows.Forms.Label();
            this.fpsLabel1 = new System.Windows.Forms.Label();
            this.fpsLabel0 = new System.Windows.Forms.Label();
            this.enableHotkeysCheckbox = new System.Windows.Forms.CheckBox();
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
            this.defaultFPSLabel = new System.Windows.Forms.Label();
            this.windowTitle = new System.Windows.Forms.Label();
            this.versionTitle = new System.Windows.Forms.Label();
            this.optionsTitle = new System.Windows.Forms.Label();
            this.meathookToggleButton = new System.Windows.Forms.Button();
            this.meathookRestartLabel = new System.Windows.Forms.Label();
            this.firewallRestartLabel = new System.Windows.Forms.Label();
            this.hotkeyPanel = new System.Windows.Forms.Panel();
            this.fpsPanel1 = new System.Windows.Forms.Panel();
            this.fpsInput1 = new System.Windows.Forms.TextBox();
            this.fpsPanel2 = new System.Windows.Forms.Panel();
            this.fpsInput2 = new System.Windows.Forms.TextBox();
            this.fpsPanel0 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.defaultFPS = new System.Windows.Forms.TextBox();
            this.titleSeparator = new System.Windows.Forms.Panel();
            this.exitButton = new System.Windows.Forms.Button();
            this.hotkeyPanel.SuspendLayout();
            this.fpsPanel1.SuspendLayout();
            this.fpsPanel2.SuspendLayout();
            this.fpsPanel0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // hotkeyField0
            // 
            this.hotkeyField0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.hotkeyField0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.hotkeyField0.Location = new System.Drawing.Point(126, 34);
            this.hotkeyField0.Name = "hotkeyField0";
            this.hotkeyField0.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField0.Size = new System.Drawing.Size(135, 26);
            this.hotkeyField0.TabIndex = 2;
            this.hotkeyField0.Tag = "macroDown";
            this.hotkeyField0.Click += new System.EventHandler(this.HotkeyAssignment_FieldSelected);
            // 
            // hotkeyField1
            // 
            this.hotkeyField1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.hotkeyField1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.hotkeyField1.Location = new System.Drawing.Point(126, 5);
            this.hotkeyField1.Name = "hotkeyField1";
            this.hotkeyField1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField1.Size = new System.Drawing.Size(135, 26);
            this.hotkeyField1.TabIndex = 3;
            this.hotkeyField1.Tag = "macroUp";
            this.hotkeyField1.Click += new System.EventHandler(this.HotkeyAssignment_FieldSelected);
            // 
            // autorunMacroCheckbox
            // 
            this.autorunMacroCheckbox.AutoSize = true;
            this.autorunMacroCheckbox.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.autorunMacroCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.autorunMacroCheckbox.Location = new System.Drawing.Point(6, 8);
            this.autorunMacroCheckbox.Name = "autorunMacroCheckbox";
            this.autorunMacroCheckbox.Size = new System.Drawing.Size(113, 22);
            this.autorunMacroCheckbox.TabIndex = 8;
            this.autorunMacroCheckbox.Text = "Enable Macro";
            this.autorunMacroCheckbox.UseVisualStyleBackColor = true;
            this.autorunMacroCheckbox.CheckedChanged += new System.EventHandler(this.AutoStartMacro_CheckChanged);
            // 
            // macroUpKeyLabel
            // 
            this.macroUpKeyLabel.AutoSize = true;
            this.macroUpKeyLabel.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroUpKeyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.macroUpKeyLabel.Location = new System.Drawing.Point(21, 8);
            this.macroUpKeyLabel.Name = "macroUpKeyLabel";
            this.macroUpKeyLabel.Size = new System.Drawing.Size(102, 18);
            this.macroUpKeyLabel.TabIndex = 7;
            this.macroUpKeyLabel.Text = "Macro Upscroll";
            // 
            // macroDownKeyLabel
            // 
            this.macroDownKeyLabel.AutoSize = true;
            this.macroDownKeyLabel.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroDownKeyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.macroDownKeyLabel.Location = new System.Drawing.Point(4, 37);
            this.macroDownKeyLabel.Name = "macroDownKeyLabel";
            this.macroDownKeyLabel.Size = new System.Drawing.Size(119, 18);
            this.macroDownKeyLabel.TabIndex = 6;
            this.macroDownKeyLabel.Text = "Macro Downscroll";
            // 
            // macroStatus
            // 
            this.macroStatus.AutoSize = true;
            this.macroStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.macroStatus.Location = new System.Drawing.Point(432, 264);
            this.macroStatus.Name = "macroStatus";
            this.macroStatus.Size = new System.Drawing.Size(51, 15);
            this.macroStatus.TabIndex = 5;
            this.macroStatus.Text = "Stopped";
            // 
            // hotkeysTitle
            // 
            this.hotkeysTitle.AutoSize = true;
            this.hotkeysTitle.Font = new System.Drawing.Font("Eternal Logo", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.hotkeysTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.hotkeysTitle.Location = new System.Drawing.Point(12, 64);
            this.hotkeysTitle.Name = "hotkeysTitle";
            this.hotkeysTitle.Size = new System.Drawing.Size(147, 21);
            this.hotkeysTitle.TabIndex = 5;
            this.hotkeysTitle.Text = "KEYBINDS";
            this.hotkeysTitle.Visible = false;
            // 
            // gameVersion
            // 
            this.gameVersion.AutoSize = true;
            this.gameVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.gameVersion.Location = new System.Drawing.Point(484, 324);
            this.gameVersion.Name = "gameVersion";
            this.gameVersion.Size = new System.Drawing.Size(99, 21);
            this.gameVersion.TabIndex = 6;
            this.gameVersion.Text = "Not Running";
            // 
            // fpsLabel2
            // 
            this.fpsLabel2.AutoSize = true;
            this.fpsLabel2.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsLabel2.Location = new System.Drawing.Point(264, 127);
            this.fpsLabel2.Name = "fpsLabel2";
            this.fpsLabel2.Size = new System.Drawing.Size(33, 18);
            this.fpsLabel2.TabIndex = 19;
            this.fpsLabel2.Text = "FPS";
            // 
            // fpsLabel1
            // 
            this.fpsLabel1.AutoSize = true;
            this.fpsLabel1.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsLabel1.Location = new System.Drawing.Point(264, 98);
            this.fpsLabel1.Name = "fpsLabel1";
            this.fpsLabel1.Size = new System.Drawing.Size(33, 18);
            this.fpsLabel1.TabIndex = 18;
            this.fpsLabel1.Text = "FPS";
            // 
            // fpsLabel0
            // 
            this.fpsLabel0.AutoSize = true;
            this.fpsLabel0.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsLabel0.Location = new System.Drawing.Point(264, 69);
            this.fpsLabel0.Name = "fpsLabel0";
            this.fpsLabel0.Size = new System.Drawing.Size(33, 18);
            this.fpsLabel0.TabIndex = 17;
            this.fpsLabel0.Text = "FPS";
            // 
            // enableHotkeysCheckbox
            // 
            this.enableHotkeysCheckbox.AutoSize = true;
            this.enableHotkeysCheckbox.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enableHotkeysCheckbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.enableHotkeysCheckbox.Location = new System.Drawing.Point(6, 33);
            this.enableHotkeysCheckbox.Name = "enableHotkeysCheckbox";
            this.enableHotkeysCheckbox.Size = new System.Drawing.Size(152, 22);
            this.enableHotkeysCheckbox.TabIndex = 9;
            this.enableHotkeysCheckbox.Text = "Enable FPS Hotkeys";
            this.enableHotkeysCheckbox.UseVisualStyleBackColor = true;
            this.enableHotkeysCheckbox.CheckedChanged += new System.EventHandler(this.EnableHotkeys_CheckChanged);
            // 
            // fpsInput0
            // 
            this.fpsInput0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.fpsInput0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fpsInput0.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsInput0.Location = new System.Drawing.Point(-4, 3);
            this.fpsInput0.MaxLength = 3;
            this.fpsInput0.Name = "fpsInput0";
            this.fpsInput0.Size = new System.Drawing.Size(34, 20);
            this.fpsInput0.TabIndex = 14;
            this.fpsInput0.Tag = "fpscap0";
            this.fpsInput0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.fpsInput0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FPSInput_KeyPressNumericOnly);
            this.fpsInput0.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FPSInput_KeyUp);
            // 
            // fpsKey2Label
            // 
            this.fpsKey2Label.AutoSize = true;
            this.fpsKey2Label.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsKey2Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsKey2Label.Location = new System.Drawing.Point(20, 127);
            this.fpsKey2Label.Name = "fpsKey2Label";
            this.fpsKey2Label.Size = new System.Drawing.Size(103, 18);
            this.fpsKey2Label.TabIndex = 13;
            this.fpsKey2Label.Text = "FPS Hotkey #3";
            // 
            // hotkeyField4
            // 
            this.hotkeyField4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.hotkeyField4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.hotkeyField4.Location = new System.Drawing.Point(126, 123);
            this.hotkeyField4.Name = "hotkeyField4";
            this.hotkeyField4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField4.Size = new System.Drawing.Size(95, 26);
            this.hotkeyField4.TabIndex = 12;
            this.hotkeyField4.Tag = "fps2";
            this.hotkeyField4.Click += new System.EventHandler(this.HotkeyAssignment_FieldSelected);
            // 
            // fpsKey1Label
            // 
            this.fpsKey1Label.AutoSize = true;
            this.fpsKey1Label.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsKey1Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsKey1Label.Location = new System.Drawing.Point(20, 98);
            this.fpsKey1Label.Name = "fpsKey1Label";
            this.fpsKey1Label.Size = new System.Drawing.Size(103, 18);
            this.fpsKey1Label.TabIndex = 11;
            this.fpsKey1Label.Text = "FPS Hotkey #2";
            // 
            // hotkeyField2
            // 
            this.hotkeyField2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.hotkeyField2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.hotkeyField2.Location = new System.Drawing.Point(126, 65);
            this.hotkeyField2.Name = "hotkeyField2";
            this.hotkeyField2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField2.Size = new System.Drawing.Size(95, 26);
            this.hotkeyField2.TabIndex = 8;
            this.hotkeyField2.Tag = "fps0";
            this.hotkeyField2.Click += new System.EventHandler(this.HotkeyAssignment_FieldSelected);
            // 
            // fpsKey0Label
            // 
            this.fpsKey0Label.AutoSize = true;
            this.fpsKey0Label.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsKey0Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsKey0Label.Location = new System.Drawing.Point(20, 69);
            this.fpsKey0Label.Name = "fpsKey0Label";
            this.fpsKey0Label.Size = new System.Drawing.Size(103, 18);
            this.fpsKey0Label.TabIndex = 10;
            this.fpsKey0Label.Text = "FPS Hotkey #1";
            // 
            // hotkeyField3
            // 
            this.hotkeyField3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.hotkeyField3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.hotkeyField3.Location = new System.Drawing.Point(126, 94);
            this.hotkeyField3.Name = "hotkeyField3";
            this.hotkeyField3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.hotkeyField3.Size = new System.Drawing.Size(95, 26);
            this.hotkeyField3.TabIndex = 9;
            this.hotkeyField3.Tag = "fps1";
            this.hotkeyField3.Click += new System.EventHandler(this.HotkeyAssignment_FieldSelected);
            // 
            // versionDropDownSelector
            // 
            this.versionDropDownSelector.BackColor = System.Drawing.SystemColors.Control;
            this.versionDropDownSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionDropDownSelector.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.versionDropDownSelector.ForeColor = System.Drawing.Color.Black;
            this.versionDropDownSelector.FormattingEnabled = true;
            this.versionDropDownSelector.Location = new System.Drawing.Point(165, 13);
            this.versionDropDownSelector.Name = "versionDropDownSelector";
            this.versionDropDownSelector.Size = new System.Drawing.Size(123, 26);
            this.versionDropDownSelector.TabIndex = 9;
            this.versionDropDownSelector.SelectedIndexChanged += new System.EventHandler(this.DropDown_IndexChanged);
            // 
            // versionChangedLabel
            // 
            this.versionChangedLabel.AutoSize = true;
            this.versionChangedLabel.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.versionChangedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.versionChangedLabel.Location = new System.Drawing.Point(5, 17);
            this.versionChangedLabel.Name = "versionChangedLabel";
            this.versionChangedLabel.Size = new System.Drawing.Size(141, 18);
            this.versionChangedLabel.TabIndex = 9;
            this.versionChangedLabel.Text = "Version Swapped";
            // 
            // refreshVersionsButton
            // 
            this.refreshVersionsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.refreshVersionsButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.refreshVersionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshVersionsButton.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.refreshVersionsButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.refreshVersionsButton.Location = new System.Drawing.Point(154, 57);
            this.refreshVersionsButton.Name = "refreshVersionsButton";
            this.refreshVersionsButton.Size = new System.Drawing.Size(145, 32);
            this.refreshVersionsButton.TabIndex = 11;
            this.refreshVersionsButton.Text = "Refresh List";
            this.refreshVersionsButton.UseVisualStyleBackColor = false;
            this.refreshVersionsButton.Click += new System.EventHandler(this.RefreshVersions_Click);
            // 
            // changeVersionButton
            // 
            this.changeVersionButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.changeVersionButton.Enabled = false;
            this.changeVersionButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.changeVersionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changeVersionButton.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.changeVersionButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.changeVersionButton.Location = new System.Drawing.Point(3, 57);
            this.changeVersionButton.Name = "changeVersionButton";
            this.changeVersionButton.Size = new System.Drawing.Size(145, 32);
            this.changeVersionButton.TabIndex = 10;
            this.changeVersionButton.Text = "Change Version";
            this.changeVersionButton.UseVisualStyleBackColor = false;
            this.changeVersionButton.Click += new System.EventHandler(this.ChangeVersion_Click);
            // 
            // firewallToggleButton
            // 
            this.firewallToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.firewallToggleButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.firewallToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.firewallToggleButton.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.firewallToggleButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.firewallToggleButton.Location = new System.Drawing.Point(3, 62);
            this.firewallToggleButton.Name = "firewallToggleButton";
            this.firewallToggleButton.Size = new System.Drawing.Size(186, 32);
            this.firewallToggleButton.TabIndex = 0;
            this.firewallToggleButton.Text = "Remove Firewall Rule";
            this.firewallToggleButton.UseVisualStyleBackColor = false;
            this.firewallToggleButton.Click += new System.EventHandler(this.FirewallToggle_Click);
            // 
            // defaultFPSLabel
            // 
            this.defaultFPSLabel.AutoSize = true;
            this.defaultFPSLabel.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.defaultFPSLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.defaultFPSLabel.Location = new System.Drawing.Point(197, 34);
            this.defaultFPSLabel.Name = "defaultFPSLabel";
            this.defaultFPSLabel.Size = new System.Drawing.Size(64, 18);
            this.defaultFPSLabel.TabIndex = 20;
            this.defaultFPSLabel.Text = "Max FPS";
            // 
            // windowTitle
            // 
            this.windowTitle.AutoSize = true;
            this.windowTitle.BackColor = System.Drawing.Color.Transparent;
            this.windowTitle.Font = new System.Drawing.Font("Eternal Battle", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.windowTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.windowTitle.Location = new System.Drawing.Point(13, 9);
            this.windowTitle.Name = "windowTitle";
            this.windowTitle.Size = new System.Drawing.Size(614, 33);
            this.windowTitle.TabIndex = 21;
            this.windowTitle.Text = "DOOM ETERNAL SPEEDRUN UTILITY";
            this.windowTitle.Visible = false;
            // 
            // versionTitle
            // 
            this.versionTitle.AutoSize = true;
            this.versionTitle.Font = new System.Drawing.Font("Eternal Logo", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.versionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.versionTitle.Location = new System.Drawing.Point(12, 451);
            this.versionTitle.Name = "versionTitle";
            this.versionTitle.Size = new System.Drawing.Size(248, 21);
            this.versionTitle.TabIndex = 22;
            this.versionTitle.Text = "CHANGE VERSION";
            this.versionTitle.Visible = false;
            // 
            // optionsTitle
            // 
            this.optionsTitle.AutoSize = true;
            this.optionsTitle.Font = new System.Drawing.Font("Eternal Logo", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.optionsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.optionsTitle.Location = new System.Drawing.Point(12, 268);
            this.optionsTitle.Name = "optionsTitle";
            this.optionsTitle.Size = new System.Drawing.Size(129, 21);
            this.optionsTitle.TabIndex = 23;
            this.optionsTitle.Text = "OPTIONS";
            this.optionsTitle.Visible = false;
            // 
            // meathookToggleButton
            // 
            this.meathookToggleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.meathookToggleButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.meathookToggleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.meathookToggleButton.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.meathookToggleButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.meathookToggleButton.Location = new System.Drawing.Point(3, 100);
            this.meathookToggleButton.Name = "meathookToggleButton";
            this.meathookToggleButton.Size = new System.Drawing.Size(186, 29);
            this.meathookToggleButton.TabIndex = 24;
            this.meathookToggleButton.Text = "Disable Cheats";
            this.meathookToggleButton.UseVisualStyleBackColor = false;
            this.meathookToggleButton.Click += new System.EventHandler(this.MeathookToggle_Click);
            // 
            // meathookRestartLabel
            // 
            this.meathookRestartLabel.AutoSize = true;
            this.meathookRestartLabel.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.meathookRestartLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.meathookRestartLabel.Location = new System.Drawing.Point(189, 105);
            this.meathookRestartLabel.Name = "meathookRestartLabel";
            this.meathookRestartLabel.Size = new System.Drawing.Size(114, 18);
            this.meathookRestartLabel.TabIndex = 25;
            this.meathookRestartLabel.Text = "RESTART GAME";
            // 
            // firewallRestartLabel
            // 
            this.firewallRestartLabel.AutoSize = true;
            this.firewallRestartLabel.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.firewallRestartLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.firewallRestartLabel.Location = new System.Drawing.Point(189, 69);
            this.firewallRestartLabel.Name = "firewallRestartLabel";
            this.firewallRestartLabel.Size = new System.Drawing.Size(114, 18);
            this.firewallRestartLabel.TabIndex = 26;
            this.firewallRestartLabel.Text = "RESTART GAME";
            // 
            // hotkeyPanel
            // 
            this.hotkeyPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.hotkeyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyPanel.Controls.Add(this.fpsKey2Label);
            this.hotkeyPanel.Controls.Add(this.hotkeyField0);
            this.hotkeyPanel.Controls.Add(this.fpsKey1Label);
            this.hotkeyPanel.Controls.Add(this.hotkeyField2);
            this.hotkeyPanel.Controls.Add(this.fpsKey0Label);
            this.hotkeyPanel.Controls.Add(this.hotkeyField4);
            this.hotkeyPanel.Controls.Add(this.macroDownKeyLabel);
            this.hotkeyPanel.Controls.Add(this.fpsLabel0);
            this.hotkeyPanel.Controls.Add(this.fpsPanel1);
            this.hotkeyPanel.Controls.Add(this.fpsLabel1);
            this.hotkeyPanel.Controls.Add(this.macroUpKeyLabel);
            this.hotkeyPanel.Controls.Add(this.hotkeyField3);
            this.hotkeyPanel.Controls.Add(this.fpsPanel2);
            this.hotkeyPanel.Controls.Add(this.hotkeyField1);
            this.hotkeyPanel.Controls.Add(this.fpsPanel0);
            this.hotkeyPanel.Controls.Add(this.fpsLabel2);
            this.hotkeyPanel.Location = new System.Drawing.Point(12, 93);
            this.hotkeyPanel.Name = "hotkeyPanel";
            this.hotkeyPanel.Size = new System.Drawing.Size(304, 156);
            this.hotkeyPanel.TabIndex = 27;
            // 
            // fpsPanel1
            // 
            this.fpsPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.fpsPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsPanel1.Controls.Add(this.fpsInput1);
            this.fpsPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsPanel1.Location = new System.Drawing.Point(227, 94);
            this.fpsPanel1.Name = "fpsPanel1";
            this.fpsPanel1.Size = new System.Drawing.Size(34, 26);
            this.fpsPanel1.TabIndex = 29;
            // 
            // fpsInput1
            // 
            this.fpsInput1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.fpsInput1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fpsInput1.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsInput1.Location = new System.Drawing.Point(-4, 3);
            this.fpsInput1.MaxLength = 3;
            this.fpsInput1.Name = "fpsInput1";
            this.fpsInput1.Size = new System.Drawing.Size(34, 20);
            this.fpsInput1.TabIndex = 14;
            this.fpsInput1.Tag = "fpscap1";
            this.fpsInput1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // fpsPanel2
            // 
            this.fpsPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.fpsPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsPanel2.Controls.Add(this.fpsInput2);
            this.fpsPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsPanel2.Location = new System.Drawing.Point(227, 123);
            this.fpsPanel2.Name = "fpsPanel2";
            this.fpsPanel2.Size = new System.Drawing.Size(34, 26);
            this.fpsPanel2.TabIndex = 29;
            // 
            // fpsInput2
            // 
            this.fpsInput2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.fpsInput2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fpsInput2.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsInput2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.fpsInput2.Location = new System.Drawing.Point(-4, 3);
            this.fpsInput2.MaxLength = 3;
            this.fpsInput2.Name = "fpsInput2";
            this.fpsInput2.Size = new System.Drawing.Size(34, 20);
            this.fpsInput2.TabIndex = 14;
            this.fpsInput2.Tag = "fpscap2";
            this.fpsInput2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // fpsPanel0
            // 
            this.fpsPanel0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.fpsPanel0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fpsPanel0.Controls.Add(this.fpsInput0);
            this.fpsPanel0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fpsPanel0.Location = new System.Drawing.Point(227, 65);
            this.fpsPanel0.Name = "fpsPanel0";
            this.fpsPanel0.Size = new System.Drawing.Size(34, 26);
            this.fpsPanel0.TabIndex = 28;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.versionDropDownSelector);
            this.panel1.Controls.Add(this.versionChangedLabel);
            this.panel1.Controls.Add(this.changeVersionButton);
            this.panel1.Controls.Add(this.refreshVersionsButton);
            this.panel1.Location = new System.Drawing.Point(12, 480);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 94);
            this.panel1.TabIndex = 30;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.enableHotkeysCheckbox);
            this.panel2.Controls.Add(this.autorunMacroCheckbox);
            this.panel2.Controls.Add(this.firewallToggleButton);
            this.panel2.Controls.Add(this.meathookRestartLabel);
            this.panel2.Controls.Add(this.firewallRestartLabel);
            this.panel2.Controls.Add(this.meathookToggleButton);
            this.panel2.Controls.Add(this.defaultFPSLabel);
            this.panel2.Location = new System.Drawing.Point(12, 297);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(304, 134);
            this.panel2.TabIndex = 31;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.defaultFPS);
            this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.panel3.Location = new System.Drawing.Point(264, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(34, 26);
            this.panel3.TabIndex = 29;
            // 
            // defaultFPS
            // 
            this.defaultFPS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.defaultFPS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.defaultFPS.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.defaultFPS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.defaultFPS.Location = new System.Drawing.Point(-4, 3);
            this.defaultFPS.MaxLength = 3;
            this.defaultFPS.Name = "defaultFPS";
            this.defaultFPS.Size = new System.Drawing.Size(34, 20);
            this.defaultFPS.TabIndex = 14;
            this.defaultFPS.Tag = "fpscapDefault";
            this.defaultFPS.Text = "250";
            this.defaultFPS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.defaultFPS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FPSInput_KeyPressNumericOnly);
            this.defaultFPS.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FPSInput_KeyUp);
            // 
            // titleSeparator
            // 
            this.titleSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.titleSeparator.Location = new System.Drawing.Point(33, 45);
            this.titleSeparator.Name = "titleSeparator";
            this.titleSeparator.Size = new System.Drawing.Size(570, 2);
            this.titleSeparator.TabIndex = 31;
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.exitButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Eternal UI 2", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.exitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.exitButton.Location = new System.Drawing.Point(482, 720);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(145, 32);
            this.exitButton.TabIndex = 32;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ClientSize = new System.Drawing.Size(639, 764);
            this.ControlBox = false;
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.titleSeparator);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.hotkeyPanel);
            this.Controls.Add(this.optionsTitle);
            this.Controls.Add(this.versionTitle);
            this.Controls.Add(this.windowTitle);
            this.Controls.Add(this.macroStatus);
            this.Controls.Add(this.gameVersion);
            this.Controls.Add(this.hotkeysTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_Closing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyAssignment_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HotkeyAssignment_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainWindow_KeyPreviewKeyDown);
            this.hotkeyPanel.ResumeLayout(false);
            this.hotkeyPanel.PerformLayout();
            this.fpsPanel1.ResumeLayout(false);
            this.fpsPanel1.PerformLayout();
            this.fpsPanel2.ResumeLayout(false);
            this.fpsPanel2.PerformLayout();
            this.fpsPanel0.ResumeLayout(false);
            this.fpsPanel0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label hotkeyField0;
        private Label hotkeyField1;
        private Label macroStatus;
        private Label macroUpKeyLabel;
        private Label macroDownKeyLabel;
        private Label hotkeysTitle;
        private Label gameVersion;
        private Label fpsKey2Label;
        private Label hotkeyField4;
        private Label fpsKey1Label;
        private Label hotkeyField2;
        private Label fpsKey0Label;
        private Label hotkeyField3;
        private TextBox fpsInput0;
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
        private Label defaultFPSLabel;
        private Label windowTitle;
        private Label versionTitle;
        private Label optionsTitle;
        private Button meathookToggleButton;
        private Label meathookRestartLabel;
        private Label firewallRestartLabel;
        private Panel hotkeyPanel;
        private Panel fpsPanel0;
        private Panel fpsPanel2;
        private TextBox fpsInput2;
        private Panel fpsPanel1;
        private TextBox fpsInput1;
        private Panel panel1;
        private Panel panel2;
        private Panel titleSeparator;
        private Button exitButton;
        private Panel panel3;
        private TextBox defaultFPS;
    }
}