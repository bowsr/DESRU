namespace DESpeedrunUtil {
    partial class UpdateDialog {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.changelogWebViewer = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.changelogLabel = new System.Windows.Forms.Label();
            this.webViewerPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.changelogWebViewer)).BeginInit();
            this.webViewerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "An update for DESRU has been detected.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "You are required to use the latest version during speedruns.";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(422, 339);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Continue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(266, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(150, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Skip For This Session";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(110, 339);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "Update DESRU";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.versionLabel.Location = new System.Drawing.Point(17, 31);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(35, 17);
            this.versionLabel.TabIndex = 5;
            this.versionLabel.Text = "9.9.9";
            // 
            // changelogWebViewer
            // 
            this.changelogWebViewer.AllowExternalDrop = false;
            this.changelogWebViewer.CreationProperties = null;
            this.changelogWebViewer.DefaultBackgroundColor = System.Drawing.SystemColors.Control;
            this.changelogWebViewer.Location = new System.Drawing.Point(-1, -1);
            this.changelogWebViewer.Name = "changelogWebViewer";
            this.changelogWebViewer.Size = new System.Drawing.Size(485, 232);
            this.changelogWebViewer.TabIndex = 6;
            this.changelogWebViewer.TabStop = false;
            this.changelogWebViewer.ZoomFactor = 0.8D;
            // 
            // changelogLabel
            // 
            this.changelogLabel.AutoSize = true;
            this.changelogLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.changelogLabel.Location = new System.Drawing.Point(12, 81);
            this.changelogLabel.Name = "changelogLabel";
            this.changelogLabel.Size = new System.Drawing.Size(74, 17);
            this.changelogLabel.TabIndex = 7;
            this.changelogLabel.Text = "Changelog:";
            // 
            // webViewerPanel
            // 
            this.webViewerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.webViewerPanel.Controls.Add(this.changelogWebViewer);
            this.webViewerPanel.Location = new System.Drawing.Point(12, 101);
            this.webViewerPanel.Name = "webViewerPanel";
            this.webViewerPanel.Size = new System.Drawing.Size(485, 232);
            this.webViewerPanel.TabIndex = 8;
            // 
            // UpdateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 374);
            this.Controls.Add(this.webViewerPanel);
            this.Controls.Add(this.changelogLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UpdateDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Update Required";
            this.Load += new System.EventHandler(this.UpdateDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.changelogWebViewer)).EndInit();
            this.webViewerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label versionLabel;
        private Microsoft.Web.WebView2.WinForms.WebView2 changelogWebViewer;
        private Label changelogLabel;
        private Panel webViewerPanel;
    }
}