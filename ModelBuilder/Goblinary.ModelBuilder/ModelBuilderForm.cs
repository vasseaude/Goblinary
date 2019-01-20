namespace Goblinary.ModelBuilder
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
	using Goblinary.Model;

	public partial class ModelBuilderForm : Form
	{
		public ModelBuilderForm()
		{
			this.InitializeComponent();

			this.sourceData.LookupData = this.lookupData;
			this.stockData.LookupData = this.lookupData;
			this.wikiData.LookupData = this.lookupData;
			this.wikiData.SourceData = this.sourceData;
			this.wikiData.StockData = this.stockData;

			this.dataSets.Add(this.lookupData);
			this.dataSets.Add(this.sourceData);
			this.dataSets.Add(this.stockData);
			this.dataSets.Add(this.wikiData);
			foreach (DataSet dataSet in this.dataSets)
			{
				this.dataSetDictionary[dataSet.DataSetName] = dataSet;
			}
		}

		private LookupData lookupData = new LookupData();
		private SourceData sourceData = new SourceData();
		private StockData stockData = new StockData();
		private WikiData wikiData = new WikiData();

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

		private void WikiDataForm_Load(object sender, EventArgs e)
		{
			this.dataSetSelector.Items.AddRange(this.dataSetDictionary.Keys.ToArray());
			this.dataSetSelector.SelectedItem = "WikiData";
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
				IWikiData dataSet = (IWikiData)this.SelectedDataSet;
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
				IWikiData dataSet = (IWikiData)this.SelectedDataSet;
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
						foreach (IWikiData dataSet in this.dataSets)
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
						foreach (IWikiData dataSet in this.dataSets)
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
						//new ModelBuilder(lookupData, workDataSet).Build();
					}
				);
				this.wikiData.XSave();
				this.dataSetSelector.SelectedItem = "WikiData";
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
