namespace Goblinary.Common
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	public enum RegexReplaceEmptyResultBehaviors
	{
		Ignore,
		Substitute,
		ThrowError
	}

	public static class Extensions
	{
		#region System

		#region String

		public static bool XIsNullOrEmpty(this string instance)
		{
			return string.IsNullOrEmpty(instance);
		}

		public static string XRegexReplaceSimple(this string instance, string pattern, string replacement)
		{
			return Regex.Replace(instance, pattern, replacement);
		}

		public static string XRegexReplace(this string instance, string pattern, string replacement, RegexReplaceEmptyResultBehaviors emptyResultBehavior = RegexReplaceEmptyResultBehaviors.Ignore, string substitution = null)
		{
			Regex regex = new Regex(pattern);
			return XRegexReplace(instance, regex, replacement, emptyResultBehavior, substitution);
		}

		public static string XRegexReplace(this string instance, Regex regex, string replacement, RegexReplaceEmptyResultBehaviors emptyResultBehavior = RegexReplaceEmptyResultBehaviors.Ignore, string substitution = null)
		{
			if (!regex.IsMatch(instance))
			{
				throw new Exception(string.Format("Pattern not matched.\r\n\tInput: '{0}'\r\n\tPattern: '{1}'", instance, regex.ToString()));
			}
			string result = regex.Replace(instance, replacement);
			if (result.XIsNullOrEmpty())
			{
				switch (emptyResultBehavior)
				{
					case RegexReplaceEmptyResultBehaviors.Substitute:
						result = substitution;
						break;
					case RegexReplaceEmptyResultBehaviors.ThrowError:
						throw new Exception(string.Format("Result is null or empty.\r\n\tInput: '{1}'\r\n\tPattern: '{2}'\r\n\tReplacement: '{0}'", replacement, instance, regex.ToString()));
				}
			}
			return result;
		}

		public static List<string> XGetRegexNames(this string instance)
		{
			string namePattern = @"\(\?\<(?<Name>[A-Za-z]*)\>";
			List<string> names = new List<string>();

			MatchCollection nameMatches = Regex.Matches(instance, namePattern);
			foreach (Match nameMatch in nameMatches)
			{
				names.Add(Regex.Replace(nameMatch.Value, namePattern, "${Name}"));
			}

			return names;
		}

		#endregion String

		#endregion System

		#region System.Data

		public static Dictionary<string, DataRow[]> XGetErrors(this DataSet instance)
		{
			Dictionary<string, DataRow[]> errors = new Dictionary<string, DataRow[]>();
			foreach (DataTable table in instance.Tables)
			{
				if (table.HasErrors)
				{
					errors[table.TableName] = table.GetErrors();
				}
			}
			return errors;
		}

		#endregion System.Data

		#region System.Windows.Forms

		#region BindingSource

		public static DataRow XGetCurrent(this BindingSource instance)
		{
			DataRow currentRow = null;
			if (instance.Current is DataRow)
			{
				currentRow = (DataRow)instance.Current;
			}
			else if (instance.Current is DataRowView)
			{
				currentRow = ((DataRowView)instance.Current).Row;
			}
			return currentRow;
		}

		public static DataRow XGetDataRow(this BindingSource instance, int rowIndex)
		{
			DataRow dataRow = null;
			if (rowIndex < instance.Count)
			{
				if (instance[rowIndex] is DataRow)
				{
					dataRow = (DataRow)instance[rowIndex];
				}
				else if (instance[rowIndex] is DataRowView)
				{
					dataRow = ((DataRowView)instance[rowIndex]).Row;
				}
			}
			return dataRow;
		}

		#endregion

		#region DataGridView

		public static DataRow XGetDataRow(this DataGridView instance, int rowIndex)
		{
			DataRow dataRow = null;
			if (instance.DataSource is BindingSource)
			{
				dataRow = ((BindingSource)instance.DataSource).XGetDataRow(rowIndex);
			}
			return dataRow;
		}

		#endregion DataGridView

		#region Form

		public static bool XCancelClose(this Form instance, bool hasChanges, string text = "Click OK to continue and lose your unsaved changes, or click Cancel to go back and save your changes before continuing.", string caption = "Lose Unsaved Changes?")
		{
			bool cancelClose = false;
			if (hasChanges)
			{
				cancelClose = MessageBox.Show(instance, text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Cancel;
			}
			return cancelClose;
		}

		public static void XHandleException(this Form instance, Exception ex, bool rethrow = false)
		{
			MessageBox.Show(instance, ex.Message, ex.GetType().FullName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			if (rethrow)
			{
				throw ex;
			}
		}

		public static void XPerform(this Form instance, MethodInvoker method, bool rethrow = false)
		{
			Cursor priorCursor = instance.Cursor;
			instance.Cursor = Cursors.WaitCursor;
			try
			{
				method();
			}
			catch (Exception ex)
			{
				instance.XHandleException(ex, rethrow);
			}
			finally
			{
				instance.Cursor = priorCursor;
			}
		}

		#endregion Form

		#endregion System.Windows.Forms
	}
}
