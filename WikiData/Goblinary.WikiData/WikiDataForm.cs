namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public partial class WikiDataForm : Form
	{
		public WikiDataForm()
		{
			InitializeComponent();

			sourceDataSet.LookupDataSet = lookupDataSet;

			workDataSet.SourceDataSet = sourceDataSet;

			dataSets.Add(lookupDataSet);
			dataSets.Add(sourceDataSet);
			dataSets.Add(workDataSet);
			foreach (DataSet dataSet in dataSets)
			{
				dataSetDictionary[dataSet.DataSetName] = dataSet;
			}
		}

		private LookupDataSet lookupDataSet = new LookupDataSet();
		private SourceDataSet sourceDataSet = new SourceDataSet();
		private WorkDataSet workDataSet = new WorkDataSet();

		private List<DataSet> dataSets = new List<DataSet>();
		private Dictionary<string, DataSet> dataSetDictionary = new Dictionary<string, DataSet>();

		private DataSet SelectedDataSet => dataSetDictionary[dataSetSelector.SelectedItem.ToString()];

	    private void BindGrid()
		{
			dataTableSelector.Items.Clear();
		    if (SelectedDataSet != null)
		        dataTableSelector.Items.AddRange(SelectedDataSet.Tables.Cast<DataTable>().OrderBy(t => t.TableName)
		            .Select(t1 => t1.TableName).ToArray());
		    if (dataTableSelector.Items.Count > 0)
				dataTableSelector.SelectedIndex = 0;
		}

		private void PFOWikiDataForm_Load(object sender, EventArgs e)
		{
			dataSetSelector.Items.AddRange(dataSetDictionary.Keys.ToArray());
			dataSetSelector.SelectedIndex = 0;
		}

		private void dataSetSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			grid.DataSource = null;
			BindGrid();
		}

		private void dataTableSelector_SelectedIndexChanged(object sender, EventArgs e)
		{
			grid.DataSource = null;
			grid.DataSource = SelectedDataSet.Tables[dataTableSelector.SelectedItem.ToString()];
		}

		private async void mainMenu_Data_Import_Click(object sender, EventArgs e)
		{
			UseWaitCursor = true;
			try
			{
				grid.DataSource = null;
				IWikiDataSet dataSet = (IWikiDataSet)SelectedDataSet;
				await Task.Run(
					() =>
					{
						dataSet.Import();
					}
				);
				BindGrid();
			}
			finally
			{
				UseWaitCursor = false;
			}
		}

		private async void mainMenu_Data_Read_Click(object sender, EventArgs e)
		{
			UseWaitCursor = true;
			try
			{
				grid.DataSource = null;
				IWikiDataSet dataSet = (IWikiDataSet)SelectedDataSet;
				await Task.Run(
					() =>
					{
						dataSet.XRead();
					}
				);
				BindGrid();
			}
			finally
			{
				UseWaitCursor = false;
			}
		}

		private async void mainMenu_Data_ImportAll_Click(object sender, EventArgs e)
		{
			UseWaitCursor = true;
			try
			{
				grid.DataSource = null;
				await Task.Run(
					() =>
					{
						foreach (IWikiDataSet dataSet in dataSets)
						{
							dataSet.Import();
						}
					}
				);
				BindGrid();
			}
			finally
			{
				UseWaitCursor = false;
			}
		}

		private async void mainMenu_Data_ReadAll_Click(object sender, EventArgs e)
		{
			UseWaitCursor = true;
			try
			{
				grid.DataSource = null;
				await Task.Run(
					() =>
					{
						foreach (IWikiDataSet dataSet in dataSets)
						{
							dataSet.XRead();
						}
					}
				);
				BindGrid();
			}
			finally
			{
				UseWaitCursor = false;
			}
		}

		private async void mainMenu_Model_Process_Click(object sender, EventArgs e)
		{
			UseWaitCursor = true;
			try
			{
				await Task.Run(
					() =>
					{
						new ModelBuilder(lookupDataSet, workDataSet).Build();
					}
				);
				workDataSet.XSave();
				dataSetSelector.SelectedItem = "WorkDataSet";
				dataTableSelector.SelectedItem = "MissingFeats";
				MessageBox.Show(this, "Model.Process complete!");
			}
			finally
			{
				UseWaitCursor = false;
			}
		}
	}
}
