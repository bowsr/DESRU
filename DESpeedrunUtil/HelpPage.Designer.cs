namespace DESpeedrunUtil {
    partial class HelpPage {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpPage));
            closeButton = new Button();
            pictureBox1 = new PictureBox();
            helpTextbox = new TextBox();
            helpTabButton0 = new Button();
            helpTabButton1 = new Button();
            helpTabButton2 = new Button();
            helpTabButton3 = new Button();
            helpTabButton4 = new Button();
            helpTabButton5 = new Button();
            helpTabButton6 = new Button();
            helpPageVersionImage = new PictureBox();
            helpPageOSDImage = new PictureBox();
            blankPanel = new Panel();
            titleText = new TextBox();
            helpTabButton7 = new Button();
            ((System.ComponentModel.ISupportInitialize) pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize) helpPageVersionImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize) helpPageOSDImage).BeginInit();
            blankPanel.SuspendLayout();
            SuspendLayout();
            // 
            // closeButton
            // 
            closeButton.BackColor = Color.FromArgb(  70,   70,   70);
            closeButton.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            closeButton.ForeColor = Color.FromArgb(  230,   230,   230);
            closeButton.Location = new Point(650, 721);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(122, 28);
            closeButton.TabIndex = 8;
            closeButton.Text = "Close";
            closeButton.UseVisualStyleBackColor = false;
            closeButton.Click += CloseButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image) resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(64, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 100);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // helpTextbox
            // 
            helpTextbox.BackColor = Color.FromArgb(  45,   45,   45);
            helpTextbox.BorderStyle = BorderStyle.None;
            helpTextbox.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            helpTextbox.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTextbox.Location = new Point(3, 3);
            helpTextbox.Multiline = true;
            helpTextbox.Name = "helpTextbox";
            helpTextbox.ReadOnly = true;
            helpTextbox.Size = new Size(542, 582);
            helpTextbox.TabIndex = 0;
            helpTextbox.TabStop = false;
            // 
            // helpTabButton0
            // 
            helpTabButton0.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton0.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton0.FlatStyle = FlatStyle.Flat;
            helpTabButton0.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton0.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton0.Location = new Point(12, 125);
            helpTabButton0.Name = "helpTabButton0";
            helpTabButton0.Size = new Size(204, 33);
            helpTabButton0.TabIndex = 11;
            helpTabButton0.Tag = "macro";
            helpTabButton0.Text = "Freescroll Macro";
            helpTabButton0.UseVisualStyleBackColor = false;
            helpTabButton0.Click += TabButton_Click;
            // 
            // helpTabButton1
            // 
            helpTabButton1.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton1.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton1.FlatStyle = FlatStyle.Flat;
            helpTabButton1.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton1.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton1.Location = new Point(12, 164);
            helpTabButton1.Name = "helpTabButton1";
            helpTabButton1.Size = new Size(204, 33);
            helpTabButton1.TabIndex = 12;
            helpTabButton1.Tag = "hk";
            helpTabButton1.Text = "Keybinds";
            helpTabButton1.UseVisualStyleBackColor = false;
            helpTabButton1.Click += TabButton_Click;
            // 
            // helpTabButton2
            // 
            helpTabButton2.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton2.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton2.FlatStyle = FlatStyle.Flat;
            helpTabButton2.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton2.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton2.Location = new Point(12, 203);
            helpTabButton2.Name = "helpTabButton2";
            helpTabButton2.Size = new Size(204, 33);
            helpTabButton2.TabIndex = 13;
            helpTabButton2.Tag = "res";
            helpTabButton2.Text = "Resolution Scaling";
            helpTabButton2.UseVisualStyleBackColor = false;
            helpTabButton2.Click += TabButton_Click;
            // 
            // helpTabButton3
            // 
            helpTabButton3.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton3.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton3.FlatStyle = FlatStyle.Flat;
            helpTabButton3.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton3.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton3.Location = new Point(12, 242);
            helpTabButton3.Name = "helpTabButton3";
            helpTabButton3.Size = new Size(204, 33);
            helpTabButton3.TabIndex = 14;
            helpTabButton3.Tag = "ver";
            helpTabButton3.Text = "Version Changer";
            helpTabButton3.UseVisualStyleBackColor = false;
            helpTabButton3.Click += TabButton_Click;
            // 
            // helpTabButton4
            // 
            helpTabButton4.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton4.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton4.FlatStyle = FlatStyle.Flat;
            helpTabButton4.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton4.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton4.Location = new Point(12, 281);
            helpTabButton4.Name = "helpTabButton4";
            helpTabButton4.Size = new Size(204, 33);
            helpTabButton4.TabIndex = 15;
            helpTabButton4.Tag = "option";
            helpTabButton4.Text = "Options";
            helpTabButton4.UseVisualStyleBackColor = false;
            helpTabButton4.Click += TabButton_Click;
            // 
            // helpTabButton5
            // 
            helpTabButton5.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton5.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton5.FlatStyle = FlatStyle.Flat;
            helpTabButton5.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton5.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton5.Location = new Point(12, 359);
            helpTabButton5.Name = "helpTabButton5";
            helpTabButton5.Size = new Size(204, 33);
            helpTabButton5.TabIndex = 16;
            helpTabButton5.Tag = "info";
            helpTabButton5.Text = "Info Panel";
            helpTabButton5.UseVisualStyleBackColor = false;
            helpTabButton5.Click += TabButton_Click;
            // 
            // helpTabButton6
            // 
            helpTabButton6.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton6.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton6.FlatStyle = FlatStyle.Flat;
            helpTabButton6.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton6.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton6.Location = new Point(12, 398);
            helpTabButton6.Name = "helpTabButton6";
            helpTabButton6.Size = new Size(204, 33);
            helpTabButton6.TabIndex = 17;
            helpTabButton6.Tag = "osd";
            helpTabButton6.Text = "On Screen Display";
            helpTabButton6.UseVisualStyleBackColor = false;
            helpTabButton6.Click += TabButton_Click;
            // 
            // helpPageVersionImage
            // 
            helpPageVersionImage.Image = (Image) resources.GetObject("helpPageVersionImage.Image");
            helpPageVersionImage.Location = new Point(372, 331);
            helpPageVersionImage.Name = "helpPageVersionImage";
            helpPageVersionImage.Size = new Size(173, 154);
            helpPageVersionImage.TabIndex = 1;
            helpPageVersionImage.TabStop = false;
            helpPageVersionImage.Visible = false;
            // 
            // helpPageOSDImage
            // 
            helpPageOSDImage.Image = (Image) resources.GetObject("helpPageOSDImage.Image");
            helpPageOSDImage.Location = new Point(348, 168);
            helpPageOSDImage.Name = "helpPageOSDImage";
            helpPageOSDImage.Size = new Size(199, 83);
            helpPageOSDImage.TabIndex = 1;
            helpPageOSDImage.TabStop = false;
            helpPageOSDImage.Visible = false;
            // 
            // blankPanel
            // 
            blankPanel.BackColor = Color.FromArgb(  45,   45,   45);
            blankPanel.BorderStyle = BorderStyle.FixedSingle;
            blankPanel.Controls.Add(helpPageOSDImage);
            blankPanel.Controls.Add(helpPageVersionImage);
            blankPanel.Controls.Add(helpTextbox);
            blankPanel.Location = new Point(222, 125);
            blankPanel.Name = "blankPanel";
            blankPanel.Size = new Size(550, 590);
            blankPanel.TabIndex = 11;
            blankPanel.Tag = "blank";
            // 
            // titleText
            // 
            titleText.BackColor = Color.FromArgb(  35,   35,   35);
            titleText.BorderStyle = BorderStyle.None;
            titleText.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            titleText.ForeColor = Color.FromArgb(  230,   230,   230);
            titleText.Location = new Point(226, 12);
            titleText.Multiline = true;
            titleText.Name = "titleText";
            titleText.ReadOnly = true;
            titleText.Size = new Size(546, 110);
            titleText.TabIndex = 1;
            titleText.TabStop = false;
            titleText.Text = resources.GetString("titleText.Text");
            titleText.TextAlign = HorizontalAlignment.Center;
            // 
            // helpTabButton7
            // 
            helpTabButton7.BackColor = Color.FromArgb(  70,   70,   70);
            helpTabButton7.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            helpTabButton7.FlatStyle = FlatStyle.Flat;
            helpTabButton7.Font = new Font("Eternal UI 2", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            helpTabButton7.ForeColor = Color.FromArgb(  230,   230,   230);
            helpTabButton7.Location = new Point(12, 320);
            helpTabButton7.Name = "helpTabButton7";
            helpTabButton7.Size = new Size(204, 33);
            helpTabButton7.TabIndex = 18;
            helpTabButton7.Tag = "trainer";
            helpTabButton7.Text = "Trainer";
            helpTabButton7.UseVisualStyleBackColor = false;
            helpTabButton7.Click += TabButton_Click;
            // 
            // HelpPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(  35,   35,   35);
            ClientSize = new Size(784, 761);
            Controls.Add(helpTabButton7);
            Controls.Add(titleText);
            Controls.Add(blankPanel);
            Controls.Add(helpTabButton6);
            Controls.Add(helpTabButton5);
            Controls.Add(helpTabButton4);
            Controls.Add(helpTabButton3);
            Controls.Add(helpTabButton2);
            Controls.Add(helpTabButton1);
            Controls.Add(helpTabButton0);
            Controls.Add(pictureBox1);
            Controls.Add(closeButton);
            Icon = (Icon) resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HelpPage";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Help and Instructions";
            FormClosing += HelpPage_FormClosing;
            Load += HelpPage_Load;
            ((System.ComponentModel.ISupportInitialize) pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize) helpPageVersionImage).EndInit();
            ((System.ComponentModel.ISupportInitialize) helpPageOSDImage).EndInit();
            blankPanel.ResumeLayout(false);
            blankPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button closeButton;
        private PictureBox pictureBox1;
        private Button helpTabButton0;
        private Button helpTabButton1;
        private Button helpTabButton2;
        private Button helpTabButton3;
        private Button helpTabButton4;
        private Button helpTabButton5;
        private TextBox helpTextbox;
        private Button helpTabButton6;
        private PictureBox helpPageVersionImage;
        private PictureBox helpPageOSDImage;
        private Panel blankPanel;
        private TextBox titleText;
        private Button helpTabButton7;
    }
}