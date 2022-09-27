namespace Updater {
    partial class ProgressWindow {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressWindow));
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.updateProgressBar = new System.Windows.Forms.ProgressBar();
            this.openDESRUButton = new System.Windows.Forms.Button();
            this.progressLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgessChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // updateProgressBar
            // 
            this.updateProgressBar.Location = new System.Drawing.Point(12, 47);
            this.updateProgressBar.Name = "updateProgressBar";
            this.updateProgressBar.Size = new System.Drawing.Size(460, 23);
            this.updateProgressBar.Step = 5;
            this.updateProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.updateProgressBar.TabIndex = 0;
            // 
            // openDESRUButton
            // 
            this.openDESRUButton.Enabled = false;
            this.openDESRUButton.Location = new System.Drawing.Point(366, 76);
            this.openDESRUButton.Name = "openDESRUButton";
            this.openDESRUButton.Size = new System.Drawing.Size(106, 23);
            this.openDESRUButton.TabIndex = 1;
            this.openDESRUButton.Text = "Launch DESRU";
            this.openDESRUButton.UseVisualStyleBackColor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(12, 29);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(38, 15);
            this.progressLabel.TabIndex = 2;
            this.progressLabel.Text = "label1";
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 111);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.openDESRUButton);
            this.Controls.Add(this.updateProgressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "DESRU Updater";
            this.Shown += new System.EventHandler(this.ProgressWindow_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private ProgressBar updateProgressBar;
        private Button openDESRUButton;
        private Label progressLabel;
    }
}