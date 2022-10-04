namespace DESpeedrunUtil {
    partial class FPSLimitWarning {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPSLimitWarning));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.disableWarningCheckbox = new System.Windows.Forms.CheckBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 24);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(496, 183);
            this.textBox1.TabIndex = 99;
            this.textBox1.TabStop = false;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // disableWarningCheckbox
            // 
            this.disableWarningCheckbox.AutoSize = true;
            this.disableWarningCheckbox.Location = new System.Drawing.Point(12, 242);
            this.disableWarningCheckbox.Name = "disableWarningCheckbox";
            this.disableWarningCheckbox.Size = new System.Drawing.Size(193, 19);
            this.disableWarningCheckbox.TabIndex = 100;
            this.disableWarningCheckbox.Text = "Don\'t Show This Warning Again";
            this.disableWarningCheckbox.UseVisualStyleBackColor = true;
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Continue;
            this.closeButton.Location = new System.Drawing.Point(365, 238);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(143, 24);
            this.closeButton.TabIndex = 101;
            this.closeButton.Text = "I Understand";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.button1.Location = new System.Drawing.Point(216, 238);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 24);
            this.button1.TabIndex = 102;
            this.button1.Text = "Disable Max FPS Limiter";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FPSLimitWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 273);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.disableWarningCheckbox);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPSLimitWarning";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Known Issue with FPS Limiter";
            this.Load += new System.EventHandler(this.FPSLimitWarning_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBox1;
        internal CheckBox disableWarningCheckbox;
        private Button closeButton;
        private Button button1;
    }
}