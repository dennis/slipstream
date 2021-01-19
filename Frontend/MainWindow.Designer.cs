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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveEventsToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenDataDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.LogAreaTextBox = new System.Windows.Forms.TextBox();
            this.LogMessageUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Controls.Add(this.ButtonFlowLayoutPanel);
            this.panel1.Controls.Add(this.LogAreaTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
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
            this.LoadEventsToolStripMenuItem,
            this.SaveEventsToFileToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "&File";
            // 
            // LoadEventsToolStripMenuItem
            // 
            this.LoadEventsToolStripMenuItem.Name = "LoadEventsToolStripMenuItem";
            this.LoadEventsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.LoadEventsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.LoadEventsToolStripMenuItem.Text = "Load events";
            this.LoadEventsToolStripMenuItem.Visible = false;
            this.LoadEventsToolStripMenuItem.Click += new System.EventHandler(this.LoadEventsToolStripMenuItem_Click);
            // 
            // SaveEventsToFileToolStripMenuItem
            // 
            this.SaveEventsToFileToolStripMenuItem.Name = "SaveEventsToFileToolStripMenuItem";
            this.SaveEventsToFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveEventsToFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SaveEventsToFileToolStripMenuItem.Text = "Save events";
            this.SaveEventsToFileToolStripMenuItem.Visible = false;
            this.SaveEventsToFileToolStripMenuItem.Click += new System.EventHandler(this.SaveEventsToFileToolStripMenuItem_Click);
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
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDataDirectoryToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpToolStripMenuItem.Text = "Help";
            // 
            // OpenDataDirectoryToolStripMenuItem
            // 
            this.OpenDataDirectoryToolStripMenuItem.Name = "OpenDataDirectoryToolStripMenuItem";
            this.OpenDataDirectoryToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.OpenDataDirectoryToolStripMenuItem.Text = "Open Data Directory";
            this.OpenDataDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenDataDirectoryToolStripMenuItem_Click);
            // 
            // ButtonFlowLayoutPanel
            // 
            this.ButtonFlowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFlowLayoutPanel.AutoScroll = true;
            this.ButtonFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ButtonFlowLayoutPanel.Location = new System.Drawing.Point(3, 418);
            this.ButtonFlowLayoutPanel.Name = "ButtonFlowLayoutPanel";
            this.ButtonFlowLayoutPanel.Size = new System.Drawing.Size(794, 29);
            this.ButtonFlowLayoutPanel.TabIndex = 3;
            // 
            // LogAreaTextBox
            // 
            this.LogAreaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogAreaTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogAreaTextBox.Location = new System.Drawing.Point(0, 27);
            this.LogAreaTextBox.Multiline = true;
            this.LogAreaTextBox.Name = "LogAreaTextBox";
            this.LogAreaTextBox.ReadOnly = true;
            this.LogAreaTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogAreaTextBox.Size = new System.Drawing.Size(797, 385);
            this.LogAreaTextBox.TabIndex = 2;
            this.LogAreaTextBox.WordWrap = false;
            // 
            // LogMessageUpdateTimer
            // 
            this.LogMessageUpdateTimer.Enabled = true;
            this.LogMessageUpdateTimer.Interval = 250;
            this.LogMessageUpdateTimer.Tick += new System.EventHandler(this.LogMessageUpdate_Tick);
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.DefaultExt = "mjson";
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.DefaultExt = "mjson";
            this.OpenFileDialog.Filter = "Event files|*.mjson|All files|*.*";
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
        private System.Windows.Forms.Timer LogMessageUpdateTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenDataDirectoryToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel ButtonFlowLayoutPanel;
        private System.Windows.Forms.TextBox LogAreaTextBox;
        private System.Windows.Forms.ToolStripMenuItem SaveEventsToFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.ToolStripMenuItem LoadEventsToolStripMenuItem;
    }
}

