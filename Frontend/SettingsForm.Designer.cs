namespace Slipstream.Frontend
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.TwitchTokenGeneratorLabel = new System.Windows.Forms.LinkLabel();
            this.TwitchChannelTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TwitchUsernameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TwitchTokenTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(677, 108);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TwitchTokenTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DiscardButton);
            this.groupBox1.Controls.Add(this.ApplyButton);
            this.groupBox1.Controls.Add(this.TwitchTokenGeneratorLabel);
            this.groupBox1.Controls.Add(this.TwitchChannelTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TwitchUsernameTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(667, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Twitch";
            // 
            // DiscardButton
            // 
            this.DiscardButton.Location = new System.Drawing.Point(505, 71);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 5;
            this.DiscardButton.Text = "Cancel";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(586, 70);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyButton.TabIndex = 6;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // TwitchTokenGeneratorLabel
            // 
            this.TwitchTokenGeneratorLabel.AutoSize = true;
            this.TwitchTokenGeneratorLabel.Location = new System.Drawing.Point(322, 77);
            this.TwitchTokenGeneratorLabel.Name = "TwitchTokenGeneratorLabel";
            this.TwitchTokenGeneratorLabel.Size = new System.Drawing.Size(123, 13);
            this.TwitchTokenGeneratorLabel.TabIndex = 4;
            this.TwitchTokenGeneratorLabel.TabStop = true;
            this.TwitchTokenGeneratorLabel.Text = "Twitch Token Generator";
            this.TwitchTokenGeneratorLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.TwitchTokenGeneratorLabel_LinkClicked);
            // 
            // TwitchChannelTextBox
            // 
            this.TwitchChannelTextBox.Location = new System.Drawing.Point(105, 48);
            this.TwitchChannelTextBox.Name = "TwitchChannelTextBox";
            this.TwitchChannelTextBox.Size = new System.Drawing.Size(211, 20);
            this.TwitchChannelTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Twitch Channel";
            // 
            // TwitchUsernameTextBox
            // 
            this.TwitchUsernameTextBox.Location = new System.Drawing.Point(105, 19);
            this.TwitchUsernameTextBox.Name = "TwitchUsernameTextBox";
            this.TwitchUsernameTextBox.Size = new System.Drawing.Size(211, 20);
            this.TwitchUsernameTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Twitch Username";
            // 
            // TwitchTokenTextBox
            // 
            this.TwitchTokenTextBox.Location = new System.Drawing.Point(105, 74);
            this.TwitchTokenTextBox.Name = "TwitchTokenTextBox";
            this.TwitchTokenTextBox.Size = new System.Drawing.Size(211, 20);
            this.TwitchTokenTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Twitch Token";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 108);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TwitchUsernameTextBox;
        private System.Windows.Forms.LinkLabel TwitchTokenGeneratorLabel;
        private System.Windows.Forms.TextBox TwitchChannelTextBox;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.TextBox TwitchTokenTextBox;
        private System.Windows.Forms.Label label3;
    }
}