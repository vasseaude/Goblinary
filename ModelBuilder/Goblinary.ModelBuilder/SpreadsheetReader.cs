namespace Goblinary.ModelBuilder
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Google.GData.Client;
	using Google.GData.Extensions;
	using Google.GData.Spreadsheets;

	public class SpreadsheetReader
	{
		public SpreadsheetReader()
		{
			this.service = new SpreadsheetsService("Goblinary.ModelBuilder");
		}

		SpreadsheetsService service;

		public DataSet GetDataSet(string spreadsheetID)
		{
			DataSet dataSet = new DataSet();
			int worksheetID = 1;
			while (true)
			{
				DataTable table = this.GetDataTable(spreadsheetID, worksheetID++.ToString());
				if (table == null)
				{
					break;
				}
				dataSet.Tables.Add(table);
			}
			return dataSet;
		}

		public DataTable GetDataTable(string spreadsheetID, string worksheetID)
		{
			ListFeed feed;
			try
			{
				feed = this.service.Query(new ListQuery(spreadsheetID, worksheetID, "public", "values"));
			}
			catch (GDataRequestException)
			{
				return null;
			}
			DataTable table = new DataTable(feed.Title.Text);
			List<DataColumn> columns = new List<DataColumn>();
			foreach (ListEntry entry in feed.Entries)
			{
				DataRow row = table.NewRow();
				DataColumn column;
				int columnIndex = 0;
				foreach (ListEntry.Custom custom in entry.Elements)
				{
					if (table.Columns.Contains(custom.LocalName))
					{
						column = table.Columns[custom.LocalName];
					}
					else
					{
						column = table.Columns.Add(custom.LocalName);
						columns.Insert(columnIndex, column);
					}
					if (custom.Value != "")
					{
						row[column] = custom.Value;
					}
					columnIndex++;
				}
				table.Rows.Add(row);
			}
			bool needsColumnReordering = false;
			for (int i = 0; i < columns.Count; i++)
			{
				if (columns[i] != table.Columns[i])
				{
					needsColumnReordering = true;
					break;
				}
			}
			if (needsColumnReordering)
			{
				DataTable newTable = new DataTable(feed.Title.Text);
				foreach (DataColumn column in columns)
				{
					newTable.Columns.Add(column.ColumnName);
				}
				foreach (DataRow row in table.Rows)
				{
					DataRow newRow = newTable.NewRow();
					foreach (DataColumn column in columns)
					{
						newRow[column.ColumnName] = row[column.ColumnName];
					}
					newTable.Rows.Add(newRow);
				}
				table = newTable;
			}
			return table;
		}

		public void FillDataSet(DataSet dataSet, string spreadsheetID)
		{
			foreach (DataTable source in this.GetDataSet(spreadsheetID).Tables)
			{
				if (dataSet.Tables.Contains(source.TableName))
				{
					this.MergeTable(source, dataSet.Tables[source.TableName]);
				}
				else
				{
					dataSet.Merge(source);
				}
			}
		}

		public void FillDataTable(DataTable table, string spreadsheetID, string worksheetID)
		{
			this.MergeTable(this.GetDataTable(spreadsheetID, worksheetID), table);
		}

		private void MergeTable(DataTable source, DataTable target)
		{
			for (int i = 0; i < target.Columns.Count; i++)
			{
				source.Columns[i].ColumnName = target.Columns[i].ColumnName;
			}
			target.Merge(source);
		}
	}
}
