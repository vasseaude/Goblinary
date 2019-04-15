
namespace Goblinary.WikiData
{
	using System.Collections.Generic;
	using System.Data;
	using Google.GData.Client;
	using Google.GData.Spreadsheets;
	using System.Linq;

    public class SpreadsheetReader
	{
		public SpreadsheetReader()
		{
			_service = new SpreadsheetsService("Nihimon.PFOData");
		}

	    readonly SpreadsheetsService _service;

		public DataSet GetDataSet(string spreadsheetId)
		{
			var dataSet = new DataSet();
			var worksheetId = 1;
			while (true)
			{
				var table = GetDataTable(spreadsheetId, worksheetId++.ToString());
			    if (table != null)
			        dataSet.Tables.Add(table);
			    else
			        break;
			}
			return dataSet;
		}

		public DataTable GetDataTable(string spreadsheetId, string worksheetId)
		{
			ListFeed feed;
			try
			{
				feed = _service.Query(new ListQuery(spreadsheetId, worksheetId, "public", "values"));
			}
			catch (GDataRequestException)
			{
				return null;
			}
			var table = new DataTable(feed.Title.Text);
			var columns = new List<DataColumn>();
			foreach (var atomEntry in feed.Entries)
			{
			    var entry = (ListEntry) atomEntry;
			    var row = table.NewRow();
			    var columnIndex = 0;
				foreach (ListEntry.Custom custom in entry.Elements)
				{
				    DataColumn column;
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
			var needsColumnReordering = columns.Where((t, i) => t != table.Columns[i]).Any();
		    if (!needsColumnReordering) return table;
		    {
		        var newTable = new DataTable(feed.Title.Text);
		        foreach (var column in columns)
		        {
		            newTable.Columns.Add(column.ColumnName);
		        }
		        foreach (DataRow row in table.Rows)
		        {
		            var newRow = newTable.NewRow();
		            foreach (var column in columns)
		            {
		                newRow[column.ColumnName] = row[column.ColumnName];
		            }
		            newTable.Rows.Add(newRow);
		        }
		        table = newTable;
		    }
		    return table;
		}

		public void FillDataSet(DataSet dataSet, string spreadsheetId)
		{
			foreach (DataTable source in GetDataSet(spreadsheetId).Tables)
			{
				if (dataSet.Tables.Contains(source.TableName))
				{
					MergeTable(source, dataSet.Tables[source.TableName]);
				}
				else
				{
					dataSet.Merge(source);
				}
			}
		}

		public void FillDataTable(DataTable table, string spreadsheetId, string worksheetId)
		{
			MergeTable(GetDataTable(spreadsheetId, worksheetId), table);
		}

		private void MergeTable(DataTable source, DataTable target)
		{
			for (var i = 0; i < target.Columns.Count; i++)
			{
				source.Columns[i].ColumnName = target.Columns[i].ColumnName;
			}
			target.Merge(source);
		}
	}
}
