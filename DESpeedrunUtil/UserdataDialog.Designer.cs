namespace DESpeedrunUtil {
    partial class UserdataDialog {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserdataDialog));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.profileSelector = new System.Windows.Forms.ComboBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.confirmButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(404, 80);
            this.textBox1.TabIndex = 20;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(228, 57);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(62, 15);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "steamid.io";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // profileSelector
            // 
            this.profileSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.profileSelector.FormattingEnabled = true;
            this.profileSelector.Location = new System.Drawing.Point(12, 98);
            this.profileSelector.Name = "profileSelector";
            this.profileSelector.Size = new System.Drawing.Size(404, 23);
            this.profileSelector.Sorted = true;
            this.profileSelector.TabIndex = 5;
            this.profileSelector.TabStop = false;
            this.profileSelector.SelectedIndexChanged += new System.EventHandler(this.profileSelector_SelectedIndexChanged);
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(301, 127);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(115, 23);
            this.exitButton.TabIndex = 6;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // confirmButton
            // 
            this.confirmButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmButton.Location = new System.Drawing.Point(180, 127);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(115, 23);
            this.confirmButton.TabIndex = 7;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            // 
            // UserdataDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 154);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.profileSelector);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserdataDialog";
            this.Text = "Multiple Steam Profiles Found";
            this.Load += new System.EventHandler(this.UserdataDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        private LinkLabel linkLabel1;
        private ComboBox profileSelector;
        private Button exitButton;
        private Button confirmButton;
    }
}