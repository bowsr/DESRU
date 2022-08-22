namespace DESpeedrunUtil {
    partial class GameDirectoryDialog {
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
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.confirmButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.openFolderDialog = new System.Windows.Forms.Button();
            this.errorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(12, 42);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(359, 23);
            this.pathTextBox.TabIndex = 0;
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(302, 90);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(100, 23);
            this.exitButton.TabIndex = 1;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            // 
            // confirmButton
            // 
            this.confirmButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmButton.Enabled = false;
            this.confirmButton.Location = new System.Drawing.Point(196, 90);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(100, 23);
            this.confirmButton.TabIndex = 2;
            this.confirmButton.Text = "Confirm";
            this.confirmButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select your DOOMEternalx64vk.exe";
            // 
            // openFolderDialog
            // 
            this.openFolderDialog.Location = new System.Drawing.Point(377, 42);
            this.openFolderDialog.Name = "openFolderDialog";
            this.openFolderDialog.Size = new System.Drawing.Size(25, 23);
            this.openFolderDialog.TabIndex = 4;
            this.openFolderDialog.Text = "...";
            this.openFolderDialog.UseVisualStyleBackColor = true;
            this.openFolderDialog.Click += new System.EventHandler(this.SelectFolder_Click);
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.errorLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.errorLabel.Location = new System.Drawing.Point(12, 68);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(335, 13);
            this.errorLabel.TabIndex = 5;
            this.errorLabel.Text = "File Path must end with \"DOOMEternal\\DOOMEternalx64vk.exe\"";
            // 
            // GameDirectoryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 125);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.openFolderDialog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.pathTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameDirectoryDialog";
            this.Text = "Unable to locate game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox pathTextBox;
        private Button exitButton;
        private Button confirmButton;
        private Label label1;
        private Button openFolderDialog;
        private Label errorLabel;
    }
}