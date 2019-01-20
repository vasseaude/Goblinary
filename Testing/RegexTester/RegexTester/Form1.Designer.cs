namespace RegexTester
{
	partial class Form1
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
			this.patternLabel = new System.Windows.Forms.Label();
			this.patternEditor = new System.Windows.Forms.TextBox();
			this.matchButton = new System.Windows.Forms.Button();
			this.matchesGrid = new System.Windows.Forms.DataGridView();
			this.inputsBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.rootBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.matchesDataSet = new RegexTester.MatchesDataSet();
			this.matchesBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.effectTextDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.isMatchDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.effectNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.descriptorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionalPrefixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionalSuffixDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.remainderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.matchesGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.inputsBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rootBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.matchesDataSet)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.matchesBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// patternLabel
			// 
			this.patternLabel.AutoSize = true;
			this.patternLabel.Location = new System.Drawing.Point(12, 9);
			this.patternLabel.Name = "patternLabel";
			this.patternLabel.Size = new System.Drawing.Size(41, 13);
			this.patternLabel.TabIndex = 0;
			this.patternLabel.Text = "Pattern";
			// 
			// patternEditor
			// 
			this.patternEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.patternEditor.Location = new System.Drawing.Point(12, 25);
			this.patternEditor.Multiline = true;
			this.patternEditor.Name = "patternEditor";
			this.patternEditor.Size = new System.Drawing.Size(1098, 90);
			this.patternEditor.TabIndex = 1;
			// 
			// matchButton
			// 
			this.matchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.matchButton.Location = new System.Drawing.Point(1035, 675);
			this.matchButton.Name = "matchButton";
			this.matchButton.Size = new System.Drawing.Size(75, 23);
			this.matchButton.TabIndex = 7;
			this.matchButton.Text = "Match";
			this.matchButton.UseVisualStyleBackColor = true;
			this.matchButton.Click += new System.EventHandler(this.matchButton_Click);
			// 
			// matchesGrid
			// 
			this.matchesGrid.AllowUserToAddRows = false;
			this.matchesGrid.AllowUserToDeleteRows = false;
			this.matchesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.matchesGrid.AutoGenerateColumns = false;
			this.matchesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.matchesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.effectTextDataGridViewTextBoxColumn,
            this.isMatchDataGridViewTextBoxColumn,
            this.effectNameDataGridViewTextBoxColumn,
            this.descriptorDataGridViewTextBoxColumn,
            this.conditionalPrefixDataGridViewTextBoxColumn,
            this.conditionDataGridViewTextBoxColumn,
            this.conditionalSuffixDataGridViewTextBoxColumn,
            this.remainderDataGridViewTextBoxColumn});
			this.matchesGrid.DataSource = this.inputsBindingSource;
			this.matchesGrid.Location = new System.Drawing.Point(12, 121);
			this.matchesGrid.Name = "matchesGrid";
			this.matchesGrid.RowHeadersVisible = false;
			this.matchesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.matchesGrid.Size = new System.Drawing.Size(1098, 548);
			this.matchesGrid.TabIndex = 6;
			// 
			// inputsBindingSource
			// 
			this.inputsBindingSource.DataMember = "Inputs";
			this.inputsBindingSource.DataSource = this.rootBindingSource;
			// 
			// rootBindingSource
			// 
			this.rootBindingSource.DataSource = this.matchesDataSet;
			this.rootBindingSource.Position = 0;
			// 
			// matchesDataSet
			// 
			this.matchesDataSet.DataSetName = "MatchesDataSet";
			this.matchesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// matchesBindingSource
			// 
			this.matchesBindingSource.DataMember = "Matches";
			this.matchesBindingSource.DataSource = this.rootBindingSource;
			// 
			// effectTextDataGridViewTextBoxColumn
			// 
			this.effectTextDataGridViewTextBoxColumn.DataPropertyName = "EffectText";
			this.effectTextDataGridViewTextBoxColumn.HeaderText = "EffectText";
			this.effectTextDataGridViewTextBoxColumn.Name = "effectTextDataGridViewTextBoxColumn";
			// 
			// isMatchDataGridViewTextBoxColumn
			// 
			this.isMatchDataGridViewTextBoxColumn.DataPropertyName = "IsMatch";
			this.isMatchDataGridViewTextBoxColumn.HeaderText = "IsMatch";
			this.isMatchDataGridViewTextBoxColumn.Name = "isMatchDataGridViewTextBoxColumn";
			// 
			// effectNameDataGridViewTextBoxColumn
			// 
			this.effectNameDataGridViewTextBoxColumn.DataPropertyName = "EffectName";
			this.effectNameDataGridViewTextBoxColumn.HeaderText = "EffectName";
			this.effectNameDataGridViewTextBoxColumn.Name = "effectNameDataGridViewTextBoxColumn";
			// 
			// descriptorDataGridViewTextBoxColumn
			// 
			this.descriptorDataGridViewTextBoxColumn.DataPropertyName = "Descriptor";
			this.descriptorDataGridViewTextBoxColumn.HeaderText = "Descriptor";
			this.descriptorDataGridViewTextBoxColumn.Name = "descriptorDataGridViewTextBoxColumn";
			// 
			// conditionalPrefixDataGridViewTextBoxColumn
			// 
			this.conditionalPrefixDataGridViewTextBoxColumn.DataPropertyName = "ConditionalPrefix";
			this.conditionalPrefixDataGridViewTextBoxColumn.HeaderText = "ConditionalPrefix";
			this.conditionalPrefixDataGridViewTextBoxColumn.Name = "conditionalPrefixDataGridViewTextBoxColumn";
			// 
			// conditionDataGridViewTextBoxColumn
			// 
			this.conditionDataGridViewTextBoxColumn.DataPropertyName = "Condition";
			this.conditionDataGridViewTextBoxColumn.HeaderText = "Condition";
			this.conditionDataGridViewTextBoxColumn.Name = "conditionDataGridViewTextBoxColumn";
			// 
			// conditionalSuffixDataGridViewTextBoxColumn
			// 
			this.conditionalSuffixDataGridViewTextBoxColumn.DataPropertyName = "ConditionalSuffix";
			this.conditionalSuffixDataGridViewTextBoxColumn.HeaderText = "ConditionalSuffix";
			this.conditionalSuffixDataGridViewTextBoxColumn.Name = "conditionalSuffixDataGridViewTextBoxColumn";
			// 
			// remainderDataGridViewTextBoxColumn
			// 
			this.remainderDataGridViewTextBoxColumn.DataPropertyName = "Remainder";
			this.remainderDataGridViewTextBoxColumn.HeaderText = "Remainder";
			this.remainderDataGridViewTextBoxColumn.Name = "remainderDataGridViewTextBoxColumn";
			// 
			// Form1
			// 
			this.AcceptButton = this.matchButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1122, 710);
			this.Controls.Add(this.matchesGrid);
			this.Controls.Add(this.matchButton);
			this.Controls.Add(this.patternEditor);
			this.Controls.Add(this.patternLabel);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.matchesGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.inputsBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rootBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.matchesDataSet)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.matchesBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label patternLabel;
		private System.Windows.Forms.TextBox patternEditor;
		private System.Windows.Forms.Button matchButton;
		private System.Windows.Forms.DataGridView matchesGrid;
		private MatchesDataSet matchesDataSet;
		private System.Windows.Forms.BindingSource rootBindingSource;
		private System.Windows.Forms.BindingSource matchesBindingSource;
		private System.Windows.Forms.BindingSource inputsBindingSource;
		private System.Windows.Forms.DataGridViewTextBoxColumn effectTextDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn isMatchDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn effectNameDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptorDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionalPrefixDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionalSuffixDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn remainderDataGridViewTextBoxColumn;
	}
}

