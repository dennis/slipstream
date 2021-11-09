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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Lua Scripts");
            this.panel1 = new System.Windows.Forms.Panel();
            this.EventsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.AboutTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ConsoleListView = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ButtonFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.EventsTabPage = new System.Windows.Forms.TabPage();
            this.EventGridView = new System.Windows.Forms.DataGridView();
            this.EventFilterDescriptionLabel = new System.Windows.Forms.Label();
            this.Tree = new System.Windows.Forms.Panel();
            this.InsideView = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveEventsToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestEventsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EndpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenDataDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogMessageUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.EventViewerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EventViewerResendMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EventViewerCopyJsonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyLuaHandlerCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearEventsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.EventsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.EventsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EventGridView)).BeginInit();
            this.Tree.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.EventViewerContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.EventsTabControl);
            this.panel1.Controls.Add(this.Tree);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 519);
            this.panel1.TabIndex = 0;
            // 
            // EventsTabControl
            // 
            this.EventsTabControl.Controls.Add(this.tabPage1);
            this.EventsTabControl.Controls.Add(this.tabPage2);
            this.EventsTabControl.Controls.Add(this.tabPage3);
            this.EventsTabControl.Controls.Add(this.EventsTabPage);
            this.EventsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventsTabControl.Location = new System.Drawing.Point(226, 24);
            this.EventsTabControl.Name = "EventsTabControl";
            this.EventsTabControl.SelectedIndex = 0;
            this.EventsTabControl.Size = new System.Drawing.Size(707, 495);
            this.EventsTabControl.TabIndex = 5;
            this.EventsTabControl.Resize += new System.EventHandler(this.EventsTabControl_Resize);
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
            this.tabPage2.Controls.Add(this.ConsoleListView);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(699, 467);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Console";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ConsoleListView
            // 
            this.ConsoleListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.ConsoleListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConsoleListView.FullRowSelect = true;
            this.ConsoleListView.GridLines = true;
            this.ConsoleListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ConsoleListView.HideSelection = false;
            this.ConsoleListView.Location = new System.Drawing.Point(3, 3);
            this.ConsoleListView.MultiSelect = false;
            this.ConsoleListView.Name = "ConsoleListView";
            this.ConsoleListView.ShowGroups = false;
            this.ConsoleListView.Size = new System.Drawing.Size(693, 461);
            this.ConsoleListView.TabIndex = 0;
            this.ConsoleListView.UseCompatibleStateImageBehavior = false;
            this.ConsoleListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 2000;
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
            // EventsTabPage
            // 
            this.EventsTabPage.Controls.Add(this.EventGridView);
            this.EventsTabPage.Controls.Add(this.EventFilterDescriptionLabel);
            this.EventsTabPage.Location = new System.Drawing.Point(4, 24);
            this.EventsTabPage.Name = "EventsTabPage";
            this.EventsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.EventsTabPage.Size = new System.Drawing.Size(699, 467);
            this.EventsTabPage.TabIndex = 3;
            this.EventsTabPage.Text = "Events";
            this.EventsTabPage.UseVisualStyleBackColor = true;
            // 
            // EventGridView
            // 
            this.EventGridView.AllowUserToAddRows = false;
            this.EventGridView.AllowUserToDeleteRows = false;
            this.EventGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EventGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventGridView.Location = new System.Drawing.Point(3, 18);
            this.EventGridView.Name = "EventGridView";
            this.EventGridView.ReadOnly = true;
            this.EventGridView.RowTemplate.Height = 25;
            this.EventGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.EventGridView.Size = new System.Drawing.Size(693, 446);
            this.EventGridView.TabIndex = 1;
            this.EventGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.EventGridView_CellMouseEnter);
            // 
            // EventFilterDescriptionLabel
            // 
            this.EventFilterDescriptionLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EventFilterDescriptionLabel.Location = new System.Drawing.Point(3, 3);
            this.EventFilterDescriptionLabel.Name = "EventFilterDescriptionLabel";
            this.EventFilterDescriptionLabel.Size = new System.Drawing.Size(693, 15);
            this.EventFilterDescriptionLabel.TabIndex = 0;
            this.EventFilterDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            treeNode1.Name = "LuaScripts";
            treeNode1.Text = "Lua Scripts";
            this.InsideView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.InsideView.Size = new System.Drawing.Size(226, 354);
            this.InsideView.TabIndex = 1;
            this.InsideView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.InsideView_AfterSelect);
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
            this.ToolsMenuItem,
            this.EndpointsToolStripMenuItem,
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
            // EndpointsToolStripMenuItem
            // 
            this.EndpointsToolStripMenuItem.Enabled = false;
            this.EndpointsToolStripMenuItem.Name = "EndpointsToolStripMenuItem";
            this.EndpointsToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.EndpointsToolStripMenuItem.Text = "&Endpoints";
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
            // EventViewerContextMenuStrip
            // 
            this.EventViewerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EventViewerResendMenuItem,
            this.EventViewerCopyJsonMenuItem,
            this.CopyLuaHandlerCodeMenuItem,
            this.ClearEventsMenuItem});
            this.EventViewerContextMenuStrip.Name = "EventViewerContextMenuStrip";
            this.EventViewerContextMenuStrip.Size = new System.Drawing.Size(201, 92);
            // 
            // EventViewerResendMenuItem
            // 
            this.EventViewerResendMenuItem.Name = "EventViewerResendMenuItem";
            this.EventViewerResendMenuItem.Size = new System.Drawing.Size(200, 22);
            this.EventViewerResendMenuItem.Text = "Resend event";
            this.EventViewerResendMenuItem.Click += new System.EventHandler(this.EventViewerResendMenuItem_Click);
            // 
            // EventViewerCopyJsonMenuItem
            // 
            this.EventViewerCopyJsonMenuItem.Name = "EventViewerCopyJsonMenuItem";
            this.EventViewerCopyJsonMenuItem.Size = new System.Drawing.Size(200, 22);
            this.EventViewerCopyJsonMenuItem.Text = "Copy JSON";
            this.EventViewerCopyJsonMenuItem.Click += new System.EventHandler(this.EventViewerCopyJsonMenuItem_Click);
            // 
            // CopyLuaHandlerCodeMenuItem
            // 
            this.CopyLuaHandlerCodeMenuItem.Name = "CopyLuaHandlerCodeMenuItem";
            this.CopyLuaHandlerCodeMenuItem.Size = new System.Drawing.Size(200, 22);
            this.CopyLuaHandlerCodeMenuItem.Text = "Copy Lua Handler Code";
            this.CopyLuaHandlerCodeMenuItem.Click += new System.EventHandler(this.CopyLuaHandlerCodeMenuItem_Click);
            // 
            // ClearEventsMenuItem
            // 
            this.ClearEventsMenuItem.Name = "ClearEventsMenuItem";
            this.ClearEventsMenuItem.Size = new System.Drawing.Size(200, 22);
            this.ClearEventsMenuItem.Text = "Clear all events";
            this.ClearEventsMenuItem.Click += new System.EventHandler(this.ClearEventsMenuItem_Click);
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
            this.EventsTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.EventsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EventGridView)).EndInit();
            this.Tree.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.EventViewerContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer LogMessageUpdateTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
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
        private System.Windows.Forms.TabControl EventsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FlowLayoutPanel ButtonFlowLayoutPanel;
        private System.Windows.Forms.RichTextBox AboutTextBox;
        private System.Windows.Forms.TabPage EventsTabPage;
        private System.Windows.Forms.Label EventFilterDescriptionLabel;
        private System.Windows.Forms.DataGridView EventGridView;
        private System.Windows.Forms.ContextMenuStrip EventViewerContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem EventViewerResendMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EventViewerCopyJsonMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyLuaHandlerCodeMenuItem;
        private System.Windows.Forms.ListView ConsoleListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem EndpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearEventsMenuItem;
    }
}

