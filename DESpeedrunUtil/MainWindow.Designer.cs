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
            this.macroStatusText = new System.Windows.Forms.TextBox();
            this.hotkeyTextBox = new System.Windows.Forms.TextBox();
            this.hotkeyField1 = new System.Windows.Forms.Label();
            this.hotkeyField2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // macroStatusText
            // 
            this.macroStatusText.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.macroStatusText.Location = new System.Drawing.Point(160, 118);
            this.macroStatusText.Name = "macroStatusText";
            this.macroStatusText.ReadOnly = true;
            this.macroStatusText.Size = new System.Drawing.Size(177, 32);
            this.macroStatusText.TabIndex = 0;
            this.macroStatusText.TabStop = false;
            this.macroStatusText.Text = "Macro Not Running";
            // 
            // hotkeyTextBox
            // 
            this.hotkeyTextBox.Location = new System.Drawing.Point(267, 255);
            this.hotkeyTextBox.Name = "hotkeyTextBox";
            this.hotkeyTextBox.ReadOnly = true;
            this.hotkeyTextBox.Size = new System.Drawing.Size(100, 23);
            this.hotkeyTextBox.TabIndex = 1;
            this.hotkeyTextBox.TabStop = false;
            // 
            // hotkeyField1
            // 
            this.hotkeyField1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField1.Location = new System.Drawing.Point(688, 9);
            this.hotkeyField1.Name = "hotkeyField1";
            this.hotkeyField1.Size = new System.Drawing.Size(100, 19);
            this.hotkeyField1.TabIndex = 2;
            this.hotkeyField1.Tag = "macroDown";
            // 
            // hotkeyField2
            // 
            this.hotkeyField2.BackColor = System.Drawing.SystemColors.Control;
            this.hotkeyField2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hotkeyField2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.hotkeyField2.Location = new System.Drawing.Point(688, 37);
            this.hotkeyField2.Name = "hotkeyField2";
            this.hotkeyField2.Size = new System.Drawing.Size(100, 19);
            this.hotkeyField2.TabIndex = 3;
            this.hotkeyField2.Tag = "macroUp";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.hotkeyField2);
            this.Controls.Add(this.hotkeyField1);
            this.Controls.Add(this.hotkeyTextBox);
            this.Controls.Add(this.macroStatusText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox macroStatusText;
        private TextBox hotkeyTextBox;
        private Label hotkeyField1;
        private Label hotkeyField2;
    }
}