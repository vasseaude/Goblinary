namespace Goblinary.WikiData
{
	partial class WikiDataForm
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
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.mainMenu_Data = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu_Data_Import = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu_Data_Read = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mainMenu_Data_ImportAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu_Data_ReadAll = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu_Model = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenu_Model_Process = new System.Windows.Forms.ToolStripMenuItem();
			this.grid = new System.Windows.Forms.DataGridView();
			this.dataTableSelector = new System.Windows.Forms.ComboBox();
			this.dataSetSelector = new System.Windows.Forms.ComboBox();
			this.mainMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenu_Data,
            this.mainMenu_Model});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(1175, 24);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "Main Menu";
			// 
			// mainMenu_Data
			// 
			this.mainMenu_Data.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenu_Data_Import,
            this.mainMenu_Data_Read,
            this.toolStripMenuItem5,
            this.mainMenu_Data_ImportAll,
            this.mainMenu_Data_ReadAll});
			this.mainMenu_Data.Name = "mainMenu_Data";
			this.mainMenu_Data.Size = new System.Drawing.Size(43, 20);
			this.mainMenu_Data.Text = "Data";
			// 
			// mainMenu_Data_Import
			// 
			this.mainMenu_Data_Import.Name = "mainMenu_Data_Import";
			this.mainMenu_Data_Import.Size = new System.Drawing.Size(127, 22);
			this.mainMenu_Data_Import.Text = "Import";
			this.mainMenu_Data_Import.Click += new System.EventHandler(this.mainMenu_Data_Import_Click);
			// 
			// mainMenu_Data_Read
			// 
			this.mainMenu_Data_Read.Name = "mainMenu_Data_Read";
			this.mainMenu_Data_Read.Size = new System.Drawing.Size(127, 22);
			this.mainMenu_Data_Read.Text = "Read";
			this.mainMenu_Data_Read.Click += new System.EventHandler(this.mainMenu_Data_Read_Click);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(124, 6);
			// 
			// mainMenu_Data_ImportAll
			// 
			this.mainMenu_Data_ImportAll.Name = "mainMenu_Data_ImportAll";
			this.mainMenu_Data_ImportAll.Size = new System.Drawing.Size(127, 22);
			this.mainMenu_Data_ImportAll.Text = "Import All";
			this.mainMenu_Data_ImportAll.Click += new System.EventHandler(this.mainMenu_Data_ImportAll_Click);
			// 
			// mainMenu_Data_ReadAll
			// 
			this.mainMenu_Data_ReadAll.Name = "mainMenu_Data_ReadAll";
			this.mainMenu_Data_ReadAll.Size = new System.Drawing.Size(127, 22);
			this.mainMenu_Data_ReadAll.Text = "Read All";
			this.mainMenu_Data_ReadAll.Click += new System.EventHandler(this.mainMenu_Data_ReadAll_Click);
			// 
			// mainMenu_Model
			// 
			this.mainMenu_Model.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenu_Model_Process});
			this.mainMenu_Model.Name = "mainMenu_Model";
			this.mainMenu_Model.Size = new System.Drawing.Size(53, 20);
			this.mainMenu_Model.Text = "Model";
			// 
			// mainMenu_Model_Process
			// 
			this.mainMenu_Model_Process.Name = "mainMenu_Model_Process";
			this.mainMenu_Model_Process.Size = new System.Drawing.Size(152, 22);
			this.mainMenu_Model_Process.Text = "Process";
			this.mainMenu_Model_Process.Click += new System.EventHandler(this.mainMenu_Model_Process_Click);
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Location = new System.Drawing.Point(12, 54);
			this.grid.Name = "grid";
			this.grid.ReadOnly = true;
			this.grid.Size = new System.Drawing.Size(1151, 667);
			this.grid.TabIndex = 11;
			// 
			// dataTableSelector
			// 
			this.dataTableSelector.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.dataTableSelector.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.dataTableSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dataTableSelector.FormattingEnabled = true;
			this.dataTableSelector.Location = new System.Drawing.Point(320, 27);
			this.dataTableSelector.Name = "dataTableSelector";
			this.dataTableSelector.Size = new System.Drawing.Size(302, 21);
			this.dataTableSelector.TabIndex = 10;
			this.dataTableSelector.SelectedIndexChanged += new System.EventHandler(this.dataTableSelector_SelectedIndexChanged);
			// 
			// dataSetSelector
			// 
			this.dataSetSelector.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.dataSetSelector.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.dataSetSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dataSetSelector.FormattingEnabled = true;
			this.dataSetSelector.Location = new System.Drawing.Point(12, 27);
			this.dataSetSelector.Name = "dataSetSelector";
			this.dataSetSelector.Size = new System.Drawing.Size(302, 21);
			this.dataSetSelector.TabIndex = 14;
			this.dataSetSelector.SelectedIndexChanged += new System.EventHandler(this.dataSetSelector_SelectedIndexChanged);
			// 
			// PFOWikiDataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1175, 733);
			this.Controls.Add(this.dataSetSelector);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.dataTableSelector);
			this.Controls.Add(this.mainMenu);
			this.MainMenuStrip = this.mainMenu;
			this.Name = "PFOWikiDataForm";
			this.Text = "PFO Wiki Data";
			this.Load += new System.EventHandler(this.PFOWikiDataForm_Load);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Data;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Data_Import;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Data_Read;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Model;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Model_Process;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Data_ImportAll;
		private System.Windows.Forms.ToolStripMenuItem mainMenu_Data_ReadAll;
		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.ComboBox dataTableSelector;
		private System.Windows.Forms.ComboBox dataSetSelector;
	}
}

