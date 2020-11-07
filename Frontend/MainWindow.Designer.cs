namespace Slipstream.Frontend
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LogAreaTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogMessageUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenScriptsDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LogAreaTextBox);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // LogAreaTextBox
            // 
            this.LogAreaTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogAreaTextBox.Location = new System.Drawing.Point(0, 24);
            this.LogAreaTextBox.Multiline = true;
            this.LogAreaTextBox.Name = "LogAreaTextBox";
            this.LogAreaTextBox.ReadOnly = true;
            this.LogAreaTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogAreaTextBox.Size = new System.Drawing.Size(800, 426);
            this.LogAreaTextBox.TabIndex = 0;
            this.LogAreaTextBox.WordWrap = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.PluginsToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "&File";
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // PluginsToolStripMenuItem
            // 
            this.PluginsToolStripMenuItem.Name = "PluginsToolStripMenuItem";
            this.PluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.PluginsToolStripMenuItem.Text = "&Plugins";
            // 
            // LogMessageUpdateTimer
            // 
            this.LogMessageUpdateTimer.Enabled = true;
            this.LogMessageUpdateTimer.Interval = 250;
            this.LogMessageUpdateTimer.Tick += new System.EventHandler(this.LogMessageUpdate_Tick);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenScriptsDirectoryToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpToolStripMenuItem.Text = "Help";
            // 
            // OpenScriptsDirectoryToolStripMenuItem
            // 
            this.OpenScriptsDirectoryToolStripMenuItem.Name = "OpenScriptsDirectoryToolStripMenuItem";
            this.OpenScriptsDirectoryToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.OpenScriptsDirectoryToolStripMenuItem.Text = "Open Scripts Directory";
            this.OpenScriptsDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenScriptsDirectoryToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "Slipstream";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox LogAreaTextBox;
        private System.Windows.Forms.Timer LogMessageUpdateTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenScriptsDirectoryToolStripMenuItem;
    }
}

