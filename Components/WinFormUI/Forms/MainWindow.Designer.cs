namespace Slipstream.Components.WinFormUI.Forms
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.AboutTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LogAreaTextBox = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ButtonFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Tree = new System.Windows.Forms.Panel();
            this.InsideView = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveEventsToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestEventsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenDataDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogMessageUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.Tree.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.Tree);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 519);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(226, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(707, 495);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.AboutTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(699, 467);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "About";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // AboutTextBox
            // 
            this.AboutTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AboutTextBox.Location = new System.Drawing.Point(3, 3);
            this.AboutTextBox.Name = "AboutTextBox";
            this.AboutTextBox.Size = new System.Drawing.Size(693, 461);
            this.AboutTextBox.TabIndex = 0;
            this.AboutTextBox.Text = "Slipstream v0.8.0\n\nBy adfadf\n\n\nasdfasdfdsfsdf";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.LogAreaTextBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(699, 467);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LogAreaTextBox
            // 
            this.LogAreaTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogAreaTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LogAreaTextBox.Location = new System.Drawing.Point(3, 3);
            this.LogAreaTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LogAreaTextBox.Multiline = true;
            this.LogAreaTextBox.Name = "LogAreaTextBox";
            this.LogAreaTextBox.ReadOnly = true;
            this.LogAreaTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogAreaTextBox.Size = new System.Drawing.Size(693, 461);
            this.LogAreaTextBox.TabIndex = 3;
            this.LogAreaTextBox.WordWrap = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ButtonFlowLayoutPanel);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(699, 467);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Buttons";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ButtonFlowLayoutPanel
            // 
            this.ButtonFlowLayoutPanel.AutoScroll = true;
            this.ButtonFlowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ButtonFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonFlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ButtonFlowLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ButtonFlowLayoutPanel.Name = "ButtonFlowLayoutPanel";
            this.ButtonFlowLayoutPanel.Size = new System.Drawing.Size(693, 461);
            this.ButtonFlowLayoutPanel.TabIndex = 5;
            // 
            // Tree
            // 
            this.Tree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(91)))), ((int)(((byte)(110)))));
            this.Tree.Controls.Add(this.InsideView);
            this.Tree.Controls.Add(this.panel2);
            this.Tree.Dock = System.Windows.Forms.DockStyle.Left;
            this.Tree.Location = new System.Drawing.Point(0, 24);
            this.Tree.Name = "Tree";
            this.Tree.Size = new System.Drawing.Size(226, 495);
            this.Tree.TabIndex = 4;
            // 
            // InsideView
            // 
            this.InsideView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InsideView.Location = new System.Drawing.Point(0, 141);
            this.InsideView.Name = "InsideView";
            this.InsideView.Size = new System.Drawing.Size(226, 354);
            this.InsideView.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 141);
            this.panel2.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.PluginsToolStripMenuItem,
            this.ToolsMenuItem,
            this.HelpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
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
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestEventsMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsMenuItem.Text = "Tools";
            // 
            // TestEventsMenuItem
            // 
            this.TestEventsMenuItem.Name = "TestEventsMenuItem";
            this.TestEventsMenuItem.Size = new System.Drawing.Size(131, 22);
            this.TestEventsMenuItem.Text = "&Test Events";
            this.TestEventsMenuItem.ToolTipText = "Allows users to send test events to test scripts";
            this.TestEventsMenuItem.Click += new System.EventHandler(this.TestEventsMenuItem_Click);
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
            this.OpenDataDirectoryToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.OpenDataDirectoryToolStripMenuItem.Text = "Open Data Directory";
            this.OpenDataDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenDataDirectoryToolStripMenuItem_Click);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainWindow";
            this.Text = "Slipstream";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.Tree.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem SaveEventsToFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.ToolStripMenuItem LoadEventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TestEventsMenuItem;
        private System.Windows.Forms.Panel Tree;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView InsideView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox LogAreaTextBox;
        private System.Windows.Forms.FlowLayoutPanel ButtonFlowLayoutPanel;
        private System.Windows.Forms.RichTextBox AboutTextBox;
    }
}

