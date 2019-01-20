namespace Goblinary.Web
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Data.Common;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.HtmlControls;
	using System.Web.UI.WebControls;
	using System.Web.Profile;
	using System.Web.Security;

	using Goblinary.Common;

	public static class Extensions
	{
		private static List<DataControlField> GenerateFields(Type itemType, string dataFieldPrefix = null, string headerTextPrefix = null)
		{
			List<DataControlField> fields = new List<DataControlField>();
			foreach (var item in (
					from pi in itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					from pa in pi.GetCustomAttributes<PresentationAttribute>()
					orderby pa.DisplayOrder
					select new
					{
						PropertyInfo = pi,
						PresentationAttribute = pa
					}
				))
			{
				if (item.PresentationAttribute.PresentationType == PresentationTypes.Wrapper)
				{
					fields.AddRange(GenerateFields(item.PropertyInfo.PropertyType, item.PropertyInfo.Name, item.PresentationAttribute.DisplayName));
				}
				else
				{
					string dataField = string.Format("{0}{1}{2}", dataFieldPrefix, dataFieldPrefix == null ? null : ".", item.PropertyInfo.Name);
					string headerText = string.Format("{0}{1}{2}", headerTextPrefix, headerTextPrefix == null ? null : " ", item.PresentationAttribute.DisplayName ?? item.PropertyInfo.Name);
					if (item.PropertyInfo.PropertyType == typeof(bool))
					{
						CheckBoxField field = new CheckBoxField();
						field.DataField = dataField;
						field.HeaderText = headerText;
						fields.Add(field);
					}
					else
					{
						BoundField field = new BoundField();
						field.DataField = dataField;
						field.HeaderText = headerText;
						field.HtmlEncode = false;
						if (item.PropertyInfo.PropertyType == typeof(int?) || item.PropertyInfo.PropertyType == typeof(decimal?))
						{
							field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						}
						fields.Add(field);
					}
				}
			}
			return fields;
		}

		public static void XGenerateColumns(this GridView instance, Type itemType)
		{
			instance.AutoGenerateColumns = false;
			instance.CssClass = "tablesorter custom-popup";
			instance.Columns.Clear();
			foreach (DataControlField field in GenerateFields(itemType))
			{
				instance.Columns.Add(field);
			}
		}

		public static void XDataBind(this GridView instance, string tableName)
		{
			instance.DataBind();
			if (instance.HeaderRow != null)
			{
				instance.HeaderRow.TableSection = TableRowSection.TableHeader;
			}
			instance.Attributes.Add("tableName", tableName);
		}

		public static void XGenerateFields(this DetailsView instance, Type itemType)
		{
			instance.AutoGenerateRows = false;
			instance.Fields.Clear();
			foreach (DataControlField field in GenerateFields(itemType))
			{
				instance.Fields.Add(field);
			}
		}

		public static void XDataBind(this DetailsView instance)
		{
			instance.DataBind();
			if (instance.HeaderRow != null)
			{
				instance.HeaderRow.TableSection = TableRowSection.TableHeader;
			}
		}

		public static void XLoadNotes(this Page instance, HtmlGenericControl notes)
		{
			Table notesTable = (Table)instance.Page.LoadControl("~/Controls/TablesorterNotes.ascx").FindControl("TablesorterNotesTable");
			notes.Controls.Add(notesTable);
		}
	}
}
