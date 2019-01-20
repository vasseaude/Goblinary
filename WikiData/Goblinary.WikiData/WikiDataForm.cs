namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	using Goblinary.Common;
	using Goblinary.WikiData.Model;

	public partial class WikiDataForm : Form
	{
		public WikiDataForm()
		{
			this.InitializeComponent();

			this.sourceDataSet.LookupDataSet = this.lookupDataSet;

			this.workDataSet.SourceDataSet = this.sourceDataSet;

			this.dataSets.Add(this.lookupDataSet);
			this.dataSets.Add(this.sourceDataSet);
			this.dataSets.Add(this.workDataSet);
			foreach (DataSet dataSet in this.dataSets)
			{
				this.dataSetDictionary[dataSet.DataSetName] = dataSet;
			}
		}

		private LookupDataSet lookupDataSet = new LookupDataSet();
		private SourceDataSet sourceDataSet = new SourceDataSet();
		private WorkDataSet workDataSet = new WorkDataSet();

		private List<DataSet> dataSets = new List<DataSet>();
		private Dictionary<string, DataSet> dataSetDictionary = new Dictionary<string, DataSet>();

		private DataSet SelectedDataSet
		{
			get
			{
				return this.dataSetDictionary[this.dataSetSelector.SelectedItem.ToString()];
			}
		}

		private void BindGrid()
		{
			this.dataTableSelector.Items.Clear();
			this.dataTableSelector.Items.AddRange((
				from DataTable t in this.SelectedDataSet.Tables
				orderby t.TableName
				select t.TableName).ToArray());
			if (this.dataTableSelector.Items.Count > 0)
				this.dataTableSelector.SelectedIndex = 0;
		}

		private void PFOWikiDataForm_Load(object sender, EventArgs e)
		{
			this.dataSetSelector.Items.AddRange(this.dataSetDictionary.Keys.ToArray());
			this.dataSetSelector.SelectedIndex = 0;
		}

		private void dataSetSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.grid.DataSource = null;
			this.BindGrid();
		}

		private void dataTableSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.grid.DataSource = null;
			this.grid.DataSource = this.SelectedDataSet.Tables[this.dataTableSelector.SelectedItem.ToString()];
		}

		private async void mainMenu_Data_Import_Click(object sender, EventArgs e)
		{
			this.UseWaitCursor = true;
			try
			{
				this.grid.DataSource = null;
				IWikiDataSet dataSet = (IWikiDataSet)this.SelectedDataSet;
				await Task.Run(
					() =>
					{
						dataSet.Import();
					}
				);
				this.BindGrid();
			}
			finally
			{
				this.UseWaitCursor = false;
			}
		}

		private async void mainMenu_Data_Read_Click(object sender, EventArgs e)
		{
			this.UseWaitCursor = true;
			try
			{
				this.grid.DataSource = null;
				IWikiDataSet dataSet = (IWikiDataSet)this.SelectedDataSet;
				await Task.Run(
					() =>
					{
						dataSet.XRead();
					}
				);
				this.BindGrid();
			}
			finally
			{
				this.UseWaitCursor = false;
			}
		}

		private async void mainMenu_Data_ImportAll_Click(object sender, EventArgs e)
		{
			this.UseWaitCursor = true;
			try
			{
				this.grid.DataSource = null;
				await Task.Run(
					() =>
					{
						foreach (IWikiDataSet dataSet in this.dataSets)
						{
							dataSet.Import();
						}
					}
				);
				this.BindGrid();
			}
			finally
			{
				this.UseWaitCursor = false;
			}
		}

		private async void mainMenu_Data_ReadAll_Click(object sender, EventArgs e)
		{
			this.UseWaitCursor = true;
			try
			{
				this.grid.DataSource = null;
				await Task.Run(
					() =>
					{
						foreach (IWikiDataSet dataSet in this.dataSets)
						{
							dataSet.XRead();
						}
					}
				);
				this.BindGrid();
			}
			finally
			{
				this.UseWaitCursor = false;
			}
		}

		private async void mainMenu_Model_Process_Click(object sender, EventArgs e)
		{
			this.UseWaitCursor = true;
			try
			{
				await Task.Run(
					() =>
					{
						new ModelBuilder(lookupDataSet, workDataSet).Build();
					}
				);
				this.workDataSet.XSave();
				this.dataSetSelector.SelectedItem = "WorkDataSet";
				this.dataTableSelector.SelectedItem = "MissingFeats";
				MessageBox.Show(this, "Model.Process complete!");
			}
			finally
			{
				this.UseWaitCursor = false;
			}
		}
	}
}
